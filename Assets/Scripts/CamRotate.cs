using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour 
{
    //ȸ���ӵ� ����
    public float rotSpeed = 200f;
    //ȸ���� ����
    float mx = 0;
    float my = 0;

    // Start is called before the first frame update
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //���� �� ���¿����� ����
        if (GameManager.gm.gState != GameManager.GameState.Go)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.Confined;//ctrl+p�� �÷��� ����

        //���콺 �Է��� �޾ƿ���
        float mouse_X = Input.GetAxis("Mouse X"); //������� �ؾ��� ����!!!!!
        float mouse_Y = Input.GetAxis("Mouse Y");

        //ȸ���� ������ ���콺 �Է� �� ����
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        //���� ȸ���� -90~90 ����
        my = Mathf.Clamp(my, -90f, 90f);

        //ī�޶� ȸ��
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
