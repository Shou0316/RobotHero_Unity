using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Unity.VisualScripting;

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyBase))]
#endif

//�x�[�X�����Ƃɂ����G�L�����N�^�[�̍s��
public class EnemyMove : EnemyBase
{
    private void Update()
    {
        //�G�̏�ԕ���
        switch (e_State)
        {
            //�p�g���[����ԂȂ�
            case EnemyState.Patrol:
                RunFlag = false;
                if (Agent.remainingDistance < 0.5f)
                {
                    NextPatrolPoint();
                }
                EnemyAnim.SetBool("RunFlag", RunFlag);
                break;

            //�v���C���[�ǐՏ�ԂȂ�
            case EnemyState.Chase:
                Chase();
                EnemyAnim.SetBool("RunFlag", RunFlag);
                break;
        }
    }
}

