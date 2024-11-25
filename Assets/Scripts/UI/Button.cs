using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ボタンが押されたときにボタンをへこませて,ボタンごとに処理を行います
public class Button : MonoBehaviour
{
    public bool ButtonFlag = false;
    Vector3 FieldPos;
    //変化するゲームオブジェクト(あらかじめ指定)
    [SerializeField] private GameObject Bridge;
    [SerializeField] private GameObject OpenField;
    [SerializeField] private GameObject[] ButtonEnemy;

    private string NowScene;    //現在のシーン名を取得する変数

    //ボタンSE
    [SerializeField] private SoundManager sound;

    // Start is called before the first frame update
    private void Start()
    {
        NowScene = SceneManager.GetActiveScene().name;
        ButtonFlag = false;
        if(NowScene == "Stage4")
        {
            for (int i = 0; i < ButtonEnemy.Length; i++)
            {
                ButtonEnemy[i].SetActive(false);
            }

            Bridge.SetActive(false);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーがボタンに触れた時
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            //ボタンが凹んで押された状態にします
            this.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            //プレイヤーが触れるか、弾が触れたらボタンが押されます
            if (ButtonFlag == false && collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
            {
                ButtonFlag = true;
                sound.PlaySE("Button");
            }

            if (this.gameObject.name == "Button2")
            {
                OpenField.SetActive(true);
                for (int i = 0; i < ButtonEnemy.Length; i++)
                {
                    ButtonEnemy[i].SetActive(true);
                }
            }

            //ボタンが押されたら橋を開通させます
            if (this.gameObject.name == "BridgeButton1")
            {
                Bridge.SetActive(true);
            }

            //ステージ3でボタンが押されたら、敵や鍵が出現します
            if (this.gameObject.name == "EnemyButton")
            {
                Bridge.SetActive(true);
                ButtonEnemy[0].SetActive(true);
                ButtonEnemy[1].SetActive(true);
                ButtonEnemy[2].SetActive(true);
                
            }
        }
    }
}
