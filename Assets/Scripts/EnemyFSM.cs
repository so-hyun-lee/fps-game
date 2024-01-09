using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFSM : MonoBehaviour
{
    //적 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    //적 상태 변수
    EnemyState m_State;

    
    public float findDistance = 8f; //플레이어 발견 범위

    public float attackDistance = 2f; //공격 범위

    public float moveSpeed = 4f; //적 이동 속도

    //캐릭터 컨트롤러 컴포넌트
    CharacterController cc;

    //플레이어 트랜스폼 컴포넌트(프라이빗으로 만든 후 명령어를 통해 플레이어에 코드로 넣는 방식)
    Transform player; //player.position으로 가지고 올 수 있음
    //GameObject player; 의 경우 player.transform.position 으로 가져와야함

    float currentTime = 0; //누적 시간
    float attackDelay = 2f; //공격 딜레이 시간

    public int attackPower = 10; //공격력

    Vector3 originPos; //초기 위치 저장
    Quaternion originRot; //초기 각도 저장

    public float moveDistance = 20f; //이동 가능 범위

    public int hp = 30; //적 체력
    public int maxHp = 30; //적 최대 체력

    //체력 슬라이더 변수
    public Slider hpSlider;

    //애니메이터 변수
    Animator anim;







    // Start is called before the first frame update
    void Start()
    {
        //플레이어 트랜스폼 컴포넌트 할당
        player = GameObject.Find("Player").transform;  //GameObject의 ! 트랜스폼 형태이기 때문에 트랜스폼을 뒤에 붙여줘야함!!!!
        //최초의 적 상태를 대기로 설정
        m_State = EnemyState.Idle;
        //캐릭터 컨트롤러 컴포넌트 할당
        cc = GetComponent<CharacterController>();
        //적 초기 위치/각도 저장
        originPos = transform.position;
        originRot = transform.rotation;
        //자식 오브젝트의 애니메이터 컴포넌트 할당
        anim = transform.GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //적 상태를 체크해서 상태별로 기능 수행
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
        //플레이어와 적의 거리가 발견 범위 이내라면 Move상태로 전환
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            Debug.Log("상태 전환:Idle -> Move");
            //이동 애니메이션으로 전환
            anim.SetTrigger("IdleToMove");
        }
    }

    void Move()
    {
        //만약 현재 위치와 초기 위치의 거리가 이동 가능 범위보다 크다면 복귀
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State=EnemyState.Return;
            Debug.Log("상태 전환:Move -> Return");
            //transform.position = originPos;
            
        }

        //플레이어와 적의 거리가 공격 범위보다 크다면 플레이어를 향해서 이동
        else if(Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            //이동 방향
            Vector3 dir = (player.position - transform.position).normalized;

            //이동
            cc.Move(dir * moveSpeed * Time.deltaTime);
            //플레이어를 향해서 방향 전환
            transform.forward = dir;
        }
        else
        {
            m_State = EnemyState.Attack;
            Debug.Log("상태 전환:Move -> Attack");
            //누적 시간을 공격 딜레이 시간만큼 미리 진행
            currentTime = attackDelay;
            //공격 대기 애니메이션 실행
            anim.SetTrigger("MoveToAttackDelay");

        }


    }

    void Attack()
    {
        //거리가 공격범위 이내라면 공격
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            //일정 시간마다 플레이어 공격
            currentTime += Time.deltaTime;

            if (currentTime > attackDelay)
            {
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격!!");
                currentTime = 0;

                //공격 애니메이션 실행
                anim.SetTrigger("StartAttack");
            }
            
        }
        else
        {
            m_State = EnemyState.Move;
            Debug.Log("상태 전환:Attack -> Move");
            currentTime = 0;
            //이동 애니메이션 실행
            anim.SetTrigger("AttackToMove");
        }
    }

    void Return()
    {
        //초기 위치에서의 거리가 0.1보다 크다면 초기 위치로 이동
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            //방향(목적지 - 내 위치)
            Vector3 dir = (originPos - transform.position).normalized;
            //이동
            cc.Move(dir * moveSpeed * Time.deltaTime);
            //복귀 지점으로 방향 전환
            transform.forward = dir;
        }
        else //그렇지 않다면 = 거리가 0.1보다 작아지면
        {
            transform.position = originPos;
            transform.rotation = originRot;

            m_State = EnemyState.Idle;
            Debug.Log("상태 전환: Return -> Idle");
            //대기 애니메이션으로 전환
            anim.SetTrigger("MoveToIdle");
        }

    }


    //데미지 처리 코루틴 함수
    IEnumerator DamageProcess()
    {
        //피격 모션만큼 기다리기
        yield return new WaitForSeconds(0.5f);
        //이동 상태로 전환
        m_State = EnemyState.Move;
        //이동 애니메이션 실행
        anim.SetTrigger("IdleToMove");
    }

    void Damaged()
    {
        //피격 코루틴 실행
        StartCoroutine(DamageProcess());
    }



    //데미지 실행 함수
    public void HitEnemy(int hitPower)
    {
        if(m_State == EnemyState.Damaged)
        {
            return;
        }

        //플레이어의 공격력만큼 적 체력 감소
        hp -= hitPower;
        //적 체력이 0보다 크면 피격
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            Damaged();
            Debug.Log("적 체력: " + hp);
        }
        //0보다 작으면 죽음
        else
        {
            m_State = EnemyState.Die;
            Debug.Log("죽음");
            //죽음 애니메이션
            anim.SetTrigger("Die");
            Die();

        }
    }

    IEnumerator DieProcess()
    {
        //캐릭터 컨트롤러 컴포넌트 비활성화
        cc.enabled = false;
        //2초 대기
        yield return new WaitForSeconds(2f);
        //적 제거
        Destroy(gameObject);
    }

    void Die()
    {
        //실행 중인 코루틴 중지
        StopAllCoroutines();
        //죽음 코루틴 실행
        StartCoroutine(DieProcess());       
    }




}
