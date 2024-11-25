using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

/// <summary>
/// PV�Đ����̃V�[�����̏���
/// </summary>
public class ScenePV : MonoBehaviour
{
    [SerializeField]Image FadePanel;
    [SerializeField] VideoPlayer GamePV;
    float FadeDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        GamePV.loopPointReached += LoopPointReached;
        GamePV.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //���炩�̑��삪���邩�A�Đ����I�������Ƃ��t�F�[�h�A�E�g���ă^�C�g����ʂɑJ��
        if(Input.anyKey)
        {
            StartCoroutine("FadeOutAndLoadScene");
        }
    }

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
        SceneManager.LoadScene("TitleMenu"); // �V�[�������[�h���ă��j���[�V�[���ɑJ��
    }

    public void LoopPointReached(VideoPlayer pv)
    {
        SceneManager.LoadScene("MainMenu"); // �V�[�������[�h���ă��j���[�V�[���ɑJ��
    }
}
