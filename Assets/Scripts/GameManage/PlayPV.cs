using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayPV : MonoBehaviour
{
    //�V�[�����̌o�ߎ���
    float SceneTime = 0.0f;
    //PV�V�[���؂�ւ��^�C��
    float TransitionTime = 15.0f;
    // �t�F�[�h�p��UI�p�l���iImage�j
    public Image FadePanel;         
    // �t�F�[�h�A�E�g�̊����ɂ����鎞��
    public float FadeDuration = 0.5f;

    private void Start()
    {
        FadePanel.color = new Color(FadePanel.color.r,FadePanel.color.g,FadePanel.color.b, 1.0f);
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        SceneTime += Time.deltaTime;

        //�������삪�Ȃ��A�J�ڃ^�C���ɒB�����Ƃ�
        if(SceneTime > TransitionTime)
        {
            //�t�F�[�h�A�E�g�����̂��APV���Đ�����
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    //�t�F�[�h�A�E�g
    public IEnumerator FadeOutAndLoadScene()
    {
        FadePanel.enabled = true;                 // �p�l����L����
        float ElapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color StartColor = FadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
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
    }

    //�t�F�[�h�C��
    public IEnumerator FadeIn()
    {
        FadePanel.enabled = true;                 // �p�l����L����
        float ElapsedTime = 0.0f;                 // �o�ߎ��Ԃ�������
        Color StartColor = FadePanel.color;       // �t�F�[�h�p�l���̊J�n�F���擾
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
}
