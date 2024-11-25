using Effekseer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

//�v���C���[�̓�����ꕔ�̏����Ǘ�����X�N���v�g
public class Player : MonoBehaviour
{
    // �萔��`
    private const float WATER_SPEED = 2.0f;
    private const float NORMAL_SPEED = 5.0f;
    private const float LANDING_VALUE = 4.0f;
    private const int RECOBERY_HP = 2;
    private const float ROTA_SPEED = 0.25f;
    private const int SET_HP = 20;
    private const int BONUS_HP = 5;
    private const int SET_DAMAGE_FRAME = 250;
    // �ړ����x��ݒ肷��
    private float MoveSpeed = NORMAL_SPEED;   //�L�����N�^�[�̓������x
    private float JumpPower = 7.0f;
    // �t���O�֘A
    private bool DiedFlag = false;               //�m����Ԃ����ׂ�t���O
    private bool HitDamageFlag = true;          //�U��������
    // ���G�t���[��
    private int NoDamageFrame = 0;
    private int jumpCount = 0;

    public int HP { get; private set; }
    public bool keyFlag { get; private set; }

    [SerializeField]
    Rigidbody rbody = null;
    [SerializeField]
    private SoundManager sound = null;
    [SerializeField]
    private EffectManager effect = null;
    [SerializeField]
    public Animator MyAnime = null;
    [SerializeField]
    GunsShot gunShot = null;


    // �v���C���[�̃|�W�V����
    Vector3 SetPlayerPos;   //�����ʒu��ݒ�


    // �v���C���[���
    private enum State
    {
        Normal,
        ItemComplete,
        Died,
    }

    static private Action<Player> _gameClear = null;
    static public void GameClearCallBack(Action<Player> clearCheck)
    {
        _gameClear = clearCheck;
    }

    static private Action<Player> _gameOver = null;
    static public void GameOverCallBack(Action<Player> overCheck)
    {
        _gameOver = overCheck;
    }

    /// <summary>
    /// �Q�[���}�l�[�W���[�Ăяo���̏�����
    /// </summary>
    public void Initialized()
    {
        SetPlayerPos = this.transform.position;
        HP = SET_HP;
    }

