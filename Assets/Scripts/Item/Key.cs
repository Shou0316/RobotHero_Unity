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
        // �v���C���[�ɃN���A�t���O��^����
        gameUI.GetItemUI(this.tag);
        // ���g�̃I�u�W�F�N�g���A�N�e�B�u������
        this.gameObject.SetActive(false);
        return;
    }

    // miniBossKey�擾���A�~�j�{�X�o�����X�e�[�W3
    private void SpawnMiniBoss()
    {
        // ���g�̃I�u�W�F�N�g���A�N�e�B�u������
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