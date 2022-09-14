using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;              //场景控制

public class PlayerController : MonoBehaviour
{
    //设为私有不显示在Unity
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource jumpAudio , hurtAudio , cherryAudio;
    public Collider2D coll;                     //链接player的Box Collider2D
    public Collider2D discoll;
    public Transform cellingCheck;
    //地面检测
    public Transform groundCheck;
    //UI
    public Text cherryNum, diamondNum;
    public LayerMask ground;
    
    public float speed, jumpForce;
    public bool isGround, isJump;
    bool jumpPressed;                           //空格键按下与否
    int jumpCount;                              //跳跃次数
    public int cherry = 0, diamond = 0;
    private bool isHurt;                        //默认为false
    
    void Start()
    {
        //为rb和anim赋值
        rb = GetComponent<Rigidbody2D>();
        
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)            //按下空格键且跳跃数大于0
        {
            jumpPressed = true;
        }
    }
    private void FixedUpdate()                                      //FixedUpdate固定帧率刷新，可增加适配性
    {
        
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);     //判断是否在地面上

        if (!isHurt)
        {
            Movement();
        }
        Jump();

        SwitchAnim();
    }

    //移动
    void Movement()
    {
        //定义变量获取X轴左右移动，GetAxis得到-1，0，1值，GetAxisRaw得到-1到1的值
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        //左右转向
        if (horizontalMove != 0)         //移动状态
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);       //3D比较好
        }
        Cround();
    }

    //跳跃
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        //第一段跳
        if (jumpPressed && isGround)     //在地面上跳跃
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpAudio.Play();                   //播放声音
        }
        //第二段跳
        else if (jumpPressed && jumpCount > 0 && !isGround)      //在天上
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpAudio.Play();                   //播放声音
        }
    }

    //转换动画
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));                      //改变running的值，Mathf.Abs()取绝对值
        
        //受伤
        if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetBool("idle", false);
            anim.SetFloat("running", 0);
            //恢复站立
            if (Mathf.Abs(rb.velocity.x) < 0.1f)                
            {
                anim.SetBool("hurt", false);
                anim.SetBool("idle", true);
                isHurt = false;
            }
        }
        else if (isGround)
        {
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
        
        else if (rb.velocity.y > 0)                 //空中往上趋势
        {
            anim.SetBool("jumping", true);
            
            anim.SetBool("falling", false);
        }
        else if (rb.velocity.y < 0)                 //空中往下趋势
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        //不用跳也能触发下落
        if (rb.velocity.y > 0.1f && !isGround)
        {
            anim.SetBool("falling", true);
        }

    }

    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)         //取消碰撞触发OnTrigger事件
    {
        //拾取物品
        if (collision.tag == "Collection")               //如果碰撞体的标签是Collection(樱桃)
        {
            cherryAudio.Play();
            Destroy(collision.gameObject);              //则销毁此物体
            cherry += 1;
            cherryNum.text = cherry.ToString();         //将int型Cherry转为字符型再赋给UI  
        }
        else if (collision.tag == "Collection2")        //钻石
        {
            cherryAudio.Play();                         //由于找不到音效所以声音和樱桃一致
            Destroy(collision.gameObject);              //则销毁此物体
            diamond +=1;
            diamondNum.text = diamond.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;    //关闭音源
            Invoke("Restart", 1f);                      //延迟两秒执行Restart函数
        }
    }


    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)      //碰撞此物体时触发     
    {
        if (collision.gameObject.tag == "Enemy")            //踩中敌人
        {
            //调用敌人父类
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))                            //当Player为下落状态
            {
                enemy.JumpOn();
                //踩中敌人来个跳跃（类似马里奥）
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            //左右碰撞受伤
            else if(transform.position.x < collision.gameObject.transform.position.x)   //Player在敌人左侧
            {
                rb.velocity = new Vector2(-5, rb.velocity.y + 5);          //向左弹
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)   //Player在敌人右侧
            {
                rb.velocity = new Vector2(5, rb.velocity.y + 5);          //向右弹
                hurtAudio.Play();
                isHurt = true;
            }
        }
    }

    //下蹲
    void Cround()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))         //检测头顶是否与地板元素接触
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouch", true);
                discoll.enabled = false;                            //关闭头部碰撞体
            }
            else
            {
                anim.SetBool("crouch", false);
                discoll.enabled = true;
            }
        }
    }

    //重启
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     //重新加载当前运行的场景

    }

}
