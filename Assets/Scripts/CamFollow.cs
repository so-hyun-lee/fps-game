using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //CamPosition �� transform component�� ������
    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ī�޶� ��ġ�� Ÿ�� ��ġ�� ��ġ��Ű��
        transform.position = target.position;
    }
}
