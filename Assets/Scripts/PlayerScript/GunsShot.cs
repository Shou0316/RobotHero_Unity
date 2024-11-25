using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using UnityEngine;

public class GunsShot : MonoBehaviour
{
    [SerializeField] private GameObject BulletModel = null;      //�V���b�g���f��
    [SerializeField] private SoundManager sound = null;
    [SerializeField] private Player _player = null;
    //�����G�t�F�N�g
    [SerializeField]GameObject ShotPoint;       //���˓_(�v���C���[�Ƒ�C)
    bool CannonFlag;
    private float ShotSpeed = 20.0f;    //���ˑ��x
    private float Rate = 0.5f;          //
    private float FrameCount = 0;       //�t���[���J�E���g
    private int ShotCount = 0;          //�V���b�g�J�E���g
    Animator MyAnim;
   
    const float SetCannonTime = 1.5f;

    float ShotTimer;
    const float SetShotTimer = 0.1f;

    float CannonTime = 0.0f;
 
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.tag == "Player")
        {
            _player = this.gameObject.GetComponent<Player>();
            MyAnim = this.gameObject.GetComponent<Animator>();
        }
    }

    //��ɏ���
    void Update()
    {
        FrameCount++;

        if (this.gameObject.tag == "Cannon")
        {
            CannonGunShot();
        }
    }

    //�v���C���[�V���b�g
    public void PlayerGunShot()
    {
        if (ShotTimer <= 0.0f)
        {
            sound.PlaySE("GunShot");

            //�e�𔭎˂���������擾���܂�
            Vector3 BulletPosition = ShotPoint.transform.position;

            //�v���C���[�̊p�x�ɉ����Ēe�𔭎˂��܂�
            GameObject Shot = Instantiate(BulletModel, BulletPosition, transform.localRotation);
            Vector3 Direction = Shot.transform.forward;
            //AddForce��p��������
            Shot.GetComponent<Rigidbody>().AddForce(Direction * ShotSpeed, ForceMode.Impulse);
            ShotTimer = SetShotTimer;
        }
        else if(ShotTimer > 0.0f)
        {
            ShotTimer -= Time.deltaTime;
        }
    }

    //��C�V���b�g�i�G�j
    void CannonGunShot()
    {
        //��莞�Ԃ��Ƃɏ������s���܂�
        Timer CannonTimer = new Timer(500);
        CannonTime += Time.deltaTime;
            
        Vector3 CannonBulletPosition = ShotPoint.transform.position;

        if(CannonTime >= SetCannonTime)
        {
            //CannonTimer�̊Ԋu�Œe�𔭎˂��܂�
            GameObject CannonShot = Instantiate(BulletModel, CannonBulletPosition, transform.localRotation);
            Vector3 Direction = CannonShot.transform.forward;
            //AddForce��p��������
            CannonShot.GetComponent<Rigidbody>().AddForce(Direction * ShotSpeed, ForceMode.Impulse);
            CannonTime = 0.0f;
        }
    }

    private IEnumerator RapidFire()
    {
        while(true)
        {
            yield return new WaitForSeconds(Rate);
        }
    }
}
