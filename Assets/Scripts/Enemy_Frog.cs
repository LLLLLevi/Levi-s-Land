using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftPoint, rightPoint; //�ƶ��޽��
    public float speed , jumpForce;
    private float left_x , right_x;          //�޽���x��

    private bool faceLeft = true;           //һ��ʼ������

   
    protected override void Start()                  //��д����
    {
        base.Start();                       //��ȡ�����Start
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();         //�Ͼ����࣬���������޽��Ͳ�����������ƶ�
        left_x = leftPoint.position.x;
        right_x = rightPoint.position.x;
        Destroy(leftPoint.gameObject);
        Destroy(rightPoint.gameObject);
    }

    void Update()
    {
        SwitchAnim();
    }
    
    void Movement()
    {
        if(faceLeft)        //������
        {
            if (coll.IsTouchingLayers(ground))                      //����Ӵ�����
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);      //������
            }
            //�߳��޽��ʱ��ͷ
            if(transform.position.x < left_x)                
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(speed, jumpForce);            //��ֹ�������߽�
                faceLeft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))          //����Ӵ�����
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);      //������
            }
            //�߳��޽��ʱ��ͷ
            if (transform.position.x > right_x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(-speed, jumpForce);                     //��ֹ�������߽�
                faceLeft = true;
            }
        }
    }
    //����ת��
    void SwitchAnim()
    {
        if(anim.GetBool("jumping"))             //��Ծ״̬
        {
            if(rb.velocity.y < 0.1)            //����
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        if(coll.IsTouchingLayers(ground) && anim.GetBool("falling"))
        {
            anim.SetBool("falling", false);
        }
    }
    
}
