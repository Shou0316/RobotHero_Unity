using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private mainGameUI gameUI;
    [SerializeField]
    private GameObject miniBoss;

    private void playerHit()
    {
        // プレイヤーにクリアフラグを与える
        gameUI.GetItemUI(this.tag);
        // 自身のオブジェクトを非アクティブ化する
        this.gameObject.SetActive(false);
        return;
    }

    // miniBossKey取得時、ミニボス出現※ステージ3
    private void SpawnMiniBoss()
    {
        // 自身のオブジェクトを非アクティブ化する
        this.gameObject.SetActive(false);
        miniBoss.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (this.name != "miniBossKey") playerHit();
            else SpawnMiniBoss();
        }
    }
}