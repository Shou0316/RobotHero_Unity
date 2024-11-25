using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̗͂�0�ɂȂ����Ƃ��A�m�����[�V�������Đ����ēG����������
/// </summary>
public class DestroyEnemy : EnemyBase
{

    private EnemyMove eMove;
    private Animator eAnim;

    //�ŏ��Ɏ��s
    private void Start()
    {
        eMove = this.GetComponent<EnemyMove>();
        eAnim = this.GetComponent<Animator>();
    }

    private void Update()
    {

        if (NowEnemyHP == 0)    //�c��̗͂�0�ɂȂ����Ƃ�
        {
            //���g�����{�X�ł���Ȃ�
            if (this.gameObject.name == "MiniBoss")
            {
                //�N���A�ɕK�v�Ȍ���\������
            }

            //�X���C���Ȃ��
            if (this.gameObject.name == "Slime")
            {
                //���S�A�j���[�V�������Đ�����
                eAnim.SetTrigger("DieTrigger");
                this.gameObject.tag = "Untagged";
                eMove.enabled = false;
                //���̌�A��莞�Ԃ��o�߂�����I�u�W�F�N�g������
                Destroy(this.transform.gameObject, SlimeDespawnTime);
                return;
                
            }
            //�I�u�W�F�N�g������
            Destroy(this.gameObject);
        }
    }
}

