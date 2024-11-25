using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ゲーム内で使用するサウンドを一元管理します
public class SoundManager : MonoBehaviour
{
    //オーディオソース
    [SerializeField]
    AudioSource GameBGM,   //BGM
                GameSE;    //SE

    //SE指定
    const int Coin = 1, Explosion = 2, Button = 3;
    const int CLIP_SE_VALUE = 20;

    // インスタンスの生成
    private static SoundManager _instance;

    public static SoundManager instance
    {
        get
        {if(_instance == null) _instance = new SoundManager();
            return _instance;
        }
    }

    //音源(クリップ)
    [SerializeField]
    AudioClip
        CoinSE,           //コイン獲得SE      
        KeySE,            //鍵獲得SE
        ExplosionSE,      //爆発SE
        WaterSE,          //水中SE
        ButtonSE,         //ボタン操作SE
        JumpSE,           //ジャンプSE
        LandingSE,        //着地SE
        FootStepSE,       //足音SE(歩行、走行時)
        RecoverySE,       //体力回復SE
        StageClearSE,     //ステージクリアSE
        GameOverSE,       //ゲームオーバーSE
        SystemButtonSE,   //ゲームボタンSE
        BruteHitSE,       //敵(Brute)攻撃ヒットSE
        SlimeHitSE,       //敵(Slime)攻撃ヒットSE
        BruteAttackSE,    //敵(Brute)側攻撃SE
        SlimeAttackSE,    //敵(Brute)側攻撃
        EnemyDiedSE;      //敵の瀕死時SE

    [SerializeField]AudioClip
        TitleBGM,         //タイトル画面のBGM
        IntialBGM,        //ステージ1,2のBGM
        Stage3BGM,        //ステージ3のBGM
        Stage4BGM,        //ステージ4のBGM
        BossBGM,          //ボスBGM
        GameClearBGM;     //ゲームクリアBGM

    //再生フラグ
    [HideInInspector]   //Unityのインスペクター上に表示しない(スクリプト内で処理)
    public bool BGMFlag;
    private bool IsPlay;    //足音用、既に再生中の場合に複数再生しないようにする

    //現在のシーン取得
    private string NowScene;

    void Start()
    {
        //オーディオソースはインスペクター上で指定
        BGMFlag = false;

        DontDestroyOnLoad(this);

        GameSE.loop = false;    //ループ再生しないようにしておく
        NowScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        //SystemManager内でBGMを再生するため、ここで実行
        if(BGMFlag == false)
        {
            PlayBGM(NowScene);
        }
    }

    /// <summary>
    /// BGMの再生処理
    /// </summary>
    /// <param name="NowSceneName">現在のシーン名</param>
    public void PlayBGM(string NowSceneName)
    {
        GameBGM.loop = true;    //ループ再生するようにしておく

        //シーンごとにBGMを再生します
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
    /// SEの再生を行う(外部呼び出し)
    /// </summary>
    /// <param name="StateName">再生する音を取得</param>
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
