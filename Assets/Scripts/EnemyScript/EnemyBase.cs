using Effekseer;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �G�L�����N�^�[�̊��N���X(�x�[�X)
/// </summary>
public class EnemyBase: MonoBehaviour
{
    protected enum EnemyState
    {
        Patrol,
        Chase,
    }

    [SerializeField]
    protected Transform Player;

    //�A�j���[�V�����A�N�H�[�^�j�I��
    protected Animator EnemyAnim;
    protected Quaternion ThisRota;

    //�l��ύX���Ȃ�����(�萔)
    protected const float SlimeDespawnTime = 2.0f;      //�X���C���f�X�|�[���^�C��
    protected const float BruteDespawnTime = 0.8f;      //Brute�f�X�|�[���^�C�� 
    protected const int IntReset = 0;                   //int�^�̒l���Z�b�g
    protected const float FloatReset = 0.0f;            //float�^�̒l���Z�b�g
    protected const float SetDistance = 1.0f;           //�Z�b�g����
    private const float SetEffectPosY = 0.8f;           //�G�t�F�N�g�ʒu�Z�b�g
    private float SearchAngle = 110.0f;                 //�T�[�`�p�x

    //���g�̃X�e�[�g
    protected EnemyState e_State;

    //�t���O�֌W
    protected bool RunFlag;                   //����t���O
    protected bool DiedFlag = false;
    
    //���g�̏��
    [SerializeField]
    protected int SetEnemyHP;
    [SerializeField]protected int NowEnemyHP;

    //NavMesh
    protected NavMeshAgent Agent;

    //�T�E���h�֘A
    [SerializeField]
    private SoundManager sound;

    //�G�t�F�N�g�֘A
    [SerializeField]
    private EffectManager effect = null;

    //�����_���ړ��Ɏg�p
    public Transform[] PatrolPoint;         //�p�g���[�����̖ڕW�n�_
    protected int DestNum = 0;              //�n�_�̔ԍ�
    private int DestNumMin = 0;             //�n�_�ԍ����莞�Ɏg�p(�ŏ��l)
    private int DestNumMax = 4;             //�n�_�ԍ����莞�Ɏg�p(�ő�l)

    // Start is called before the first frame update
    void Start()
    {
        //�����ݒ�
        Player = GameObject.FindWithTag("Player").transform;        //�v���C���[�I�u�W�F�N�g�擾
        EnemyAnim = this.GetComponent<Animator>();                  //�G�A�j���[�V�����擾 
        Agent = this.GetComponent<NavMeshAgent>();                  //NavMesh��Agent�擾
        NowEnemyHP = SetEnemyHP;                                    //�̗̓Z�b�g(�G���Ƃɐݒ�)

        //�I�u�W�F�N�g���ύX(�N���[���o��)
        if (this.name == "Slime(Clone)")
        {
            this.name = "Slime";
        }

        if (this.name == "Brute(Clone)")
        {
            this.name = "Brute";
        }
    }

    protected void NextPatrolPoint()
    {
        //�����_���Œl������
        DestNum = Random.Range(DestNumMin, DestNumMax);

        //���܂����l�Ƀ|�C���g�������Ă����
        if (PatrolPoint[DestNum] != null)
        {
            //���̃|�C���g�Ɍ����Đi��
            Agent.destination = PatrolPoint[DestNum].position;
        }
    }

    //�s���p�^�[���B�����_���ȕ����ɍU������
    protected void EnemyAttack()
    {
        //�v���C���[�Ƃ̋����𒲂ׂ�
        float PlayerDistance = Vector3.Distance(Player.transform.position, this.transform.position);
        //�v���C���[�Ƃ̋������Z�b�g�����������߂Â����Ƃ�
        if (PlayerDistance <= SetDistance)
        {
            //�U���A�j���[�V�������Đ�
            EnemyAnim.SetTrigger("AttackTrigger");
        }
    }

    //�^�[�Q�b�g�ǐ�
    protected void Chase()
    {
        //�����t���O��^�Ƃ�
        RunFlag = true;
        //�v���C���[�̂�������Ɍ��������킹��
        transform.LookAt(Player.transform.position);
        //�i�ޖڕW���v���C���[�ɐ؂�ւ��Đi��
        Agent.destination = Player.transform.position;

    }

    private void hitDamage()
    {
        //�U����H��������ƃG�t�F�N�g���Đ�����@���L�����ʂɍU�������ω�
        if (this.gameObject.name == "Brute")
        {
            RunFlag = true;
            sound.PlaySE("BruteHitAttack");
            effect.PlayEffect("EnemyHit",transform.position);
        }

        if (this.gameObject.name == "Slime")
        {
            sound.PlaySE("SlimeHitAttack");
            effect.PlayEffect("EnemyHit", transform.position);
        }

        //�G�̏�Ԃ�ǐՏ�Ԃɂ���
        e_State = EnemyState.Chase;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        //�e�ƐڐG�����Ƃ�
        if (collision.gameObject.tag == "Bullet")
        {
            //���g�̗̑͂�1���炷
            NowEnemyHP--;

            //�̗͂�1�ȏ�Ȃ��
            if (!DiedFlag) hitDamage();
            
            if (NowEnemyHP <= IntReset)    //�c��̗͂�0�ɂȂ����Ƃ�
            {
                DiedFlag = true;
                this.gameObject.tag = "Untagged";

                if (this.gameObject.name == "Slime")
                {
                    EnemyAnim.SetTrigger("DieTrigger");
                    effect.PlayEffect("SlimeDied", transform.position);
                    enabled = false;
                    // �|��鉹���Đ�
                    sound.PlaySE("EnemyDied");
                    // ���̌�A��莞�Ԃ��o�߂�����I�u�W�F�N�g������
                    Destroy(this.transform.gameObject, SlimeDespawnTime);
                    return;
                }

                if (this.gameObject.name == "Brute")
                {
                    effect.PlayEffect("BruteDied", transform.position);
                    enabled = false;
                    Destroy(this.transform.gameObject,BruteDespawnTime);
                    return;
                }
            }

        }

        //�������������v���C���[�ł��̗͂�1�ȏ�Ȃ��
        if (collision.gameObject.tag == "Player" && !DiedFlag)
        {
            EnemyAnim.SetTrigger("AttackTrigger");
            //�U�������Đ�
            if (this.gameObject.name == "Brute")
            {
                sound.PlaySE("BruteAttack");
            }

            if (this.gameObject.name == "Slime")
            {
                sound.PlaySE("SlimeAttack");
            }

        }

    }

    //�{���G���A����
    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !DiedFlag)
        {
            //�v���C���[�̕���
            var PlayerDirection = other.transform.position - transform.position;
            //�G�̑O������̎�l���̕���
            var Angle = Vector3.Angle(PlayerDirection, transform.forward);
            //�T�[�`�p�x���ɂ���΁A�ǐՂ�
            if(Angle < SearchAngle)
            {
                e_State = EnemyState.Chase;
            }
        }
    }

    //�T�[�`�͈͂��痣�ꂽ�Ƃ�
    protected void OnTriggerExit(Collider other)
    {
        //�p�g���[���X�e�[�g�ɐ؂�ւ�
        e_State = EnemyState.Patrol;
    }

}
