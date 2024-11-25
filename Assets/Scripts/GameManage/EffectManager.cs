using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effekseerを使って再生するエフェクトを一元管理
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
    //エフェクトを再生する関数(外部から呼び出して使用する)
    public void PlayEffect(string StateName,Vector3 ObjPos)
    {
        //回復
        if (StateName == "Recovery")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(RecoveryAsset, transform.position);
            return;
        }

        //コイン獲得
        if (StateName == "Coin")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(CoinAsset, transform.position);
            return;
        }

        //ステージクリアエフェクト(ノーマル)
        if (StateName == "StageClear")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(StageClearAsset, transform.position);
            return;
        }

        //ステージクリアエフェクト(ステージ内コイン全獲得)
        if (StateName == "StageCoinClear")
        {
            this.transform.position = ObjPos;
            EffectPlayHandle = EffekseerSystem.PlayEffect(CompleteClearAsset, transform.position);
            return;
        }

        //敵退治エフェクト
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
