using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effekseer���g���čĐ�����G�t�F�N�g���ꌳ�Ǘ�
/// </summary>
public class EffectManager : MonoBehaviour
{
    [SerializeField] private EffekseerEffectAsset
        CoinAsset = null, 
        RecoveryAsset = null,
        StageClearAsset = null,
        CompleteClearAsset = null,
        EnemyHitAsset = null,
        BruteDiedAsset = null,
        SlimeDiedAsset = null;

    private float setEnemyEffectPosY = 0.5f;
    private EffekseerHandle EffectPlayHandle;    
    //�G�t�F�N�g���Đ�����֐�(�O������Ăяo���Ďg�p����)
    public void PlayEffect(string StateName,Vector3 ObjPos)
    {
        //��
        if (StateName == "Recovery")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(RecoveryAsset, transform.position);
            return;
        }

        //�R�C���l��
        if (StateName == "Coin")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(CoinAsset, transform.position);
            return;
        }

        //�X�e�[�W�N���A�G�t�F�N�g(�m�[�}��)
        if (StateName == "StageClear")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(StageClearAsset, transform.position);
            return;
        }

        //�X�e�[�W�N���A�G�t�F�N�g(�X�e�[�W���R�C���S�l��)
        if (StateName == "StageCoinClear")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(CompleteClearAsset, transform.position);
            return;
        }

        //�G�ގ��G�t�F�N�g
        if (StateName == "EnemyHit")
        {
            ObjPos.y += setEnemyEffectPosY;
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(EnemyHitAsset,transform.position);
            return;
        }

        if (StateName == "BruteDied")
        {
            ObjPos.y += setEnemyEffectPosY;
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(BruteDiedAsset, transform.position);
            return;
        }

        if (StateName == "SlimeDied")
        {
            ObjPos.y += setEnemyEffectPosY;
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(SlimeDiedAsset, transform.position);
            return;
        }
    }
}
