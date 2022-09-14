using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //protected���޸������ʹ��
    protected Animator anim;
    protected AudioSource deathAudio;

    //�鷽��
    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    //����������������Ч
    public void Death()
    {
        GetComponent<Animator>().enabled = false;       //ͣ����ײ��
        Destroy(gameObject);
    }

    public void JumpOn()
    {
        anim.SetTrigger("death");
        deathAudio.Play();          //����
    }
}
