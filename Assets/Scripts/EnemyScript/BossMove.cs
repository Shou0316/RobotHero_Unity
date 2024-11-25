using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// �{�X�L�����N�^�[(���{�X�A���C���{�X)�̍s���p�^�[��
/// </summary>
public class BossMove : MonoBehaviour
{
    enum State
    {
        Chase,
        Attack,
        Died
    }

    //�I�u�W�F�N�g�A�g�����X�t�H�[���錾
    [SerializeField]
    private GameObject setEnemy;          //�G�̔z��
    [SerializeField]
    private Transform Central;           //�G�̒��S
    private Transform Player;            //�v���C���[

    [SerializeField]
    private SoundManager sound;
    private Animator BossAnim;
    //�萔
    const float TargetAngle = 30.0F;        //�^�[�Q�b�g�p�x
    const int HalfHP = 2;                   //�{�X�̗̑͂Ɗ���Z�p
    const int MaxEnemyCount = 8;            //�t�B�[���h���ɏo������G�̏��

    //�����_���ړ��֌W
    private float TimeCount;
    private float CountTime = 0.0f;         //���Ԍv��
    private float WaitTime = 3.0f;          //�ҋ@���Ԑݒ�

    //�t���O�֌W
    private bool WalkFlag;                  //���s�t���O
    private bool RunFlag;                   //����t���O
    private bool TrackPlayerFlag = false;   //�v���C���[�ǐՃt���O

    private int EnemyNum = 5;
    private int BossActJuge;    //�{�X�̍s������
    private int EnemyCount;     //�G���J�E���g

    //NavMesh
    NavMeshAgent Agent;

    //�ڕW�n�_
    Vector3 TargetPos;

    //�{�X��Ԏ擾
    BossData bossData;

    //���̑��l
    const float DistanceValue = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //�g�����X�t�H�[���擾
        Player = GameObject.FindWithTag("Player").transform;    //�v���C���[
        //�e�R���|�[�l���g�擾
        BossAnim = this.GetComponent<Animator>();               //�A�j���[�^�[
        Agent = this.GetComponent<NavMeshAgent>();              //NavMeshAgent
        bossData = this.GetComponent<BossData>();               //���g(�{�X)�̃X�e�[�^�X
        //����t���O
        RunFlag = false;
        //���W�b�h�{�f�B�[�擾
        Agent.destination = Player.position;
        
        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        //�G�̐����J�E���g
        EnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        //�{�X�̗̑͂�1�ȏ�Ȃ��
        if(bossData.BossHP >= 1)
            FindPlayer(TargetAngle);


        //�ړ��ȊO�̍s���p�^�[���������_���Ɍ���
        BossActJuge = Random.Range(0, 4);
        
        //1�ԂȂ烉���_���Ƀv���C���[�U��
        if(BossActJuge == 0 && bossData.BossHP >= 1)
            BossAttack(); 

        // ���g���ŏI�{�X�Ȃ�ΓG���o��������
        if(BossActJuge == 1 && this.gameObject.name == "BossCharactor")
        {
            //�{�X�̗̑͂������ȉ��ł��A�G�̏o������ɖ������Ă��Ȃ��Ƃ�
            if (bossData.BossHP <= bossData.SetBossHP / HalfHP &&  EnemyCount <= MaxEnemyCount)
                SetEnemy();
        }

        //�v���C���[��ǐՂ��Ă��Ȃ��Ƃ��̓����_�����s(��)
        if (TrackPlayerFlag == false)
            NextPoint();

