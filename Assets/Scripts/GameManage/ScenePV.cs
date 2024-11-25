using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

/// <summary>
/// PV再生中のシーン中の処理
/// </summary>
public class ScenePV : MonoBehaviour
{
    [SerializeField]Image FadePanel;
    [SerializeField] VideoPlayer GamePV;
    float FadeDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        GamePV.loopPointReached += LoopPointReached;
        GamePV.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //何らかの操作があるか、再生が終了したときフェードアウトしてタイトル画面に遷移
        if(Input.anyKey)
        {
            StartCoroutine("FadeOutAndLoadScene");
        }
    }

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
        SceneManager.LoadScene("TitleMenu"); // シーンをロードしてメニューシーンに遷移
    }

    public void LoopPointReached(VideoPlayer pv)
    {
        SceneManager.LoadScene("MainMenu"); // シーンをロードしてメニューシーンに遷移
    }
}
