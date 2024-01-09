using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFire : MonoBehaviour
{
    //발사 위치
    public GameObject firePosition;
    //폭탄 오브젝트
    public GameObject bombFactory;
    //투척 파워
    public float throwPower = 15f;
    //총알 이펙트
    public GameObject bulletEffect;
    //파티클 시스템
    ParticleSystem ps;
    //총알의 공격력
    public int weaponPower = 10; 

    // Start is called before the first frame update
    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 오른쪽 버튼 입력
        if (Input.GetMouseButtonDown(1)) 
        {
            //폭탄생성
            GameObject bomb = Instantiate(bombFactory);
            //폭탄의 위치를 발사 위치로 이동
            bomb.transform.position = firePosition.transform.position;  
            //폭탄의 리지드바디 컴포넌트를 가져오기
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //카메라의 정면으로 폭탄에 힘을 가하기
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse); //impulse:폭발적인 힘을 가해줌
        }


        //마우스 왼쪽 버튼 입력
        if (Input.GetMouseButtonDown(0))
        {
            //레이 생성 후 위치와 방향 설정
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //레이가 부딪힌 대상 저장
            RaycastHit hitInfo = new RaycastHit();

            //레이를 발사한 후에 부딪힌 물체가 있으면 이펙트를 표시하기
            if (Physics.Raycast(ray, out hitInfo))
            {
                //부딪힌 대상의 레이어가 Enemy라면
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //부딪힌 대상(Enemy)의 EnemyFSM의 HitEnemy 함수 실행
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);

                }
                bulletEffect.transform.position = hitInfo.point; //point : 충돌한 위치의 좌표 /normal : 충돌한 지점의 법선 벡터

                //총알 효과를 레이가 부딪힌 지점의 법선 벡터와 일치시키기
                bulletEffect.transform.forward = hitInfo.normal;
                ps.Play();                                                
            }
        }


    }
}