    /// <summary>
    /// �v���C���[����(�ʒu�A�p�x)
    /// </summary>
    public void PlayerControll()
    {
        // �ŏ��ɗ���������`�F�b�N
        CheckPlayerLanding();

        // ���ɓG�̍U�����q�b�g���邩
        if (!HitDamageFlag)
        {
            NoDamageFrame++;
            CheckHitEnemyDamageTime();
        }

        //�v���C���[�̈ړ�����(�L�[�{�[�h)
        if (Input.GetKey(KeyCode.W))
        {
            MyAnime.SetBool("RunFlag", Input.GetKey(KeyCode.W));
            this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MyAnime.SetBool("RunFlag", Input.GetKey(KeyCode.S));
            this.transform.position -= this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        else MyAnime.SetBool("RunFlag", false);

        // �W�����v
        if (Input.GetKeyDown(KeyCode.Space))  Jump();

        //���_�E�v���C���[�p�x�ύX
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            this.transform.Rotate(0, -ROTA_SPEED, 0);
        

        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
           this.transform.Rotate(0, ROTA_SPEED, 0);

        // �v���C���[�̍U��
        if (Input.GetKey(KeyCode.P) || Input.GetMouseButton(0))
        {
            //_onPushBullet(transform.position);
            MyAnime.SetTrigger("GunTrigger");
            gunShot.PlayerGunShot();
        }
    }

    /// <summary>
    /// �񕜃A�C�e�����G�ꂽ�Ƃ��Ή�
    /// </summary>
    private void hpCharge()
    {
        //HP�񕜏���
        HP += RECOBERY_HP;
        //HP������𒴂��Ă���Ƃ���
        if (HP >= SET_HP)
            HP = SET_HP;
        //�񕜉��Đ�
        sound.PlaySE("Recovery");
        effect.PlayEffect("Recovery",transform.position);
    }

    /// <summary>
    /// ���G���Ԃ𐧌�
    /// </summary>
    private void CheckHitEnemyDamageTime()
    {
        if (NoDamageFrame > SET_DAMAGE_FRAME)
        {
            NoDamageFrame = 0;
            HitDamageFlag = true;
        }
    }

    /// <summary>
    /// �W�����v����
    /// </summary>
    private void Jump()
    {
        // �J�E���^�[��0�̂Ƃ�
        if (jumpCount == 0)
        {
            jumpCount++;
            //�W�����v����
            MyAnime.SetTrigger("JumpTrigger");
            //�W�����v�����Đ�����
            sound.PlaySE("Jump");
            //AddForce���g�����W�����v
            rbody.AddForce(new Vector3(0.0f, JumpPower, 0.0f), ForceMode.Impulse);
        }
    }

    /// <summary>
    /// �t�B�[���h����̗�������
    /// </summary>
    private void CheckPlayerLanding()
    {
        //�v���C���[�̗�������
        if (this.transform.position.y <= -LANDING_VALUE)
        {
            if (DiedFlag == false)
            {
                //�_���[�W�q�b�g
                HitDamage();
                this.transform.position = SetPlayerPos;
            }
        }
    }

    /// <summary>
    /// �_���[�W����
    /// </summary>
    private void HitDamage()
    {
        HP--;

        if (HP <= 0 && DiedFlag == false) Died();
    }

    /// <summary>
    /// �v���C���[�����񂾂Ƃ��Ɏ��s���鏈��
    /// </summary>
    private void Died()
    {
        DiedFlag = true;
        sound.PlaySE("GameOver");
        MyAnime.SetTrigger("DiedTrigger");
        _gameOver(this);
        this.gameObject.tag = "Untagged";
    }

    /// <summary>
    /// ���炩�̃I�u�W�F�N�g�ƐڐG�����Ƃ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        jumpCount = 0;
        //��C�܂��͓G�Ƃ̐ڐG�t���O
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("CannonBall"))
        {
            // Died�t���O���U�ōU���������^�C�~���O�ł���Ȃ�
            if (DiedFlag == false && HitDamageFlag)
            {
                //HP�����炷
                HitDamage();
                HitDamageFlag = false;
            }

            //HP��0�ɂȂ�����
            if (HP == 0 && DiedFlag == false)
            {
                //Died�֐����Ăяo��
                Died();
            }
        }

        //�����t�B�[���h�̃t���O
        if (collision.gameObject.CompareTag("MoveField"))    
            this.transform.SetParent(collision.transform);
        else transform.SetParent(null);
    }

    /// <summary>
    /// �v���C���[���g���K�[�ɐG�ꂽ��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Recovery"))
        {
            //���݂̗̑͒l���ݒ�l�Ɠ���������ȏ�Ȃ甲����
            if (HP >= SET_HP)  return;
            
            Destroy(other.gameObject);
            // ���J�o���[�֐��Ăяo��
            hpCharge();
        }

        if (other.gameObject.CompareTag("Key"))
        {
            sound.PlaySE("GetKey");
            keyFlag = true;
        }

        if (other.gameObject.CompareTag("Castle"))
        {
            _gameClear(this);
        }
    }

    /// <summary>
    /// �g���K�[���ɓ����Ă���Ƃ�
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        //���ɓ����t���O(�����Ă���Ȃ�A���x�𗎂Ƃ�)
        if (other.gameObject.CompareTag("Water"))
            MoveSpeed = WATER_SPEED;


        //�K�i����
        if (other.gameObject.CompareTag("Stairs"))    //�v���C���[���K�i�ɗ����Ă���
        {
            float StairsHeight = other.gameObject.transform.position.y - this.gameObject.transform.position.y;
            //���A�o��鍂���ɂ���Ȃ�
            if (StairsHeight <= 0.7f)
            {
                //�v���C���[�ɏ�����̗͂������āA�K�i�����
                rbody.velocity = transform.up;
            }
        }
    }

    /// <summary>
    /// �g���K�[���甲������
    /// </summary>
    /// <param name="other">�R���C�_�[</param>
    private void OnTriggerExit(Collider other)
    {
        //������オ�����Ƃ��A���x�����Ƃɖ߂��܂�
        if (other.gameObject.CompareTag("Water"))
            MoveSpeed = NORMAL_SPEED;
    }

    /// <summary>
    /// HP�̍ő�l�Q�b�^�[
    /// </summary>
    /// <returns></returns>
    public int getMaxHP()
    {
        return SET_HP;
    }
}
