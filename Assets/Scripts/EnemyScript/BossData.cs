using UnityEngine;

public class BossData : MonoBehaviour
{
    const float DieTime = 3.0f;
    public int SetBossHP = 80;                  //ボスの体力値
    public int SetMiniBossHP = 50;              //中ボスの体力値
    [SerializeField] private GameObject KeyObj;         //鍵オブジェクト(ステージに設置)
    [SerializeField] private SoundManager sound;       //サウンドマネージャーオブジェクト
    [SerializeField] private mainGameUI UIDisPlay;      //UIdisPlayクラス取得


    BossMove bMove;                             //BossMoveクラス取得
    Animator BossAnim;                          //自身のアニメーター取得
    public int BossHP;                          //自身の現在の体力
    private bool BossAttackFlag;

    private void Start()
    {
        //自身から各コンポーネント取得
        bMove = this.GetComponent<BossMove>();
        BossAnim = this.GetComponent<Animator>();
        
        //予め鍵オブジェクトを非アクティブ化しておく
        KeyObj.SetActive(false);

     //中ボス・ボスの体力設定
        if (this.gameObject.name == "BossCharactor")    //自身がボスの場合
        {
            BossHP = SetBossHP; //ボス用の体力を入れる
        }

        if (this.name == "MiniBoss")    //自身が中ボスのとき
        {
            BossHP = SetMiniBossHP;     //中ボス用の体力を入れる
        }

        sound.PlaySE("Button");
    }

    //常に処理
    private void Update()
    {
        //プレイヤーの体力が0のとき
        if (BossHP == 0)
        {
            //BossMoveを非アクティブ化
            bMove.enabled = false;
            //クリア用の鍵を表示
            KeyObj.SetActive(true);
            //倒れるアニメーションを再生
            BossAnim.SetTrigger("DieTrigger");
            //倒れるSE再生
            sound.PlaySE("EnemyDied");
            //一定時間経過後、ボスを消す。
            Destroy(this.gameObject, DieTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            BossHP--;
            UIDisPlay.BossGaugeClass();

        }

        //プレイヤーと接触したとき
        if(collision.gameObject.tag == "Player")
        {
            BossAttackFlag = true;
        }
    }

}
