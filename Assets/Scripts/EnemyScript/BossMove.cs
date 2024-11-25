using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// ボスキャラクター(中ボス、メインボス)の行動パターン
/// </summary>
public class BossMove : MonoBehaviour
{
    enum State
    {
        Chase,
        Attack,
        Died
    }

    //オブジェクト、トランスフォーム宣言
    [SerializeField]
    private GameObject setEnemy;          //敵の配列
    [SerializeField]
    private Transform Central;           //敵の中心
    private Transform Player;            //プレイヤー

    [SerializeField]
    private SoundManager sound;
    private Animator BossAnim;
    //定数
    const float TargetAngle = 30.0F;        //ターゲット角度
    const int HalfHP = 2;                   //ボスの体力と割り算用
    const int MaxEnemyCount = 8;            //フィールド内に出現する敵の上限

    //ランダム移動関係
    private float TimeCount;
    private float CountTime = 0.0f;         //時間計測
    private float WaitTime = 3.0f;          //待機時間設定

    //フラグ関係
    private bool WalkFlag;                  //歩行フラグ
    private bool RunFlag;                   //走るフラグ
    private bool TrackPlayerFlag = false;   //プレイヤー追跡フラグ

    private int EnemyNum = 5;
    private int BossActJuge;    //ボスの行動判定
    private int EnemyCount;     //敵数カウント

    //NavMesh
    NavMeshAgent Agent;

    //目標地点
    Vector3 TargetPos;

    //ボス状態取得
    BossData bossData;

    //その他値
    const float DistanceValue = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //トランスフォーム取得
        Player = GameObject.FindWithTag("Player").transform;    //プレイヤー
        //各コンポーネント取得
        BossAnim = this.GetComponent<Animator>();               //アニメーター
        Agent = this.GetComponent<NavMeshAgent>();              //NavMeshAgent
        bossData = this.GetComponent<BossData>();               //自身(ボス)のステータス
        //走るフラグ
        RunFlag = false;
        //リジッドボディー取得
        Agent.destination = Player.position;
        
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        //敵の数をカウント
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        //ボスの体力が1以上ならば
        if(bossData.BossHP >= 1)
            FindPlayer(TargetAngle);


        //移動以外の行動パターンをランダムに決定
        BossActJuge = Random.Range(0, 4);
        
        //1番ならランダムにプレイヤー攻撃
        if(BossActJuge == 0 && bossData.BossHP >= 1)
            BossAttack(); 

        // 自身が最終ボスならば敵を出現させる
        if(BossActJuge == 1 && this.gameObject.name == "BossCharactor")
        {
            //ボスの体力が半分以下でかつ、敵の出現上限に満たしていないとき
            if (bossData.BossHP <= bossData.SetBossHP / HalfHP &&  EnemyCount <= MaxEnemyCount)
                SetEnemy();
        }

        //プレイヤーを追跡していないときはランダム歩行(仮)
        if (TrackPlayerFlag == false)
            NextPoint();

        //体力が0になったら、歩行や追跡フラグを止める
        if(bossData.BossHP == 0)
        {
            WalkFlag = false;
        }
    }

    //行動パターン①プレイヤーを探す
    void FindPlayer(float Angle)
    {
        //プレイヤーのベクトルを取得
        Vector3 TargetVec = Player.position - this.transform.position;
        //ベクトル間の内積を計算
        var Dot = Vector3.Dot(transform.forward, TargetVec.normalized);
        //内積を角度計算
        var Deg = Mathf.Acos(Dot) * Mathf.Rad2Deg;

        //ボスの視野角内にプレイヤーが入っているとき
        if (Deg <= Angle)
        {
            TrackPlayerFlag = true;
            WalkFlag = false;
            Agent.destination = Player.position;
            BossAnim.SetBool("RunFlag", TrackPlayerFlag);
        }
        else //視野角内に入っていないとき
        {
            //関連フラグを偽にする
            TrackPlayerFlag = false;
            WalkFlag = true;
        }
    }

    //行動パターン②-1 次の目標地点を設定して、目標へ向かう
    void NextPoint()
    {
        TrackPlayerFlag = false;
        WalkFlag = true;

        //座標変更
        Agent.destination = TargetPos;
        CountTime += Time.deltaTime;

        //ストップさせない
        Agent.isStopped = false;
        //アニメーション再生
        BossAnim.SetBool("WalkFlag", WalkFlag);

        //ターゲット範囲内にいるとき
        if(this.transform.position.x == TargetPos.x &&
           this.transform.position.z == TargetPos.z || TimeCount >= WaitTime)
        {
            StopMove();
            ChangeDirection();
        }

    }

    void ChangeDirection()
    {
        float RandomX = Random.Range(-1f, 1f);
        float RandomZ = Random.Range(-1, 1f);
        TargetPos = new Vector3(RandomX, 0f, RandomZ);
    }

    //行動パターン②-2 目標地点について、ストップする
    void StopMove()
    {
        //ストップさせる
        Agent.isStopped = true;
        //待ち時間を計測
        CountTime += Time.deltaTime;
        RunFlag = false;

        //待ち時間を設定された数値を超えると起動
        if(CountTime > WaitTime)
        {
            NextPoint();
            CountTime = 0.0f;
            ChangeDirection();
        }
    }

    //行動パターン③ランダムな方向に攻撃する
    void BossAttack()
    {
        if(bossData.BossHP >= 1)
        {
            //プレイヤーとの距離を調べる
            float PlayerDistance = Vector3.Distance(Player.transform.position, this.gameObject.transform.position);
            //プレイヤーとの距離が一定近づいたとき
            if (PlayerDistance <= DistanceValue)
            {
                //攻撃アニメーションを再生
                BossAnim.SetTrigger("AttackTrigger");
            }
        }
    }

    //行動パターン④フィールドに敵を出現させる
    void SetEnemy()
    {
        setEnemy.SetActive(true);
    }

    //コリジョン同士何かと接触したとき
    private void OnCollisionEnter(Collision collision)
    {
        //ボスの体力が1以上で
        if(bossData.BossHP >= 1)
        {
            //弾が当たったら、プレイヤーへ攻撃する処理
            if (collision.gameObject.tag == "Bullet")
            {
                //捜索フラグを真
                TrackPlayerFlag = true;
                //プレイヤーを追跡
                Agent.destination = Player.position;
                //攻撃を受けた音を再生する
                sound.PlaySE("EnemyHitAttack");
            }

            //プレイヤーに当たったら、プレイヤーからの攻撃処理
            if (collision.gameObject.tag == "Player")
            {
                BossAnim.SetTrigger("AttackTrigger");
                sound.PlaySE("BruteAttack");
            }

        }
    }

    //捜索エリア判定
    protected void OnTriggerStay(Collider other)
    {
        //エリア内にプレイヤーがいれば捜索フラグ真
        if (other.gameObject.tag == "Player")
            TrackPlayerFlag = true;
    }

    protected void OnTriggerExit(Collider other)
    {
        //エリア内にプレイヤーがいなければ捜索フラグ偽
        if (other.gameObject.tag == "Player")
            TrackPlayerFlag = false;
    }

}
