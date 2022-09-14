using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;              //��������

public class PlayerController : MonoBehaviour
{
    //��Ϊ˽�в���ʾ��Unity
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource jumpAudio , hurtAudio , cherryAudio;
    public Collider2D coll;                     //����player��Box Collider2D
    public Collider2D discoll;
    public Transform cellingCheck;
    //������
    public Transform groundCheck;
    //UI
    public Text cherryNum, diamondNum;
    public LayerMask ground;
    
    public float speed, jumpForce;
    public bool isGround, isJump;
    bool jumpPressed;                           //�ո���������
    int jumpCount;                              //��Ծ����
    public int cherry = 0, diamond = 0;
    private bool isHurt;                        //Ĭ��Ϊfalse
    
    void Start()
    {
        //Ϊrb��anim��ֵ
        rb = GetComponent<Rigidbody2D>();
        
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)            //���¿ո������Ծ������0
        {
            jumpPressed = true;
        }
    }
    private void FixedUpdate()                                      //FixedUpdate�̶�֡��ˢ�£�������������
    {
        
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);     //�ж��Ƿ��ڵ�����

        if (!isHurt)
        {
            Movement();
        }
        Jump();

        SwitchAnim();
    }

    //�ƶ�
    void Movement()
    {
        //���������ȡX�������ƶ���GetAxis�õ�-1��0��1ֵ��GetAxisRaw�õ�-1��1��ֵ
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        //����ת��
        if (horizontalMove != 0)         //�ƶ�״̬
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);       //3D�ȽϺ�
        }
        Cround();
    }

    //��Ծ
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        //��һ����
        if (jumpPressed && isGround)     //�ڵ�������Ծ
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpAudio.Play();                   //��������
        }
        //�ڶ�����
        else if (jumpPressed && jumpCount > 0 && !isGround)      //������
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
            jumpAudio.Play();                   //��������
        }
    }

    //ת������
    void SwitchAnim()
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));                      //�ı�running��ֵ��Mathf.Abs()ȡ����ֵ
        
        //����
        if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetBool("idle", false);
            anim.SetFloat("running", 0);
            //�ָ�վ��
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
        
        else if (rb.velocity.y > 0)                 //������������
        {
            anim.SetBool("jumping", true);
            
            anim.SetBool("falling", false);
        }
        else if (rb.velocity.y < 0)                 //������������
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        //������Ҳ�ܴ�������
        if (rb.velocity.y > 0.1f && !isGround)
        {
            anim.SetBool("falling", true);
        }

    }

    //��ײ������
    private void OnTriggerEnter2D(Collider2D collision)         //ȡ����ײ����OnTrigger�¼�
    {
        //ʰȡ��Ʒ
        if (collision.tag == "Collection")               //�����ײ��ı�ǩ��Collection(ӣ��)
        {
            cherryAudio.Play();
            Destroy(collision.gameObject);              //�����ٴ�����
            cherry += 1;
            cherryNum.text = cherry.ToString();         //��int��CherryתΪ�ַ����ٸ���UI  
        }
        else if (collision.tag == "Collection2")        //��ʯ
        {
            cherryAudio.Play();                         //�����Ҳ�����Ч����������ӣ��һ��
            Destroy(collision.gameObject);              //�����ٴ�����
            diamond +=1;
            diamondNum.text = diamond.ToString();
        }

        if (collision.tag == "DeadLine")
        {
            GetComponent<AudioSource>().enabled = false;    //�ر���Դ
            Invoke("Restart", 1f);                      //�ӳ�����ִ��Restart����
        }
    }


    //�������
    private void OnCollisionEnter2D(Collision2D collision)      //��ײ������ʱ����     
    {
        if (collision.gameObject.tag == "Enemy")            //���е���
        {
            //���õ��˸���
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))                            //��PlayerΪ����״̬
            {
                enemy.JumpOn();
                //���е���������Ծ����������£�
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            //������ײ����
            else if(transform.position.x < collision.gameObject.transform.position.x)   //Player�ڵ������
            {
                rb.velocity = new Vector2(-5, rb.velocity.y + 5);          //����
                hurtAudio.Play();
                isHurt = true;
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)   //Player�ڵ����Ҳ�
            {
                rb.velocity = new Vector2(5, rb.velocity.y + 5);          //���ҵ�
                hurtAudio.Play();
                isHurt = true;
            }
        }
    }

    //�¶�
    void Cround()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))         //���ͷ���Ƿ���ذ�Ԫ�ؽӴ�
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouch", true);
                discoll.enabled = false;                            //�ر�ͷ����ײ��
            }
            else
            {
                anim.SetBool("crouch", false);
                discoll.enabled = true;
            }
        }
    }

    //����
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);     //���¼��ص�ǰ���еĳ���

    }

}
