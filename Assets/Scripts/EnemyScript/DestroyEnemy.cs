using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 体力が0になったとき、瀕死モーションを再生して敵を消す処理
/// </summary>
public class DestroyEnemy : EnemyBase
{

    private EnemyMove eMove;
    private Animator eAnim;

    //最初に実行
    private void Start()
    {
        eMove = this.GetComponent<EnemyMove>();
        eAnim = this.GetComponent<Animator>();
    }

    private void Update()
    {

        if (NowEnemyHP == 0)    //残り体力が0になったとき
        {
            //自身が中ボスであるなら
            if (this.gameObject.name == "MiniBoss")
            {
                //クリアに必要な鍵を表示する
            }

            //スライムならば
            if (this.gameObject.name == "Slime")
            {
                //死亡アニメーションを再生する
                eAnim.SetTrigger("DieTrigger");
                this.gameObject.tag = "Untagged";
                eMove.enabled = false;
                //その後、一定時間が経過したらオブジェクトを消す
                Destroy(this.transform.gameObject, SlimeDespawnTime);
                return;
                
            }
            //オブジェクトを消す
            Destroy(this.gameObject);
        }
    }
}

