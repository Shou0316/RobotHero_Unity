using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;      //シーン名取得に使用

/// <summary>
/// 画面GUIの表示設定
/// </summary>
public class mainGameUI : MonoBehaviour
{
    // 体力テキスト(プレイヤー用)
    [SerializeField] private TextMeshProUGUI playerTextHP;

    [Header ("体力ゲージ")]
    [SerializeField] Slider playerGauge,      //プレイヤー
                            bossGauge;        //ボス

    [Header("コイン・カギ")]
    [SerializeField]
    private GameObject[] CoinUI;
    [SerializeField]
    private GameObject KeyUI;
    [SerializeField]
    private GameObject CompleteText;

    [Header("キャラクター情報取得")]
    [SerializeField] GameObject Boss;
    BossData bossData;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private SoundManager sound = null;

    //-------体力ゲージ関連-------
    private int 
        minPlayerHp = 0,          //体力ゲージの最小値
        maxPlayerHp = 0,          //体力ゲージの最大値
        nowPlayerHp = 0;          //体力ゲージの現状値

    private int 
        minBossHP = 0,
        maxBossHP = 0,
        nowBossHP = 0;

    // テキストカラー
    private readonly Color White = new Color(255, 255, 255);
    private readonly Color Yellow = new Color(255, 161, 0);
    private readonly Color Red = new Color(255, 0, 0);
    
    private string getNowScene = null;
    private int getCoinNum = 0;

    public void Initialized()
    {
        //シーン名取得
        getNowScene = SceneManager.GetActiveScene().name;
        setPlayerGauge();

        // ボス関連取得 
        if (getNowScene == "Stage5" || getNowScene == "Stage3")
        {
            bossData = Boss.GetComponent<BossData>();
            setBossHPGauge();
        }
    }

    /// <summary>
    /// プレイヤーのゲージ設定
    /// </summary>
    private void setPlayerGauge()
    {
        if (playerGauge == null) return;

        //プレイヤー体力ゲージ設定
        playerGauge.minValue = 0;
        playerGauge.maxValue = _player.HP;
        playerGauge.value = _player.HP;
    }

    /// <summary>
    /// 中ボス、ボスのHPゲージ設定
    /// </summary>
    private void setBossHPGauge()
    {
        switch (getNowScene)
        {
            case "Stage3":
                maxBossHP = bossData.SetMiniBossHP;
                nowBossHP = bossData.SetMiniBossHP;
                break;
            case "Stage5":
                maxBossHP = bossData.SetBossHP;
                nowBossHP = bossData.SetBossHP;
                break;
        }

        minBossHP = 0;
        //カウンターに反映
        bossGauge.minValue = minBossHP;
        bossGauge.maxValue = maxBossHP;
        bossGauge.value = nowBossHP;
    }

    /// <summary>
    /// 獲得したアイテムのUIを表示する
    /// </summary>
    public void GetItemUI(string getItemName)
    {
        // コインの獲得を受け取ったら、応じたUIを表示する
        if (getItemName == "Coin")
        {
            sound.PlaySE(getItemName);
            CoinUI[getCoinNum].SetActive(true);
            getCoinNum++;
        }

        // コインがコンプリートしていたら
        if (getCoinNum >= 3) CompleteText.SetActive(true);

        // カギの獲得を受け取ったら
        if (getItemName == "Key")
        {
            sound.PlaySE(getItemName);
            KeyUI.SetActive(true);
        }
    }

    /// <summary>
    /// 体力ゲージ関連の処理
    /// </summary>
    public void CharaGauge()
    {
        // 一応チェック プレイヤーがnullなら何もしない
        if (_player == null) return;

        playerGauge.value = _player.HP;
        playerTextHP.text = _player.HP + " / " + _player.getMaxHP();

        if (_player.HP >= 6)
            playerTextHP.color = White;
        if (_player.HP <= 5)
            playerTextHP.color = Yellow;
        if (_player.HP <= 3)
            playerTextHP.color = Red;
    }

    // ボスゲージ
    public void BossGaugeClass()
    {
        bossGauge.gameObject.SetActive(true);
        bossGauge.value = bossData.BossHP;
        
        // 体力が0なら表示をしない
        if(bossGauge.value < 0)
            bossGauge.gameObject.SetActive(false);
    }
}
