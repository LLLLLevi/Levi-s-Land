using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator anim;
    private Collider2D coll;
    public LayerMask ground;
    public Transform leftPoint, rightPoint; //移动限界点
    public float speed , jumpForce;
    private float left_x , right_x;          //限界点的x轴

    private bool faceLeft = true;           //一开始面向左

   
    protected override void Start()                  //重写父类
    {
        base.Start();                       //获取父类的Start
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        transform.DetachChildren();         //断绝子类，这样左右限界点就不会跟着青蛙移动
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
        if(faceLeft)        //面向左
        {
            if (coll.IsTouchingLayers(ground))                      //如果接触地面
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-speed, jumpForce);      //向左跳
            }
            //走出限界点时掉头
            if(transform.position.x < left_x)                
            {
                transform.localScale = new Vector3(-1, 1, 1);
                rb.velocity = new Vector2(speed, jumpForce);            //防止他跳出边界
                faceLeft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))          //如果接触地面
            {
                anim.SetBool("jumping", true);
                rb.velocity = new Vector2(speed, jumpForce);      //向右跳
            }
            //走出限界点时掉头
            if (transform.position.x > right_x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                rb.velocity = new Vector2(-speed, jumpForce);                     //防止他跳出边界
                faceLeft = true;
            }
        }
    }
    //动画转换
    void SwitchAnim()
    {
        if(anim.GetBool("jumping"))             //跳跃状态
        {
            if(rb.velocity.y < 0.1)            //下落
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
