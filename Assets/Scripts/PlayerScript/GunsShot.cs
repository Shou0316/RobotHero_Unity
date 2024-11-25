using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;

public class GunsShot : MonoBehaviour
{
    [SerializeField] private GameObject BulletModel = null;      //ショットモデル
    [SerializeField] private SoundManager sound = null;
    [SerializeField] private Player _player = null;
    //爆発エフェクト
    [SerializeField]GameObject ShotPoint;       //発射点(プレイヤーと大砲)
    bool CannonFlag;
    private float ShotSpeed = 20.0f;    //発射速度
    private float Rate = 0.5f;          //
    private float FrameCount = 0;       //フレームカウント
    private int ShotCount = 0;          //ショットカウント
    Animator MyAnim;
   
    const float SetCannonTime = 1.5f;

    float ShotTimer;
    const float SetShotTimer = 0.1f;

    float CannonTime = 0.0f;
 
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.tag == "Player")
        {
            _player = this.gameObject.GetComponent<Player>();
            MyAnim = this.gameObject.GetComponent<Animator>();
        }
    }

    //常に処理
    void Update()
    {
        FrameCount++;

        if (this.gameObject.tag == "Cannon")
        {
            CannonGunShot();
        }
    }

    //プレイヤーショット
    public void PlayerGunShot()
    {
        if (ShotTimer <= 0.0f)
        {
            sound.PlaySE("GunShot");

            //弾を発射する方向を取得します
            Vector3 BulletPosition = ShotPoint.transform.position;

            //プレイヤーの角度に沿って弾を発射します
            GameObject Shot = Instantiate(BulletModel, BulletPosition, transform.localRotation);
            Vector3 Direction = Shot.transform.forward;
            //AddForceを用いた発射
            Shot.GetComponent<Rigidbody>().AddForce(Direction * ShotSpeed, ForceMode.Impulse);
            ShotTimer = SetShotTimer;
        }
        else if(ShotTimer > 0.0f)
        {
            ShotTimer -= Time.deltaTime;
        }
    }

    //大砲ショット（敵）
    void CannonGunShot()
    {
        //一定時間ごとに処理実行します
        Timer CannonTimer = new Timer(500);
        CannonTime += Time.deltaTime;
            
        Vector3 CannonBulletPosition = ShotPoint.transform.position;

        if(CannonTime >= SetCannonTime)
        {
            //CannonTimerの間隔で弾を発射します
            GameObject CannonShot = Instantiate(BulletModel, CannonBulletPosition, transform.localRotation);
            Vector3 Direction = CannonShot.transform.forward;
            //AddForceを用いた発射
            CannonShot.GetComponent<Rigidbody>().AddForce(Direction * ShotSpeed, ForceMode.Impulse);
            CannonTime = 0.0f;
        }
    }

    private IEnumerator RapidFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(Rate);
        }
    }
}
