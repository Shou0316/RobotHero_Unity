using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;
using System;

public class Coin : MonoBehaviour
{
    private string itemName;
    [SerializeField]
    private mainGameUI gameUI = null;
    [SerializeField]
    EffectManager effectManager = null;

    private void Start()
    {
        itemName = this.gameObject.tag;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            effectManager.PlayEffect(itemName,this.transform.position);
            gameUI.GetItemUI(itemName);
            Destroy(this.gameObject);
        }
    }

}
