using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//�{�^���������ꂽ�Ƃ��Ƀ{�^�����ւ��܂���,�{�^�����Ƃɏ������s���܂�
public class Button : MonoBehaviour
{
    public bool ButtonFlag = false;
    Vector3 FieldPos;
    //�ω�����Q�[���I�u�W�F�N�g(���炩���ߎw��)
    [SerializeField] private GameObject Bridge;
    [SerializeField] private GameObject OpenField;
    [SerializeField] private GameObject[] ButtonEnemy;

    private string NowScene;    //���݂̃V�[�������擾����ϐ�

    //�{�^��SE
    [SerializeField] private SoundManager sound;

    // Start is called before the first frame update
    private void Start()
    {
        NowScene = SceneManager.GetActiveScene().name;
        ButtonFlag = false;
        if(NowScene == "Stage4")
        {
            for (int i = 0; i < ButtonEnemy.Length; i++)
            {
                ButtonEnemy[i].SetActive(false);
            }

            Bridge.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[���{�^���ɐG�ꂽ��
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            //�{�^��������ŉ����ꂽ��Ԃɂ��܂�
            this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            //�v���C���[���G��邩�A�e���G�ꂽ��{�^����������܂�
            if (ButtonFlag == false && collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
            {
                ButtonFlag = true;
                sound.PlaySE("Button");
            }

            if (this.gameObject.name == "Button2")
            {
                OpenField.SetActive(true);
                for (int i = 0; i < ButtonEnemy.Length; i++)
                {
                    ButtonEnemy[i].SetActive(true);
                }
            }

            //�{�^���������ꂽ�狴���J�ʂ����܂�
            if (this.gameObject.name == "BridgeButton1")
            {
                Bridge.SetActive(true);
            }

            //�X�e�[�W3�Ń{�^���������ꂽ��A�G�⌮���o�����܂�
            if (this.gameObject.name == "EnemyButton")
            {
                Bridge.SetActive(true);
                ButtonEnemy[0].SetActive(true);
                ButtonEnemy[1].SetActive(true);
                ButtonEnemy[2].SetActive(true);
                
            }
        }
    }
}
