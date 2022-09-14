using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //protected仅限父子类间使用
    protected Animator anim;
    protected AudioSource deathAudio;

    //虚方法
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    //敌人死亡动画与音效
    public void Death()
    {
        GetComponent<Animator>().enabled = false;       //停用碰撞体
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
        deathAudio.Play();          //播放
    }
}
