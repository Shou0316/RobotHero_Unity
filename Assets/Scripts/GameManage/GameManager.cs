using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;

/// <summary>
/// ゲームのクリア判定などの状態設定
/// </summary>
public class GameManager : MonoBehaviour
{
    // エフェクト名宣言
    private readonly string clear = "StageClear"; 

    //ゲームの進行状態
    public enum GameState
    {
        Title,
        InGame,      // 進行中
        Pause,       // ポーズ
        StageClear,  // ステージクリア
        Result,
    };

    //初期ステート設定
    [SerializeField]
    private GameState NowGameState = GameState.InGame;

    [SerializeField] protected GameObject PauseUI = null;
    [SerializeField] private GameObject clearUI = null;
    [SerializeField] private GameObject gameOverUI = null;

    [SerializeField]
    private mainGameUI gameUI = null;
    [SerializeField]
    private SoundManager sound = null;

    //シーン取得
    Scene NowScene;

    [SerializeField]
    private Transform goalTrans = null;
    [SerializeField]
    private Player _player = null;
    [SerializeField]
    private EffectManager effect = null;

    private void Start()
    {
        // フレーム固定
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 90;
        // シーン取得
        NowScene = SceneManager.GetActiveScene();

        if(NowGameState == GameState.Title)
            sound.PlayBGM("GameTitle");

        // インゲーム時のみ確認するもの
        if (NowGameState == GameState.InGame)
        {
            Initialized();
            PauseCheck();
        }
    }

    private void Initialized()
    {
        // プレイヤー初期化
        _player.Initialized();
        // UI初期設定
        gameUI.Initialized();
        Player.GameClearCallBack(gameResultClear);
        Player.GameOverCallBack(gameResultMiss);
    }

    private void FixedUpdate()
    {
        GameStateCheck();
        // インゲーム時のみ処理するもの
        if (NowGameState == GameState.InGame)
        {
            gameUI.CharaGauge();
            PauseCheck();
            getPlayerControll();
        }
    }

    /// <summary>
    /// ポーズボタン押下チェック
    /// </summary>
    private void PauseCheck()
    {
        //エスケープキーが入力されたら
        if (Input.GetKeyDown(KeyCode.Escape))
            NowGameState = GameManager.GameState.Pause;
    }

    /// <summary>
    /// ゲーム状態チェック
    /// </summary>
    private void GameStateCheck()
    {
        //ゲームの状態分岐(ゲーム中かポーズ中か)
        switch (NowGameState)
        {
            case GameState.InGame:      //ケース１：インゲーム
                Time.timeScale = 1;
                PauseUI.SetActive(false);
                break;

            case GameState.Pause:       //ケース２：ポーズ中
                Time.timeScale = 0;
                PauseUI.SetActive(true);
                break;

            case GameState.Result:
                break;
        }

    }

    /// <summary>
    /// クリアチェック
    /// 鍵を持っていたらクリア そうでなければ何もしない
    /// </summary>
    /// <param name="player"></param>
    private void gameResultClear(Player player)
    {
        // クリアチェック
        if (!_player.keyFlag) return;

        // ゲームクリアチェック
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
    /// ポーズ画面から復帰時に使用
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
