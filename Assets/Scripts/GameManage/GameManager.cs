using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

/// <summary>
/// �Q�[���̃N���A����Ȃǂ̏�Ԑݒ�
/// </summary>
public class GameManager : MonoBehaviour
{
    // �G�t�F�N�g���錾
    private readonly string clear = "StageClear"; 

    //�Q�[���̐i�s���
    public enum GameState
    {
        Title,
        InGame,      // �i�s��
        Pause,       // �|�[�Y
        StageClear,  // �X�e�[�W�N���A
        Result,
    };

    //�����X�e�[�g�ݒ�
    [SerializeField]
    private GameState NowGameState = GameState.InGame;

    [SerializeField] protected GameObject PauseUI = null;
    [SerializeField] private GameObject clearUI = null;
    [SerializeField] private GameObject gameOverUI = null;

    [SerializeField]
    private mainGameUI gameUI = null;
    [SerializeField]
    private SoundManager sound = null;

    //�V�[���擾
    Scene NowScene;

    [SerializeField]
    private Transform goalTrans = null;
    [SerializeField]
    private Player _player = null;
    [SerializeField]
    private EffectManager effect = null;

    private void Start()
    {
        // �t���[���Œ�
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
        // �V�[���擾
        NowScene = SceneManager.GetActiveScene();

        if(NowGameState == GameState.Title)
            sound.PlayBGM("GameTitle");

        // �C���Q�[�����̂݊m�F�������
        if (NowGameState == GameState.InGame)
        {
            Initialized();
            PauseCheck();
        }
    }

    private void Initialized()
    {
        // �v���C���[������
        _player.Initialized();
        // UI�����ݒ�
        gameUI.Initialized();
        Player.GameClearCallBack(gameResultClear);
        Player.GameOverCallBack(gameResultMiss);
    }

    private void FixedUpdate()
    {
        GameStateCheck();
        // �C���Q�[�����̂ݏ����������
        if (NowGameState == GameState.InGame)
        {
            gameUI.CharaGauge();
            PauseCheck();
            getPlayerControll();
        }
    }

    /// <summary>
    /// �|�[�Y�{�^�������`�F�b�N
    /// </summary>
    private void PauseCheck()
    {
        //�G�X�P�[�v�L�[�����͂��ꂽ��
        if (Input.GetKeyDown(KeyCode.Escape))
            NowGameState = GameManager.GameState.Pause;
    }

    /// <summary>
    /// �Q�[����ԃ`�F�b�N
    /// </summary>
    private void GameStateCheck()
    {
        //�Q�[���̏�ԕ���(�Q�[�������|�[�Y����)
        switch (NowGameState)
        {
            case GameState.InGame:      //�P�[�X�P�F�C���Q�[��
                Time.timeScale = 1;
                PauseUI.SetActive(false);
                break;

            case GameState.Pause:       //�P�[�X�Q�F�|�[�Y��
                Time.timeScale = 0;
                PauseUI.SetActive(true);
                break;

            case GameState.Result:
                break;
        }

    }

    /// <summary>
    /// �N���A�`�F�b�N
    /// ���������Ă�����N���A �����łȂ���Ή������Ȃ�
    /// </summary>
    /// <param name="player"></param>
    private void gameResultClear(Player player)
    {
        // �N���A�`�F�b�N
        if (!_player.keyFlag) return;

        // �Q�[���N���A�`�F�b�N
        if (NowScene.name == "Stage5")
        {
            SceneManager.LoadScene("GameClear");
            return;
        }

        clearUI.SetActive(true);
        sound.PlaySE(clear);
        effect.PlayEffect(clear,goalTrans.position);
        _player.enabled = false;
        NowGameState = GameState.Result;
    }

    /// <summary>
    /// �|�[�Y��ʂ��畜�A���Ɏg�p
    /// </summary>
    public void backInGame()
    {
        NowGameState = GameState.InGame;
    }

    private void gameResultMiss(Player player)
    {
        gameOverUI.SetActive(true);
        sound.PlaySE("GameOver");
        _player.enabled = false;
        NowGameState = GameState.Result;
    }
    private void getPlayerControll()
    {
        _player.PlayerControll();
    }
}
