using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���̍����≡�����Ƀt�B�[���h�������X�N���v�g
public class MoveField : MonoBehaviour
{
    //���x�ω�����
    public bool upDown;
    //�T�C�h
    public bool sideSide;

    private float timer;
    private int state = 0;
    //�ҋ@����
    public float waitTime = 3;
    //�ړ�����
    public float time = 2;
    //�ړ��X�s�[�h
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
            //�X�e�[�g��0�Ȃ��
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
