using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    private Rigidbody2D rb;

    public Transform upPoint, downPoint;    //移动限界点
    public float speed;
    private float up_y, down_y;             //限界点的y轴

    private bool faceDown = true;           //一开始面向下

    
    protected override void Start()             //重写父类
    {
        base.Start();                           //获取父类的Start
        rb = GetComponent<Rigidbody2D>();
        transform.DetachChildren();         //断绝子类，这样限界点就不会跟着老鹰移动
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
            rb.velocity = new Vector2(rb.velocity.x, -speed);       //向下走
            //走出限界点时掉头
            if (transform.position.y < down_y)
            {
                faceDown = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);        //向上走
            //走出限界点时掉头
            if (transform.position.y > up_y)
            {
                faceDown = true;
            }
        }
    }
}