        //�̗͂�0�ɂȂ�����A���s��ǐՃt���O���~�߂�
        if(bossData.BossHP == 0)
        {
            WalkFlag = false;
        }
    }

    //�s���p�^�[���@�v���C���[��T��
    void FindPlayer(float Angle)
    {
        //�v���C���[�̃x�N�g�����擾
        Vector3 TargetVec = Player.position - this.transform.position;
        //�x�N�g���Ԃ̓��ς��v�Z
        var Dot = Vector3.Dot(transform.forward, TargetVec.normalized);
        //���ς��p�x�v�Z
        var Deg = Mathf.Acos(Dot) * Mathf.Rad2Deg;

        //�{�X�̎���p���Ƀv���C���[�������Ă���Ƃ�
        if (Deg <= Angle)
        {
            TrackPlayerFlag = true;
            WalkFlag = false;
            Agent.destination = Player.position;
            BossAnim.SetBool("RunFlag", TrackPlayerFlag);
        }
        else //����p���ɓ����Ă��Ȃ��Ƃ�
        {
            //�֘A�t���O���U�ɂ���
            TrackPlayerFlag = false;
            WalkFlag = true;
        }
    }

    //�s���p�^�[���A-1 ���̖ڕW�n�_��ݒ肵�āA�ڕW�֌�����
    void NextPoint()
    {
        TrackPlayerFlag = false;
        WalkFlag = true;

        //���W�ύX
        Agent.destination = TargetPos;
        CountTime += Time.deltaTime;

        //�X�g�b�v�����Ȃ�
        Agent.isStopped = false;
        //�A�j���[�V�����Đ�
        BossAnim.SetBool("WalkFlag", WalkFlag);

        //�^�[�Q�b�g�͈͓��ɂ���Ƃ�
        if(this.transform.position.x == TargetPos.x &&
           this.transform.position.z == TargetPos.z || TimeCount >= WaitTime)
        {
            StopMove();
            ChangeDirection();
        }

    }

    void ChangeDirection()
    {
        float RandomX = Random.Range(-1f, 1f);
        float RandomZ = Random.Range(-1, 1f);
        TargetPos = new Vector3(RandomX, 0f, RandomZ);
    }

    //�s���p�^�[���A-2 �ڕW�n�_�ɂ��āA�X�g�b�v����
    void StopMove()
    {
        //�X�g�b�v������
        Agent.isStopped = true;
        //�҂����Ԃ��v��
        CountTime += Time.deltaTime;
        RunFlag = false;

        //�҂����Ԃ�ݒ肳�ꂽ���l�𒴂���ƋN��
        if(CountTime > WaitTime)
        {
            NextPoint();
            CountTime = 0.0f;
            ChangeDirection();
        }
    }

    //�s���p�^�[���B�����_���ȕ����ɍU������
    void BossAttack()
    {
        if(bossData.BossHP >= 1)
        {
            //�v���C���[�Ƃ̋����𒲂ׂ�
            float PlayerDistance = Vector3.Distance(Player.transform.position, this.gameObject.transform.position);
            //�v���C���[�Ƃ̋��������߂Â����Ƃ�
            if (PlayerDistance <= DistanceValue)
            {
                //�U���A�j���[�V�������Đ�
                BossAnim.SetTrigger("AttackTrigger");
            }
        }
    }

    //�s���p�^�[���C�t�B�[���h�ɓG���o��������
    void SetEnemy()
    {
        setEnemy.SetActive(true);
    }

    //�R���W�������m�����ƐڐG�����Ƃ�
    private void OnCollisionEnter(Collision collision)
    {
        //�{�X�̗̑͂�1�ȏ��
        if(bossData.BossHP >= 1)
        {
            //�e������������A�v���C���[�֍U�����鏈��
            if (collision.gameObject.tag == "Bullet")
            {
                //�{���t���O��^
                TrackPlayerFlag = true;
                //�v���C���[��ǐ�
                Agent.destination = Player.position;
                //�U�����󂯂������Đ�����
                sound.PlaySE("EnemyHitAttack");
            }

            //�v���C���[�ɓ���������A�v���C���[����̍U������
            if (collision.gameObject.tag == "Player")
            {
                BossAnim.SetTrigger("AttackTrigger");
                sound.PlaySE("BruteAttack");
            }

        }
    }

    //�{���G���A����
    protected void OnTriggerStay(Collider other)
    {
        //�G���A���Ƀv���C���[������Α{���t���O�^
        if (other.gameObject.tag == "Player")
            TrackPlayerFlag = true;
    }

    protected void OnTriggerExit(Collider other)
    {
        //�G���A���Ƀv���C���[�����Ȃ���Α{���t���O�U
        if (other.gameObject.tag == "Player")
            TrackPlayerFlag = false;
    }

}
