using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour 
{
    //회전속도 변수
    public float rotSpeed = 200f;
    //회전값 변수
    float mx = 0;
    float my = 0;

    // Start is called before the first frame update
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //게임 중 상태에서만 조작
        if (GameManager.gm.gState != GameManager.GameState.Go)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.Confined;//ctrl+p로 플레이 끄기

        //마우스 입력을 받아오기
        float mouse_X = Input.GetAxis("Mouse X"); //띄어쓰기까지 해야함 주의!!!!!
        float mouse_Y = Input.GetAxis("Mouse Y");

        //회전값 변수에 마우스 입력 값 누적
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        //상하 회전을 -90~90 제한
        my = Mathf.Clamp(my, -90f, 90f);

        //카메라 회전
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
