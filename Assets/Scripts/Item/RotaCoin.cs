using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����R�C���ȂǁA�t�B�[���h���ɗ����Ă���A�C�e������]����X�N���v�g
public class RotaCoin : MonoBehaviour
{
    const float RotaX = 0.0f;
    const float RotaY = 4.0f;
    const float RotaZ = 0.0f;

    //�����Ɖ�]�������܂�
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
