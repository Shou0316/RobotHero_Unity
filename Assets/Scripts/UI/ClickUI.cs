using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//何らかのボタンがクリックされたら、前の画面に戻ります
public class ClickUI : MonoBehaviour
{

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(this.gameObject == true && Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
    }
}
