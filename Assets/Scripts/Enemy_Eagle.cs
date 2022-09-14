using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;

    public Transform upPoint, downPoint;    //�ƶ��޽��
    public float speed;
    private float up_y, down_y;             //�޽���y��

    private bool faceDown = true;           //һ��ʼ������

    
    protected override void Start()             //��д����
    {
        base.Start();                           //��ȡ�����Start
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();         //�Ͼ����࣬�����޽��Ͳ��������ӥ�ƶ�
        up_y = upPoint.position.y;
        down_y = downPoint.position.y;
        Destroy(upPoint.gameObject);
        Destroy(downPoint.gameObject);
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (faceDown)
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);       //������
            //�߳��޽��ʱ��ͷ
            if (transform.position.y < down_y)
            {
                faceDown = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);        //������
            //�߳��޽��ʱ��ͷ
            if (transform.position.y > up_y)
            {
                faceDown = true;
            }
        }
    }
}
