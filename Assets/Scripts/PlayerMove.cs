using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{
    //�̵��ӵ�
    public float moveSpeed = 7f;
    //ĳ���� ��Ʈ�ѷ� ������Ʈ ����
    CharacterController cc; //������Ʈ ������ ó���� ���� �Ҵ��������. ���� start���� ! Ȥ�� ����Ƽ���� !
    //�߷� ����
    float gravity = -20f;
    //���� �ӷ� ����
    float yVelocity = 0;
    //������ ����
    public float jumpPower = 7f;
    //���� ���� ����
    public bool isJumping = false;

    //�÷��̾� ü�� ����
    public int hp = 100;
    //�ִ� ü��
    int maxHp;
    //ü�� �����̴� ����
    public Slider hpSlider;
    //���� ȿ�� �̹���
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
        //���� �� ���¿����� ����
        if(GameManager.gm.gState != GameManager.GameState.Go)
        {
            return;
        }


        //Ű���� �Է°�
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //�̵� ���� ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; //�������;

        //ī�޶� �������� ������ȯ
        dir = Camera.main.transform.TransformDirection(dir);



        //�̵�
        //transform.position += dir * moveSpeed *Time.deltaTime;

        //�ٴڿ� �����ߴٸ�
        if(cc.collisionFlags == CollisionFlags.Below && isJumping)
        {
            isJumping = false;
            yVelocity = 0;
        }

        //�����̽��ٷ� �����ϱ�
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }


        //���� �ӵ��� �߷°� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //�̵�
        cc.Move(dir * moveSpeed * Time.deltaTime);

        //���� ü���� �����̴� value�� �ݿ��ϱ�
        hpSlider.value = (float)hp/(float)maxHp;

    }

    //�÷��̾� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        //���� ���ݷ¸�ŭ �÷��̾� ü���� ����
        hp -= damage;
        //ü���� ������ �� 0���� �ʱ�ȭ
        if (hp < 0)
        {
            hp = 0;
        }
        else
        {
            //ü���� ����� �� �ǰ� ȿ�� ���
            StartCoroutine(PlayBloodEffect());
        }
        Debug.Log("�÷��̾� ü��: " + hp);
    }
    //�ǰ� ȿ�� �ڷ�ƾ �Լ�
    IEnumerator PlayBloodEffect()
    {
        //�ǰ� ȿ�� Ȱ��ȭ
        bloodEffect.SetActive(true);
        //0.3�� ���
        yield return new WaitForSeconds(1f);
        //�ǰ� ȿ�� ��Ȱ��ȭ
        bloodEffect.SetActive(false);
    }


}
