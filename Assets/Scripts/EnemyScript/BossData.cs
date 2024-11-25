using UnityEngine;

public class BossData : MonoBehaviour
{
    const float DieTime = 3.0f;
    public int SetBossHP = 80;                  //�{�X�̗̑͒l
    public int SetMiniBossHP = 50;              //���{�X�̗̑͒l
    [SerializeField] private GameObject KeyObj;         //���I�u�W�F�N�g(�X�e�[�W�ɐݒu)
    [SerializeField] private SoundManager sound;       //�T�E���h�}�l�[�W���[�I�u�W�F�N�g
    [SerializeField] private mainGameUI UIDisPlay;      //UIdisPlay�N���X�擾


    BossMove bMove;                             //BossMove�N���X�擾
    Animator BossAnim;                          //���g�̃A�j���[�^�[�擾
    public int BossHP;                          //���g�̌��݂̗̑�
    private bool BossAttackFlag;

    private void Start()
    {
        //���g����e�R���|�[�l���g�擾
        bMove = this.GetComponent<BossMove>();
        BossAnim = this.GetComponent<Animator>();
        
        //�\�ߌ��I�u�W�F�N�g���A�N�e�B�u�����Ă���
        KeyObj.SetActive(false);

     //���{�X�E�{�X�̗̑͐ݒ�
        if (this.gameObject.name == "BossCharactor")    //���g���{�X�̏ꍇ
        {
            BossHP = SetBossHP; //�{�X�p�̗̑͂�����
        }

        if (this.name == "MiniBoss")    //���g�����{�X�̂Ƃ�
        {
            BossHP = SetMiniBossHP;     //���{�X�p�̗̑͂�����
        }

        sound.PlaySE("Button");
    }

    //��ɏ���
    private void Update()
    {
        //�v���C���[�̗̑͂�0�̂Ƃ�
        if (BossHP == 0)
        {
            //BossMove���A�N�e�B�u��
            bMove.enabled = false;
            //�N���A�p�̌���\��
            KeyObj.SetActive(true);
            //�|���A�j���[�V�������Đ�
            BossAnim.SetTrigger("DieTrigger");
            //�|���SE�Đ�
            sound.PlaySE("EnemyDied");
            //��莞�Ԍo�ߌ�A�{�X�������B
            Destroy(this.gameObject, DieTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            BossHP--;
            UIDisPlay.BossGaugeClass();

        }

        //�v���C���[�ƐڐG�����Ƃ�
        if(collision.gameObject.tag == "Player")
        {
            BossAttackFlag = true;
        }
    }

}
