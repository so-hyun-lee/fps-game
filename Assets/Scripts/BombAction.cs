using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    //폭발 이펙트 변수
    public GameObject bombEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        //효과 프리팹을 생성
        GameObject eff = Instantiate(bombEffect);
        //효과 위치를 폭탄 위치로 이동
        eff.transform.position = transform.position;
        
        //폭탄제거(자기자신)
        Destroy(gameObject);
        
    }

}
