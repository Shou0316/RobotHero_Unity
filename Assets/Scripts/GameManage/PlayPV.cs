using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayPV : MonoBehaviour
{
    //シーン中の経過時間
    float SceneTime = 0.0f;
    //PVシーン切り替えタイム
    float TransitionTime = 15.0f;
    // フェード用のUIパネル（Image）
    public Image FadePanel;         
    // フェードアウトの完了にかかる時間
    public float FadeDuration = 0.5f;

    private void Start()
    {
        FadePanel.color = new Color(FadePanel.color.r,FadePanel.color.g,FadePanel.color.b, 1.0f);
        StartCoroutine(FadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        SceneTime += Time.deltaTime;

        //何も操作がなく、遷移タイムに達したとき
        if(SceneTime > TransitionTime)
        {
            //フェードアウトしたのち、PVを再生する
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    //フェードアウト
    public IEnumerator FadeOutAndLoadScene()
    {
        FadePanel.enabled = true;                 // パネルを有効化
        float ElapsedTime = 0.0f;                 // 経過時間を初期化
        Color StartColor = FadePanel.color;       // フェードパネルの開始色を取得
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
    }

    //フェードイン
    public IEnumerator FadeIn()
    {
        FadePanel.enabled = true;                 // パネルを有効化
        float ElapsedTime = 0.0f;                 // 経過時間を初期化
        Color StartColor = FadePanel.color;       // フェードパネルの開始色を取得
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
}
