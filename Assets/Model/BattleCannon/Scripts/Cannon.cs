using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using UnityEngine;

//一定時間ごとに弾を発射し続けるスクリプト
public class Cannon : MonoBehaviour
{
    [SerializeField]
    private GameObject CannonBullet;
    private Transform EnemyShotTrans;
    private GunsShot cShot;    //発射スクリプトを保持する変数
    [SerializeField]
    private SoundManager sound;

    private void Start()
    {
        cShot = this.GetComponent<GunsShot>();
        //開始時弾が発射しない状態にする
        cShot.enabled = false;
    }

    //トリガー内処理
    private void OnTriggerStay(Collider other)
    {
        //プレイヤーが大砲作動圏内に入っていれば
        if(other.gameObject.name == "Player")
        {
            //大砲を動作する
            cShot.enabled = true;
        }
    }

    //何かがトリガーから離れた時
    private void OnTriggerExit(Collider other)
    {
        //プレイヤーが作動圏内から離れた時
        if (other.gameObject.name == "Player")
        {
            //大砲を停止する
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

