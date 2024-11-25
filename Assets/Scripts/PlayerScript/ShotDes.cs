using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敵に当たったか何かに当たったら消えるスクリプト
public class ShotDes : MonoBehaviour
{
    float DesCount;
    [SerializeField] Transform ShotObj;

    private void OnCollisionEnter(Collision collision)
    {
        //モデルを削除します
        Destroy(this.gameObject);
    }

    private void Update()
    {
        //エフェクト削除処理
        DesCount += Time.deltaTime;
        if(DesCount >= 3.0f)
        {
            DesCount = 0.0f;
            Destroy(this.gameObject);
        }
    }
}
