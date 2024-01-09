using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    //���� �ð�
    public float destroyTime = 6f;
    //��� �ð�
    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��� �ð� > ���� �ð�
        if (currentTime > destroyTime)
        {
            //����Ʈ ����
            Destroy(gameObject);
        }

        //��� �ð� ����
        currentTime += currentTime + Time.deltaTime;
    }
}
