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

//ベースをもとにした敵キャラクターの行動
public class EnemyMove : EnemyBase
{
    private void Update()
    {
        //敵の状態分岐
        switch (e_State)
        {
            //パトロール状態なら
            case EnemyState.Patrol:
                RunFlag = false;
                if (Agent.remainingDistance < 0.5f)
                {
                    NextPatrolPoint();
                }
                EnemyAnim.SetBool("RunFlag", RunFlag);
                break;

            //プレイヤー追跡状態なら
            case EnemyState.Chase:
                Chase();
                EnemyAnim.SetBool("RunFlag", RunFlag);
                break;
        }
    }
}

