using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���炩�̃{�^�����N���b�N���ꂽ��A�O�̉�ʂɖ߂�܂�
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
