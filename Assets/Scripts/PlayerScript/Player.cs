using Effekseer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

//プレイヤーの動きや一部の情報を管理するスクリプト
public class Player : MonoBehaviour
{
    // 定数定義
    private const float WATER_SPEED = 2.0f;
    private const float NORMAL_SPEED = 5.0f;
    private const float LANDING_VALUE = 4.0f;
    private const int RECOBERY_HP = 2;
    private const float ROTA_SPEED = 0.25f;
    private const int SET_HP = 20;
    private const int BONUS_HP = 5;
    private const int SET_DAMAGE_FRAME = 250;
    // 移動速度を設定する
    private float MoveSpeed = NORMAL_SPEED;   //キャラクターの動く速度
    private float JumpPower = 7.0f;
    // フラグ関連
    private bool DiedFlag = false;               //瀕死状態か調べるフラグ
    private bool HitDamageFlag = true;          //攻撃が効く
    // 無敵フレーム
    private int NoDamageFrame = 0;
    private int jumpCount = 0;

    public int HP { get; private set; }
    public bool keyFlag { get; private set; }

    [SerializeField]
    Rigidbody rbody = null;
    [SerializeField]
    private SoundManager sound = null;
    [SerializeField]
    private EffectManager effect = null;
    [SerializeField]
    public Animator MyAnime = null;
    [SerializeField]
    GunsShot gunShot = null;


    // プレイヤーのポジション
    Vector3 SetPlayerPos;   //初期位置を設定


    // プレイヤー状態
    private enum State
    {
        Normal,
        ItemComplete,
        Died,
    }

    static private Action<Player> _gameClear = null;
    static public void GameClearCallBack(Action<Player> clearCheck)
    {
        _gameClear = clearCheck;
    }

    static private Action<Player> _gameOver = null;
    static public void GameOverCallBack(Action<Player> overCheck)
    {
        _gameOver = overCheck;
    }

    /// <summary>
    /// ゲームマネージャー呼び出しの初期化
    /// </summary>
    public void Initialized()
    {
        SetPlayerPos = this.transform.position;
        HP = SET_HP;
    }

    /// <summary>
    /// プレイヤー操作(位置、角度)
    /// </summary>
    public void PlayerControll()
    {
        // 最初に落下判定をチェック
        CheckPlayerLanding();

        // 次に敵の攻撃がヒットするか
        if (!HitDamageFlag)
        {
            NoDamageFrame++;
            CheckHitEnemyDamageTime();
        }

        //プレイヤーの移動処理(キーボード)
        if (Input.GetKey(KeyCode.W))
        {
            MyAnime.SetBool("RunFlag", Input.GetKey(KeyCode.W));
            this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MyAnime.SetBool("RunFlag", Input.GetKey(KeyCode.S));
            this.transform.position -= this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        else MyAnime.SetBool("RunFlag", false);

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))  Jump();

        //視点・プレイヤー角度変更
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            this.transform.Rotate(0, -ROTA_SPEED, 0);
        

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
           this.transform.Rotate(0, ROTA_SPEED, 0);

        // プレイヤーの攻撃
        if (Input.GetKey(KeyCode.P) || Input.GetMouseButton(0))
        {
            //_onPushBullet(transform.position);
            MyAnime.SetTrigger("GunTrigger");
            gunShot.PlayerGunShot();
        }
    }

    /// <summary>
    /// 回復アイテムが触れたとき対応
    /// </summary>
    private void hpCharge()
    {
        //HP回復処理
        HP += RECOBERY_HP;
        //HPが上限を超えているときは
        if (HP >= SET_HP)
            HP = SET_HP;
        //回復音再生
        sound.PlaySE("Recovery");
        effect.PlayEffect("Recovery",transform.position);
    }

    /// <summary>
    /// 無敵時間を制御
    /// </summary>
    private void CheckHitEnemyDamageTime()
    {
        if (NoDamageFrame > SET_DAMAGE_FRAME)
        {
            NoDamageFrame = 0;
            HitDamageFlag = true;
        }
    }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {
        // カウンターが0のとき
        if (jumpCount == 0)
        {
            jumpCount++;
            //ジャンプする
            MyAnime.SetTrigger("JumpTrigger");
            //ジャンプ音を再生する
            sound.PlaySE("Jump");
            //AddForceを使ったジャンプ
            rbody.AddForce(new Vector3(0.0f, JumpPower, 0.0f), ForceMode.Impulse);
        }
    }

    /// <summary>
    /// フィールドからの落下処理
    /// </summary>
    private void CheckPlayerLanding()
    {
        //プレイヤーの落下判定
        if (this.transform.position.y <= -LANDING_VALUE)
        {
            if (DiedFlag == false)
            {
                //ダメージヒット
                HitDamage();
                this.transform.position = SetPlayerPos;
            }
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    private void HitDamage()
    {
        HP--;

        if (HP <= 0 && DiedFlag == false) Died();
    }

    /// <summary>
    /// プレイヤーが死んだときに実行する処理
    /// </summary>
    private void Died()
    {
        DiedFlag = true;
        sound.PlaySE("GameOver");
        MyAnime.SetTrigger("DiedTrigger");
        _gameOver(this);
        this.gameObject.tag = "Untagged";
    }

    /// <summary>
    /// 何らかのオブジェクトと接触したとき
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 0;
        //大砲または敵との接触フラグ
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("CannonBall"))
        {
            // Diedフラグが偽で攻撃が効くタイミングであるなら
            if (DiedFlag == false && HitDamageFlag)
            {
                //HPを減らす
                HitDamage();
                HitDamageFlag = false;
            }

            //HPが0になったら
            if (HP == 0 && DiedFlag == false)
            {
                //Died関数を呼び出し
                Died();
            }
        }

        //動くフィールドのフラグ
        if (collision.gameObject.CompareTag("MoveField"))    
            this.transform.SetParent(collision.transform);
        else transform.SetParent(null);
    }

    /// <summary>
    /// プレイヤーがトリガーに触れた時
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Recovery"))
        {
            //現在の体力値が設定値と同じかそれ以上なら抜ける
            if (HP >= SET_HP)  return;
            
            Destroy(other.gameObject);
            // リカバリー関数呼び出し
            hpCharge();
        }

        if (other.gameObject.CompareTag("Key"))
        {
            sound.PlaySE("GetKey");
            keyFlag = true;
        }

        if (other.gameObject.CompareTag("Castle"))
        {
            _gameClear(this);
        }
    }

    /// <summary>
    /// トリガー内に入っているとき
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //水に入水フラグ(入っているなら、速度を落とす)
        if (other.gameObject.CompareTag("Water"))
            MoveSpeed = WATER_SPEED;


        //階段判定
        if (other.gameObject.CompareTag("Stairs"))    //プレイヤーが階段に立っていて
        {
            float StairsHeight = other.gameObject.transform.position.y - this.gameObject.transform.position.y;
            //かつ、登れる高さにあるなら
            if (StairsHeight <= 0.7f)
            {
                //プレイヤーに上方向の力を加えて、階段を上る
                rbody.velocity = transform.up;
            }
        }
    }

    /// <summary>
    /// トリガーから抜けた時
    /// </summary>
    /// <param name="other">コライダー</param>
    private void OnTriggerExit(Collider other)
    {
        //水から上がったとき、速度をもとに戻します
        if (other.gameObject.CompareTag("Water"))
            MoveSpeed = NORMAL_SPEED;
    }

    /// <summary>
    /// HPの最大値ゲッター
    /// </summary>
    /// <returns></returns>
    public int getMaxHP()
    {
        return SET_HP;
    }
}
