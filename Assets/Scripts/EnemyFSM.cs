using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    //�� ���� ���
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    //�� ���� ����
    EnemyState m_State;

    
    public float findDistance = 8f; //�÷��̾� �߰� ����

    public float attackDistance = 2f; //���� ����

    public float moveSpeed = 4f; //�� �̵� �ӵ�

    //ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    //�÷��̾� Ʈ������ ������Ʈ(�����̺����� ���� �� ��ɾ ���� �÷��̾ �ڵ�� �ִ� ���)
    Transform player; //player.position���� ������ �� �� ����
    //GameObject player; �� ��� player.transform.position ���� �����;���

    float currentTime = 0; //���� �ð�
    float attackDelay = 2f; //���� ������ �ð�

    public int attackPower = 10; //���ݷ�

    Vector3 originPos; //�ʱ� ��ġ ����
    Quaternion originRot; //�ʱ� ���� ����

    public float moveDistance = 20f; //�̵� ���� ����

    public int hp = 30; //�� ü��
    public int maxHp = 30; //�� �ִ� ü��

    //ü�� �����̴� ����
    public Slider hpSlider;

    //�ִϸ����� ����
    Animator anim;







    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾� Ʈ������ ������Ʈ �Ҵ�
        player = GameObject.Find("Player").transform;  //GameObject�� ! Ʈ������ �����̱� ������ Ʈ�������� �ڿ� �ٿ������!!!!
        //������ �� ���¸� ���� ����
        m_State = EnemyState.Idle;
        //ĳ���� ��Ʈ�ѷ� ������Ʈ �Ҵ�
        cc = GetComponent<CharacterController>();
        //�� �ʱ� ��ġ/���� ����
        originPos = transform.position;
        originRot = transform.rotation;
        //�ڽ� ������Ʈ�� �ִϸ����� ������Ʈ �Ҵ�
        anim = transform.GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //�� ���¸� üũ�ؼ� ���º��� ��� ����
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            //case EnemyState.Damaged: 
            //    Damaged();
            //    break;

        }

        hpSlider.value = (float)hp / (float)maxHp;

    }

    void Idle()
    {
        //�÷��̾�� ���� �Ÿ��� �߰� ���� �̳���� Move���·� ��ȯ
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log("���� ��ȯ:Idle -> Move");
            //�̵� �ִϸ��̼����� ��ȯ
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        //���� ���� ��ġ�� �ʱ� ��ġ�� �Ÿ��� �̵� ���� �������� ũ�ٸ� ����
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State=EnemyState.Return;
            Debug.Log("���� ��ȯ:Move -> Return");
            //transform.position = originPos;
            
        }

        //�÷��̾�� ���� �Ÿ��� ���� �������� ũ�ٸ� �÷��̾ ���ؼ� �̵�
        else if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //�̵� ����
            Vector3 dir = (player.position - transform.position).normalized;

            //�̵�
            cc.Move(dir * moveSpeed * Time.deltaTime);
            //�÷��̾ ���ؼ� ���� ��ȯ
            transform.forward = dir;
        }
        else
        {
            m_State = EnemyState.Attack;
            Debug.Log("���� ��ȯ:Move -> Attack");
            //���� �ð��� ���� ������ �ð���ŭ �̸� ����
            currentTime = attackDelay;
            //���� ��� �ִϸ��̼� ����
            anim.SetTrigger("MoveToAttackDelay");

        }


    }

    void Attack()
    {
        //�Ÿ��� ���ݹ��� �̳���� ����
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //���� �ð����� �÷��̾� ����
            currentTime += Time.deltaTime;

            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����!!");
                currentTime = 0;

                //���� �ִϸ��̼� ����
                anim.SetTrigger("StartAttack");
            }
            
        }
        else
        {
            m_State = EnemyState.Move;
            Debug.Log("���� ��ȯ:Attack -> Move");
            currentTime = 0;
            //�̵� �ִϸ��̼� ����
            anim.SetTrigger("AttackToMove");
        }
    }

    void Return()
    {
        //�ʱ� ��ġ������ �Ÿ��� 0.1���� ũ�ٸ� �ʱ� ��ġ�� �̵�
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            //����(������ - �� ��ġ)
            Vector3 dir = (originPos - transform.position).normalized;
            //�̵�
            cc.Move(dir * moveSpeed * Time.deltaTime);
            //���� �������� ���� ��ȯ
            transform.forward = dir;
        }
        else //�׷��� �ʴٸ� = �Ÿ��� 0.1���� �۾�����
        {
            transform.position = originPos;
            transform.rotation = originRot;

            m_State = EnemyState.Idle;
            Debug.Log("���� ��ȯ: Return -> Idle");
            //��� �ִϸ��̼����� ��ȯ
            anim.SetTrigger("MoveToIdle");
        }

    }


    //������ ó�� �ڷ�ƾ �Լ�
    IEnumerator DamageProcess()
    {
        //�ǰ� ��Ǹ�ŭ ��ٸ���
        yield return new WaitForSeconds(0.5f);
        //�̵� ���·� ��ȯ
        m_State = EnemyState.Move;
        //�̵� �ִϸ��̼� ����
        anim.SetTrigger("IdleToMove");
    }

    void Damaged()
    {
        //�ǰ� �ڷ�ƾ ����
        StartCoroutine(DamageProcess());
    }



    //������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        if(m_State == EnemyState.Damaged)
        {
            return;
        }

        //�÷��̾��� ���ݷ¸�ŭ �� ü�� ����
        hp -= hitPower;
        //�� ü���� 0���� ũ�� �ǰ�
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            Damaged();
            Debug.Log("�� ü��: " + hp);
        }
        //0���� ������ ����
        else
        {
            m_State = EnemyState.Die;
            Debug.Log("����");
            //���� �ִϸ��̼�
            anim.SetTrigger("Die");
            Die();

        }
    }

    IEnumerator DieProcess()
    {
        //ĳ���� ��Ʈ�ѷ� ������Ʈ ��Ȱ��ȭ
        cc.enabled = false;
        //2�� ���
        yield return new WaitForSeconds(2f);
        //�� ����
        Destroy(gameObject);
    }

    void Die()
    {
        //���� ���� �ڷ�ƾ ����
        StopAllCoroutines();
        //���� �ڷ�ƾ ����
        StartCoroutine(DieProcess());       
    }




}
