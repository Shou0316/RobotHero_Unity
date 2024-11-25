using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鍵がフィールドに出現するか判定するスクリプト
public class SetKeyCheck : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Key;

    //スタート処理
    private void Start()
    {
        Key.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //フィールド内の敵を倒したら
        if(Enemy == null || Enemy.activeSelf == false)
        {
            Key.SetActive(true);
            this.GetComponent<SetKeyCheck>().enabled = false;
        }
    }
}
