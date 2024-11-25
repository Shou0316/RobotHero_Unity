using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//�V�[���J�ڂȂǃV�X�e���֘A�̃{�^���X�N���v�g
public class SystemButton : MonoBehaviour
{

    //�t�F�[�h�A�E�g�̎���
    const float FadeTime = 1.0f;
    //���[�v��
    const int LoopCount = 20;

    protected Scene NowScene;
    Transform MyButton;
    //�{�^���T�C�Y
    const float NormalButtonScale = 3.2f;
    const float OnMouseButtonScale = 3.5f;
    
    //�I�u�W�F�N�g�擾
    [SerializeField] GameObject FadeObj;                //�t�F�[�h�I�u�W�F�N�g
    [SerializeField] GameObject OperationMethodObj;     //������@�I�u�W�F�N�g
    [SerializeField]
    private GameManager gameManager;
    [SerializeField] SoundManager sound;            //�T�E���h�I�u�W�F�N�g


    private void Start()
    {
        MyButton = this.GetComponent<Transform>();
        //FadeObj.SetActive(false);
    }

    void OnMouseOver()
    {
        //�{�^���T�C�Y
        MyButton.localScale = new Vector3(OnMouseButtonScale, OnMouseButtonScale, OnMouseButtonScale);
    }

    void OnMouseExit()
    {
        MyButton.localScale = new Vector3(NormalButtonScale, NormalButtonScale, NormalButtonScale);
    }

    //���ꂼ��Ή�����I�u�W�F�N�g(�{�^��)�̑���ŃV�[���J��
    protected void OnClick()
    {
        //SE�Đ�
        sound.PlaySE("SystemButton");

        //�Q�[���X�^�[�g
        if (this.gameObject.name == "GameStartButton")
        {
            StartCoroutine("FadeOut");

            SceneManager.LoadScene("OperationDescription");
        }

        // ���̃X�e�[�W�J��
        if (this.gameObject.name == "NextStageButton")
        {
            // �܂��V�[�����擾
            NowScene = SceneManager.GetActiveScene();

            if(NowScene.name == "Stage1")
                SceneManager.LoadScene("Stage2");
            if(NowScene.name == "Stage2")
                SceneManager.LoadScene("Stage3");
            if(NowScene.name == "Stage3")
                SceneManager.LoadScene("Stage4");
            if(NowScene.name == "Stage4")
                SceneManager.LoadScene("Stage5");
        }

        // �Q�[���I��
        if(this.gameObject.name == "EndGameButton")
        {
            StartCoroutine("FadeOut");

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
            #endif
        }

        // �Q�[���ĊJ
        if(this.gameObject.name == "PlayGameButton")
        {
            GameObject MenuUI = this.transform.parent.gameObject;
            gameManager.backInGame();
            MenuUI.SetActive(false);
            Time.timeScale = 1;
        }

        // �X�e�[�W���g���C
        if(this.gameObject.name == "RetryButton")
        {
            NowScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(NowScene.name);
        }

        // �^�C�g����ʑJ��
        if (this.gameObject.name == "TitleMenuButton")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameTitle");
        }

        // �|�[�Y��ʂő�����@�{�^���������ꂽ�瑀��������m�F�ł���
        if(this.gameObject.name == "OperationMethodButton")
        {
            // ������@��ʂ�\������
            OperationMethodObj.SetActive(true);
        }
    }

    //�t�F�[�h�A�E�g(�������Ó])
    protected IEnumerator FadeOut()
    {
        Time.timeScale = 1;
        FadeObj.SetActive(true);
        // Image�R���|�[�l���g���擾
        Image Fade = FadeObj.GetComponent<Image>();
        // �t�F�[�h���̐F��ݒ�
        Fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f));
        // �E�F�C�g���Ԃ��Z�o
        float WaitTime = FadeTime / LoopCount;
        // �F�̊Ԋu���Z�o
        float AlphaInterval = 255.0f / LoopCount;

        // �F�����X�ɕς��郋�[�v
        for (float Alpha = 0.0f; Alpha <= 255.0f; Alpha += AlphaInterval)
        {
            // �҂�����
            yield return new WaitForSeconds(WaitTime);

            // Alpha�l(�����x)���������グ�Ă���
            Color newColor = Fade.color;
            newColor.a = Alpha / 255.0f;
            Fade.color = newColor;
        }

    }
}