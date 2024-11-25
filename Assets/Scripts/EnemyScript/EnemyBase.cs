using Effekseer;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵キャラクターの基底クラス(ベース)
/// </summary>
public class EnemyBase: MonoBehaviour
{
    protected enum EnemyState
    {
        Patrol,
        Chase,
    }

    [SerializeField]
    protected Transform Player;

    //アニメーション、クォータニオン
    protected Animator EnemyAnim;
    protected Quaternion ThisRota;

    //値を変更しないもの(定数)
    protected const float SlimeDespawnTime = 2.0f;      //スライムデスポーンタイム
    protected const float BruteDespawnTime = 0.8f;      //Bruteデスポーンタイム 
    protected const int IntReset = 0;                   //int型の値リセット
    protected const float FloatReset = 0.0f;            //float型の値リセット
    protected const float SetDistance = 1.0f;           //セット距離
    private const float SetEffectPosY = 0.8f;           //エフェクト位置セット
    private float SearchAngle = 110.0f;                 //サーチ角度

    //自身のステート
    protected EnemyState e_State;

    //フラグ関係
    protected bool RunFlag;                   //走るフラグ
    protected bool DiedFlag = false;
    
    //自身の情報
    [SerializeField]
    protected int SetEnemyHP;
    [SerializeField]protected int NowEnemyHP;

    //NavMesh
    protected NavMeshAgent Agent;

    //サウンド関連
    [SerializeField]
    private SoundManager sound;

    //エフェクト関連
    [SerializeField]
    private EffectManager effect = null;

    //ランダム移動に使用
    public Transform[] PatrolPoint;         //パトロール時の目標地点
    protected int DestNum = 0;              //地点の番号
    private int DestNumMin = 0;             //地点番号決定時に使用(最小値)
    private int DestNumMax = 4;             //地点番号決定時に使用(最大値)

    // Start is called before the first frame update
    void Start()
    {
        //初期設定
        Player = GameObject.FindWithTag("Player").transform;        //プレイヤーオブジェクト取得
        EnemyAnim = this.GetComponent<Animator>();                  //敵アニメーション取得 
        Agent = this.GetComponent<NavMeshAgent>();                  //NavMeshのAgent取得
        NowEnemyHP = SetEnemyHP;                                    //体力セット(敵ごとに設定)

        //オブジェクト名変更(クローン出現)
        if (this.name == "Slime(Clone)")
        {
            this.name = "Slime";
        }

        if (this.name == "Brute(Clone)")
        {
            this.name = "Brute";
        }
    }

    protected void NextPatrolPoint()
    {
        //ランダムで値を決定
        DestNum = Random.Range(DestNumMin, DestNumMax);

        //決まった値にポイントが入っていれば
        if (PatrolPoint[DestNum] != null)
        {
            //そのポイントに向けて進む
            Agent.destination = PatrolPoint[DestNum].position;
        }
    }

    //行動パターン③ランダムな方向に攻撃する
    protected void EnemyAttack()
    {
        //プレイヤーとの距離を調べる
        float PlayerDistance = Vector3.Distance(Player.transform.position, this.transform.position);
        //プレイヤーとの距離がセットした距離分近づいたとき
        if (PlayerDistance <= SetDistance)
        {
            //攻撃アニメーションを再生
            EnemyAnim.SetTrigger("AttackTrigger");
        }
    }

    //ターゲット追跡
    protected void Chase()
    {
        //ランフラグを真とし
        RunFlag = true;
        //プレイヤーのいる方向に向きを合わせて
        transform.LookAt(Player.transform.position);
        //進む目標をプレイヤーに切り替えて進む
        Agent.destination = Player.transform.position;

    }

    private void hitDamage()
    {
        //攻撃を食らった音とエフェクトを再生する　※キャラ別に攻撃音が変化
        if (this.gameObject.name == "Brute")
        {
            RunFlag = true;
            sound.PlaySE("BruteHitAttack");
            effect.PlayEffect("EnemyHit",transform.position);
        }

        if (this.gameObject.name == "Slime")
        {
            sound.PlaySE("SlimeHitAttack");
            effect.PlayEffect("EnemyHit", transform.position);
        }

        //敵の状態を追跡状態にする
        e_State = EnemyState.Chase;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        //弾と接触したとき
        if (collision.gameObject.tag == "Bullet")
        {
            //自身の体力を1減らす
            NowEnemyHP--;

            //体力が1以上ならば
            if (!DiedFlag) hitDamage();
            
            if (NowEnemyHP <= IntReset)    //残り体力が0になったとき
            {
                DiedFlag = true;
                this.gameObject.tag = "Untagged";

                if (this.gameObject.name == "Slime")
                {
                    EnemyAnim.SetTrigger("DieTrigger");
                    effect.PlayEffect("SlimeDied", transform.position);
                    enabled = false;
                    // 倒れる音を再生
                    sound.PlaySE("EnemyDied");
                    // その後、一定時間が経過したらオブジェクトを消す
                    Destroy(this.transform.gameObject, SlimeDespawnTime);
                    return;
                }

                if (this.gameObject.name == "Brute")
                {
                    effect.PlayEffect("BruteDied", transform.position);
                    enabled = false;
                    Destroy(this.transform.gameObject,BruteDespawnTime);
                    return;
                }
            }

        }

        //当たった物がプレイヤーでかつ体力が1以上ならば
        if (collision.gameObject.tag == "Player" && !DiedFlag)
        {
            EnemyAnim.SetTrigger("AttackTrigger");
            //攻撃音を再生
            if (this.gameObject.name == "Brute")
            {
                sound.PlaySE("BruteAttack");
            }

            if (this.gameObject.name == "Slime")
            {
                sound.PlaySE("SlimeAttack");
            }

        }

    }

    //捜索エリア判定
    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !DiedFlag)
        {
            //プレイヤーの方向
            var PlayerDirection = other.transform.position - transform.position;
            //敵の前方からの主人公の方向
            var Angle = Vector3.Angle(PlayerDirection, transform.forward);
            //サーチ角度内にいれば、追跡へ
            if(Angle < SearchAngle)
            {
                e_State = EnemyState.Chase;
            }
        }
    }

    //サーチ範囲から離れたとき
    protected void OnTriggerExit(Collider other)
    {
        //パトロールステートに切り替え
        e_State = EnemyState.Patrol;
    }

}
