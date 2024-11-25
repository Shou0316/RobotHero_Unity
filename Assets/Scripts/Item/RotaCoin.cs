using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鍵やコインなど、フィールド内に落ちているアイテムが回転するスクリプト
public class RotaCoin : MonoBehaviour
{
    const float RotaX = 0.0f;
    const float RotaY = 4.0f;
    const float RotaZ = 0.0f;

    //ずっと回転し続けます
    void Update()
    {
        gameObject.transform.Rotate(RotaX,RotaY,RotaZ);

        if (this.gameObject.transform.rotation.y >= 180.0f || this.gameObject.transform.rotation.y <= -180.0f)
        {
            this.gameObject.GetComponent<RotaCoin>().enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Field")
        {
           
        }
    }
}
