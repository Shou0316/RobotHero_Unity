using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//画面の暗転明転処理
public class FadeInOut : MonoBehaviour
{
    // フェード用のUIパネル（Image）
    [SerializeField]
    private Image FadePanel;
    [SerializeField]
    private Image GameRule;
    [SerializeField]
    private Image OperationDescription;

    // フェードイン・アウトの完了にかかる時間
    public float FadeDuration = 1.0f;

    private bool FadeFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        FadePanel = GameObject.FindWithTag("FadeUI").GetComponent<Image>();
        StartCoroutine("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "OperationDescription")
        {
            GameStart();
        }
    }

    void GameStart()
    {
        if (Input.anyKeyDown)
        {
            if (OperationDescription.gameObject.activeSelf)
                StartCoroutine("FadeOut");

            GameRule.gameObject.SetActive(false);
            OperationDescription.gameObject.SetActive(true);
        }
    }

    //フェードイン(ゆっくり明転)
    public IEnumerator FadeIn()
    {
        FadePanel.enabled = true;                 // パネルを有効化
        float ElapsedTime = 0.0f;                 // 経過時間を初期化
        Color StartColor = new Color(FadePanel.color.r,FadePanel.color.g,FadePanel.color.b,1.0f);       // フェードパネルの開始色を取得
        Color EndColor = new Color(StartColor.r, StartColor.g, StartColor.b, 0.0f); // フェードパネルの最終色を設定

        while (ElapsedTime < FadeDuration)
        {
            ElapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(ElapsedTime / FadeDuration);  // フェードの進行度を計算
            FadePanel.color = Color.Lerp(StartColor, EndColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        FadePanel.color = EndColor;  // フェードが完了したら最終色に設定
        FadePanel.enabled = false;                 // パネルを有効化

    }

    //フェードアウト(ゆっくり暗転)
    public IEnumerator FadeOut()
    {
        FadePanel.enabled = true;                 // パネルを有効化
        FadeFlag = false;
        float ElapsedTime = 0.0f;                 // 経過時間を初期化
        Color StartColor = new Color(FadePanel.color.r, FadePanel.color.g, FadePanel.color.b, 0.0f);       // フェードパネルの開始色を取得
        Color EndColor = new Color(StartColor.r, StartColor.g, StartColor.b, 1.0f); // フェードパネルの最終色を設定

        while (ElapsedTime < FadeDuration)
        {
            ElapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(ElapsedTime / FadeDuration);  // フェードの進行度を計算
            FadePanel.color = Color.Lerp(StartColor, EndColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        FadePanel.color = EndColor;  // フェードが完了したら最終色に設定
        SceneManager.LoadScene("PlayPV"); // シーンをロードしてメニューシーンに遷移

        //暗転後、ステージ1に移動する
        SceneManager.LoadScene("Stage1");
    }
}
