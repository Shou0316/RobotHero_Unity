using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�G�ɓ��������������ɓ��������������X�N���v�g
public class ShotDes : MonoBehaviour
{
    float DesCount;
    [SerializeField] Transform ShotObj;

    private void OnCollisionEnter(Collision collision)
    {
        //���f�����폜���܂�
        Destroy(this.gameObject);
    }

    private void Update()
    {
        //�G�t�F�N�g�폜����
        DesCount += Time.deltaTime;
        if(DesCount >= 3.0f)
        {
            DesCount = 0.0f;
            Destroy(this.gameObject);
        }
    }
}
