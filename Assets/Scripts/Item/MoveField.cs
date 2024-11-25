using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一定の高さや横方向にフィールドが動くスクリプト
public class MoveField : MonoBehaviour
{
    //高度変化判定
    public bool upDown;
    //サイド
    public bool sideSide;

    private float timer;
    private int state = 0;
    //待機時間
    public float waitTime = 3;
    //移動時間
    public float time = 2;
    //移動スピード
    public float speed = 2;

    // Use this for initialization
    void Start()
    {
        state = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (sideSide == true)
        {
            //ステートが0ならば
            if (state == 0)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 1;
                }
            }
            if (state == 1)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 2;
                }
            }
            if (state == 2)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.forward * Time.deltaTime * -speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 3;
                }
            }
            if (state == 3)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 0;
                }
            }
        }

        if (upDown == true)
        {
            if (state == 0)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.up * Time.deltaTime * speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 1;
                }
            }
            if (state == 1)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 2;
                }
            }
            if (state == 2)
            {
                timer += Time.deltaTime;
                transform.Translate(Vector3.up * Time.deltaTime * -speed);
                if (timer >= time)
                {
                    timer = 0;
                    state = 3;
                }
            }
            if (state == 3)
            {
                timer += Time.deltaTime;
                if (timer >= waitTime)
                {
                    timer = 0;
                    state = 0;
                }
            }
        }

    }
}
