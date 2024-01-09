using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFire : MonoBehaviour
{
    //�߻� ��ġ
    public GameObject firePosition;
    //��ź ������Ʈ
    public GameObject bombFactory;
    //��ô �Ŀ�
    public float throwPower = 15f;
    //�Ѿ� ����Ʈ
    public GameObject bulletEffect;
    //��ƼŬ �ý���
    ParticleSystem ps;
    //�Ѿ��� ���ݷ�
    public int weaponPower = 10; 

    // Start is called before the first frame update
    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //���콺 ������ ��ư �Է�
        if (Input.GetMouseButtonDown(1)) 
        {
            //��ź����
            GameObject bomb = Instantiate(bombFactory);
            //��ź�� ��ġ�� �߻� ��ġ�� �̵�
            bomb.transform.position = firePosition.transform.position;  
            //��ź�� ������ٵ� ������Ʈ�� ��������
            Rigidbody rb = bomb.GetComponent<Rigidbody>();
            //ī�޶��� �������� ��ź�� ���� ���ϱ�
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse); //impulse:�������� ���� ������
        }


        //���콺 ���� ��ư �Է�
        if (Input.GetMouseButtonDown(0))
        {
            //���� ���� �� ��ġ�� ���� ����
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            //���̰� �ε��� ��� ����
            RaycastHit hitInfo = new RaycastHit();

            //���̸� �߻��� �Ŀ� �ε��� ��ü�� ������ ����Ʈ�� ǥ���ϱ�
            if (Physics.Raycast(ray, out hitInfo))
            {
                //�ε��� ����� ���̾ Enemy���
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    //�ε��� ���(Enemy)�� EnemyFSM�� HitEnemy �Լ� ����
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);

                }
                bulletEffect.transform.position = hitInfo.point; //point : �浹�� ��ġ�� ��ǥ /normal : �浹�� ������ ���� ����

                //�Ѿ� ȿ���� ���̰� �ε��� ������ ���� ���Ϳ� ��ġ��Ű��
                bulletEffect.transform.forward = hitInfo.normal;
                ps.Play();                                                
            }
        }


    }
}
