using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//��ʂ̈Ó]���]����
public class FadeInOut : MonoBehaviour
{
    // �t�F�[�h�p��UI�p�l���iImage�j
    [SerializeField]
    private Image FadePanel;
    [SerializeField]
    private Image GameRule;
    [SerializeField]
    private Image OperationDescription;

    // �t�F�[�h�C���E�A�E�g�̊����ɂ����鎞��
    public float FadeDuration = 1.0f;

    private bool FadeFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        FadePanel = GameObject.FindWithTag("FadeUI").GetComponent<Image>();
        StartCoroutine("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "OperationDescription")
        {
            GameStart();
        }
    }

    void GameStart()
    {
        if (Input.anyKeyDown)
        {
            if (OperationDescription.gameObject.activeSelf)
                StartCoroutine("FadeOut");

            GameRule.gameObject.SetActive(false);
            OperationDescription.gameObject.SetActive(true);
        }
    }

    //�t�F�[�h�C��(������薾�])
    public IEnumerator FadeIn()
    {
        FadePanel.enabled = true;                 // �p�l����L����
        float ElapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color StartColor = new Color(FadePanel.color.r,FadePanel.color.g,FadePanel.color.b,1.0f);       // �t�F�[�h�p�l���̊J�n�F���擾
        Color EndColor = new Color(StartColor.r, StartColor.g, StartColor.b, 0.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        while (ElapsedTime < FadeDuration)
        {
            ElapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(ElapsedTime / FadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            FadePanel.color = Color.Lerp(StartColor, EndColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        FadePanel.color = EndColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
        FadePanel.enabled = false;                 // �p�l����L����

    }

    //�t�F�[�h�A�E�g(�������Ó])
    public IEnumerator FadeOut()
    {
        FadePanel.enabled = true;                 // �p�l����L����
        FadeFlag = false;
        float ElapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color StartColor = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, 0.0f);       // �t�F�[�h�p�l���̊J�n�F���擾
        Color EndColor = new Color(StartColor.r, StartColor.g, StartColor.b, 1.0f); // �t�F�[�h�p�l���̍ŏI�F��ݒ�

        while (ElapsedTime < FadeDuration)
        {
            ElapsedTime += Time.deltaTime;                        // �o�ߎ��Ԃ𑝂₷
            float t = Mathf.Clamp01(ElapsedTime / FadeDuration);  // �t�F�[�h�̐i�s�x���v�Z
            FadePanel.color = Color.Lerp(StartColor, EndColor, t); // �p�l���̐F��ύX���ăt�F�[�h�A�E�g
            yield return null;                                     // 1�t���[���ҋ@
        }

        FadePanel.color = EndColor;  // �t�F�[�h������������ŏI�F�ɐݒ�
        SceneManager.LoadScene("PlayPV"); // �V�[�������[�h���ă��j���[�V�[���ɑJ��

        //�Ó]��A�X�e�[�W1�Ɉړ�����
        SceneManager.LoadScene("Stage1");
    }
}
