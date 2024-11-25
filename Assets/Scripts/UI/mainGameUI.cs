using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;      //�V�[�����擾�Ɏg�p

/// <summary>
/// ���GUI�̕\���ݒ�
/// </summary>
public class mainGameUI : MonoBehaviour
{
    // �̗̓e�L�X�g(�v���C���[�p)
    [SerializeField] private TextMeshProUGUI playerTextHP;

    [Header ("�̗̓Q�[�W")]
    [SerializeField] Slider playerGauge,      //�v���C���[
                            bossGauge;        //�{�X

    [Header("�R�C���E�J�M")]
    [SerializeField]
    private GameObject[] CoinUI;
    [SerializeField]
    private GameObject KeyUI;
    [SerializeField]
    private GameObject CompleteText;

    [Header("�L�����N�^�[���擾")]
    [SerializeField] GameObject Boss;
    BossData bossData;
    [SerializeField]
    private Player _player;
    [SerializeField]
    private SoundManager sound = null;

    //-------�̗̓Q�[�W�֘A-------
    private int 
        minPlayerHp = 0,          //�̗̓Q�[�W�̍ŏ��l
        maxPlayerHp = 0,          //�̗̓Q�[�W�̍ő�l
        nowPlayerHp = 0;          //�̗̓Q�[�W�̌���l

    private int 
        minBossHP = 0,
        maxBossHP = 0,
        nowBossHP = 0;

    // �e�L�X�g�J���[
    private readonly Color White = new Color(255, 255, 255);
    private readonly Color Yellow = new Color(255, 161, 0);
    private readonly Color Red = new Color(255, 0, 0);
    
    private string getNowScene = null;
    private int getCoinNum = 0;

    public void Initialized()
    {
        //�V�[�����擾
        getNowScene = SceneManager.GetActiveScene().name;
        setPlayerGauge();

        // �{�X�֘A�擾 
        if (getNowScene == "Stage5" || getNowScene == "Stage3")
        {
            bossData = Boss.GetComponent<BossData>();
            setBossHPGauge();
        }
    }

    /// <summary>
    /// �v���C���[�̃Q�[�W�ݒ�
    /// </summary>
    private void setPlayerGauge()
    {
        if (playerGauge == null) return;

        //�v���C���[�̗̓Q�[�W�ݒ�
        playerGauge.minValue = 0;
        playerGauge.maxValue = _player.HP;
        playerGauge.value = _player.HP;
    }

    /// <summary>
    /// ���{�X�A�{�X��HP�Q�[�W�ݒ�
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
        //�J�E���^�[�ɔ��f
        bossGauge.minValue = minBossHP;
        bossGauge.maxValue = maxBossHP;
        bossGauge.value = nowBossHP;
    }

    /// <summary>
    /// �l�������A�C�e����UI��\������
    /// </summary>
    public void GetItemUI(string getItemName)
    {
        // �R�C���̊l�����󂯎������A������UI��\������
        if (getItemName == "Coin")
        {
            sound.PlaySE(getItemName);
            CoinUI[getCoinNum].SetActive(true);
            getCoinNum++;
        }

        // �R�C�����R���v���[�g���Ă�����
        if (getCoinNum >= 3) CompleteText.SetActive(true);

        // �J�M�̊l�����󂯎������
        if (getItemName == "Key")
        {
            sound.PlaySE(getItemName);
            KeyUI.SetActive(true);
        }
    }

    /// <summary>
    /// �̗̓Q�[�W�֘A�̏���
    /// </summary>
    public void CharaGauge()
    {
        // �ꉞ�`�F�b�N �v���C���[��null�Ȃ牽�����Ȃ�
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

    // �{�X�Q�[�W
    public void BossGaugeClass()
    {
        bossGauge.gameObject.SetActive(true);
        bossGauge.value = bossData.BossHP;
        
        // �̗͂�0�Ȃ�\�������Ȃ�
        if(bossGauge.value < 0)
            bossGauge.gameObject.SetActive(false);
    }
}
