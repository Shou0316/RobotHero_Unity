using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//シーン遷移などシステム関連のボタンスクリプト
public class SystemButton : MonoBehaviour
{

    //フェードアウトの時間
    const float FadeTime = 1.0f;
    //ループ回数
    const int LoopCount = 20;

    protected Scene NowScene;
    Transform MyButton;
    //ボタンサイズ
    const float NormalButtonScale = 3.2f;
    const float OnMouseButtonScale = 3.5f;
    
    //オブジェクト取得
    [SerializeField] GameObject FadeObj;                //フェードオブジェクト
    [SerializeField] GameObject OperationMethodObj;     //操作方法オブジェクト
    [SerializeField]
    private GameManager gameManager;
    [SerializeField] SoundManager sound;            //サウンドオブジェクト


    private void Start()
    {
        MyButton = this.GetComponent<Transform>();
        //FadeObj.SetActive(false);
    }

    void OnMouseOver()
    {
        //ボタンサイズ
        MyButton.localScale = new Vector3(OnMouseButtonScale, OnMouseButtonScale, OnMouseButtonScale);
    }

    void OnMouseExit()
    {
        MyButton.localScale = new Vector3(NormalButtonScale, NormalButtonScale, NormalButtonScale);
    }

    //それぞれ対応するオブジェクト(ボタン)の操作でシーン遷移
    protected void OnClick()
    {
        //SE再生
        sound.PlaySE("SystemButton");

        //ゲームスタート
        if (this.gameObject.name == "GameStartButton")
        {
            StartCoroutine("FadeOut");

            SceneManager.LoadScene("OperationDescription");
        }

        // 次のステージ遷移
        if (this.gameObject.name == "NextStageButton")
        {
            // まずシーンを取得
            NowScene = SceneManager.GetActiveScene();

            if(NowScene.name == "Stage1")
                SceneManager.LoadScene("Stage2");
            if(NowScene.name == "Stage2")
                SceneManager.LoadScene("Stage3");
            if(NowScene.name == "Stage3")
                SceneManager.LoadScene("Stage4");
            if(NowScene.name == "Stage4")
                SceneManager.LoadScene("Stage5");
        }

        // ゲーム終了
        if(this.gameObject.name == "EndGameButton")
        {
            StartCoroutine("FadeOut");

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_STANDALONE
                UnityEngine.Application.Quit();
            #endif
        }

        // ゲーム再開
        if(this.gameObject.name == "PlayGameButton")
        {
            GameObject MenuUI = this.transform.parent.gameObject;
            gameManager.backInGame();
            MenuUI.SetActive(false);
            Time.timeScale = 1;
        }

        // ステージリトライ
        if(this.gameObject.name == "RetryButton")
        {
            NowScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(NowScene.name);
        }

        // タイトル画面遷移
        if (this.gameObject.name == "TitleMenuButton")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("GameTitle");
        }

        // ポーズ画面で操作方法ボタンが押されたら操作説明を確認できる
        if(this.gameObject.name == "OperationMethodButton")
        {
            // 操作方法画面を表示する
            OperationMethodObj.SetActive(true);
        }
    }

    //フェードアウト(ゆっくり暗転)
    protected IEnumerator FadeOut()
    {
        Time.timeScale = 1;
        FadeObj.SetActive(true);
        // Imageコンポーネントを取得
        Image Fade = FadeObj.GetComponent<Image>();
        // フェード時の色を設定
        Fade.color = new Color((0.0f / 255.0f), (0.0f / 255.0f), (0.0f / 0.0f), (0.0f / 255.0f));
        // ウェイト時間を算出
        float WaitTime = FadeTime / LoopCount;
        // 色の間隔を算出
        float AlphaInterval = 255.0f / LoopCount;

        // 色を徐々に変えるループ
        for (float Alpha = 0.0f; Alpha <= 255.0f; Alpha += AlphaInterval)
        {
            // 待ち時間
            yield return new WaitForSeconds(WaitTime);

            // Alpha値(透明度)を少しずつ上げていく
            Color newColor = Fade.color;
            newColor.a = Alpha / 255.0f;
            Fade.color = newColor;
        }

    }
}