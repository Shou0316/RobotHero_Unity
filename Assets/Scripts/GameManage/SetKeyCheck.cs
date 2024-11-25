using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����t�B�[���h�ɏo�����邩���肷��X�N���v�g
public class SetKeyCheck : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Key;

    //�X�^�[�g����
    private void Start()
    {
        Key.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //�t�B�[���h���̓G��|������
        if(Enemy == null || Enemy.activeSelf == false)
        {
            Key.SetActive(true);
            this.GetComponent<SetKeyCheck>().enabled = false;
        }
    }
}
