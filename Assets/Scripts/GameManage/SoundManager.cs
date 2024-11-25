using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�Q�[�����Ŏg�p����T�E���h���ꌳ�Ǘ����܂�
public class SoundManager : MonoBehaviour
{
    //�I�[�f�B�I�\�[�X
    [SerializeField]
    AudioSource GameBGM,   //BGM
                GameSE;    //SE

    //SE�w��
    const int Coin = 1, Explosion = 2, Button = 3;
    const int CLIP_SE_VALUE = 20;

    // �C���X�^���X�̐���
    private static SoundManager _instance;

    public static SoundManager instance
    {
        get
        {if(_instance == null) _instance = new SoundManager();
            return _instance;
        }
    }

    //����(�N���b�v)
    [SerializeField]
    AudioClip
        CoinSE,           //�R�C���l��SE      
        KeySE,            //���l��SE
        ExplosionSE,      //����SE
        WaterSE,          //����SE
        ButtonSE,         //�{�^������SE
        JumpSE,           //�W�����vSE
        LandingSE,        //���nSE
        FootStepSE,       //����SE(���s�A���s��)
        RecoverySE,       //�̗͉�SE
        StageClearSE,     //�X�e�[�W�N���ASE
        GameOverSE,       //�Q�[���I�[�o�[SE
        SystemButtonSE,   //�Q�[���{�^��SE
        BruteHitSE,       //�G(Brute)�U���q�b�gSE
        SlimeHitSE,       //�G(Slime)�U���q�b�gSE
        BruteAttackSE,    //�G(Brute)���U��SE
        SlimeAttackSE,    //�G(Brute)���U��
        EnemyDiedSE;      //�G�̕m����SE

    [SerializeField]AudioClip
        TitleBGM,         //�^�C�g����ʂ�BGM
        IntialBGM,        //�X�e�[�W1,2��BGM
        Stage3BGM,        //�X�e�[�W3��BGM
        Stage4BGM,        //�X�e�[�W4��BGM
        BossBGM,          //�{�XBGM
        GameClearBGM;     //�Q�[���N���ABGM

    //�Đ��t���O
    [HideInInspector]   //Unity�̃C���X�y�N�^�[��ɕ\�����Ȃ�(�X�N���v�g���ŏ���)
    public bool BGMFlag;
    private bool IsPlay;    //�����p�A���ɍĐ����̏ꍇ�ɕ����Đ����Ȃ��悤�ɂ���

    //���݂̃V�[���擾
    private string NowScene;

    void Start()
    {
        //�I�[�f�B�I�\�[�X�̓C���X�y�N�^�[��Ŏw��
        BGMFlag = false;

        DontDestroyOnLoad(this);

        GameSE.loop = false;    //���[�v�Đ����Ȃ��悤�ɂ��Ă���
        NowScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        //SystemManager����BGM���Đ����邽�߁A�����Ŏ��s
        if(BGMFlag == false)
        {
            PlayBGM(NowScene);
        }
    }

    /// <summary>
    /// BGM�̍Đ�����
    /// </summary>
    /// <param name="NowSceneName">���݂̃V�[����</param>
    public void PlayBGM(string NowSceneName)
    {
        GameBGM.loop = true;    //���[�v�Đ�����悤�ɂ��Ă���

        //�V�[�����Ƃ�BGM���Đ����܂�
        if (NowSceneName == "GameTitle")
        {
            GameBGM.clip = TitleBGM;
            GameBGM.Play();
            BGMFlag = true;
        }
        else if (NowSceneName == "Stage1" || NowSceneName == "Stage2")
        {
            GameBGM.clip = IntialBGM;
            GameBGM.Play();
            BGMFlag = true;
        }

        else if (NowSceneName == "Stage3")
        {
            GameBGM.clip = Stage3BGM;
            GameBGM.Play();
            BGMFlag = true;
        }
        else if (NowSceneName == "Stage4")
        {
            GameBGM.clip = Stage4BGM;
            GameBGM.Play();
            BGMFlag = true;
        }
        else if (NowSceneName == "Stage5")
        {
            GameBGM.clip = BossBGM;
            GameBGM.Play();
            BGMFlag = true;
        }
        else if (NowSceneName == "GameClear")
        {
            GameBGM.clip = GameClearBGM;
            GameBGM.Play();
            BGMFlag = true;
        }
        return;
    }

    /// <summary>
    /// SE�̍Đ����s��(�O���Ăяo��)
    /// </summary>
    /// <param name="StateName">�Đ����鉹���擾</param>
    public void PlaySE(string StateName)
    {
        if(StateName == "Coin")
            GameSE.PlayOneShot(CoinSE);
     
        if(StateName =="GunShot")
        {
            GameSE.volume = 0.50f;
            GameSE.PlayOneShot(ExplosionSE);
        }
      
        if(StateName == "Water")
            GameSE.PlayOneShot(WaterSE);
        
        if (StateName == "Button")
            GameSE.PlayOneShot(ButtonSE);
        
        if(StateName == "Jump")
            GameSE.PlayOneShot(JumpSE);

        if(StateName == "Landing")
            GameSE.PlayOneShot(LandingSE);
     
        if(StateName == "GetKey")
            GameSE.PlayOneShot(KeySE);
        
        if(StateName == "Recovery")
            GameSE.PlayOneShot(RecoverySE);
     
        if (StateName == "SystemButton")
            GameSE.PlayOneShot(SystemButtonSE);
        
        if(StateName == "StageClear")
        {
            GameBGM.Stop();
            GameBGM.PlayOneShot(StageClearSE);
        }

        if(StateName == "GameOver")
        {
            GameBGM.Stop();
            GameSE.PlayOneShot(GameOverSE);
        }

        if(StateName == "BruteHitAttack")
            GameSE.PlayOneShot(BruteHitSE);
        
        if(StateName == "SlimeHitAttack")
            GameSE.PlayOneShot(SlimeHitSE);
 
        if (StateName == "BruteAttack")
        {
            GameSE.clip = BruteAttackSE;
            GameSE.Play();
        }

        if(StateName == "SlimeAttack")
        {
            GameSE.clip = SlimeAttackSE;
            GameSE.Play();
        }

        if (StateName == "EnemyDied")
        {
            GameSE.clip = EnemyDiedSE;
            GameSE.Play();
        }
    }
}
