using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{
    //이동속도
    public float moveSpeed = 7f;
    //캐릭터 컨트롤러 컴포넌트 변수
    CharacterController cc; //컴포넌트 변수는 처음에 값을 할당해줘야함. 보통 start에서 ! 혹은 유니티에서 !
    //중력 변수
    float gravity = -20f;
    //수직 속력 변수
    float yVelocity = 0;
    //점프력 변수
    public float jumpPower = 7f;
    //점프 상태 변수
    public bool isJumping = false;

    //플레이어 체력 변수
    public int hp = 100;
    //최대 체력
    int maxHp;
    //체력 슬라이더 변수
    public Slider hpSlider;
    //블러드 효과 이미지
    public GameObject bloodEffect;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        maxHp = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //게임 중 상태에서만 조작
        if(GameManager.gm.gState != GameManager.GameState.Go)
        {
            return;
        }


        //키보드 입력값
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //이동 방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; //해줘야함;

        //카메라를 기준으로 방향전환
        dir = Camera.main.transform.TransformDirection(dir);



        //이동
        //transform.position += dir * moveSpeed *Time.deltaTime;

        //바닥에 착지했다면
        if(cc.collisionFlags == CollisionFlags.Below && isJumping)
        {
            isJumping = false;
            yVelocity = 0;
        }

        //스페이스바로 점프하기
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }


        //수직 속도에 중력값 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //이동
        cc.Move(dir * moveSpeed * Time.deltaTime);

        //현재 체력을 슬라이더 value에 반영하기
        hpSlider.value = (float)hp/(float)maxHp;

    }

    //플레이어 피격 함수
    public void DamageAction(int damage)
    {
        //적의 공격력만큼 플레이어 체력을 감소
        hp -= damage;
        //체력이 음수일 때 0으로 초기화
        if (hp < 0)
        {
            hp = 0;
        }
        else
        {
            //체력이 양수일 때 피격 효과 출력
            StartCoroutine(PlayBloodEffect());
        }
        Debug.Log("플레이어 체력: " + hp);
    }
    //피격 효과 코루틴 함수
    IEnumerator PlayBloodEffect()
    {
        //피격 효과 활성화
        bloodEffect.SetActive(true);
        //0.3초 대기
        yield return new WaitForSeconds(1f);
        //피격 효과 비활성화
        bloodEffect.SetActive(false);
    }


}
