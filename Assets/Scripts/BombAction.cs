using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    //���� ����Ʈ ����
    public GameObject bombEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�浹���� ��
    private void OnCollisionEnter(Collision collision)
    {
        //ȿ�� �������� ����
        GameObject eff = Instantiate(bombEffect);
        //ȿ�� ��ġ�� ��ź ��ġ�� �̵�
        eff.transform.position = transform.position;
        
        //��ź����(�ڱ��ڽ�)
        Destroy(gameObject);
        
    }

}
