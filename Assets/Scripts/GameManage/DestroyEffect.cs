using UnityEngine;
using System.Collections;

//エフェクトの消滅タイミングを指示・実行するスクリプト
public class DestroyEffect : MonoBehaviour {

    const float DeleteTime = 1.5f;
    const float ResetTime = 0.0f;

    //エフェクトが消えるまでのカウント
    float DeleteCountTime;

    //接触して爆発時の音
    [SerializeField] private AudioSource explosionSE;


    private void Start()
    {
        DeleteCountTime = 0.0f;
        explosionSE = this.GetComponent<AudioSource>();
    }

    void Update ()
	{

		if(Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C))
		   Destroy(transform.gameObject);

        //explosionSE.PlayOneShot(explosionSE.clip);

        //出てきたときにタイムカウント
        if(this.gameObject.activeSelf == true)
        {
            DeleteCountTime += Time.deltaTime;
            if(DeleteCountTime >= DeleteTime)
            {
                DeleteCountTime = ResetTime;
                Destroy(this.gameObject);
            }
        }
	
	}
}
