using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;

//��莞�Ԃ��Ƃɒe�𔭎˂�������X�N���v�g
public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject CannonBullet;
    private Transform EnemyShotTrans;
    private GunsShot cShot;    //���˃X�N���v�g��ێ�����ϐ�
    [SerializeField]
    private SoundManager sound;

    private void Start()
    {
        cShot = this.GetComponent<GunsShot>();
        //�J�n���e�����˂��Ȃ���Ԃɂ���
        cShot.enabled = false;
    }

    //�g���K�[������
    private void OnTriggerStay(Collider other)
    {
        //�v���C���[����C�쓮�����ɓ����Ă����
        if(other.gameObject.name == "Player")
        {
            //��C�𓮍삷��
            cShot.enabled = true;
        }
    }

    //�������g���K�[���痣�ꂽ��
    private void OnTriggerExit(Collider other)
    {
        //�v���C���[���쓮�������痣�ꂽ��
        if (other.gameObject.name == "Player")
        {
            //��C���~����
            cShot.enabled = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sound.PlaySE("BruteAttack");
        }

    }

}

