using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialog : MonoBehaviour
{
    public GameObject enterDialog;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")            //�������Ҵ���
        {
            enterDialog.SetActive(true);          //�����Ի���
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")            
        {
            enterDialog.SetActive(false);           //�رնԻ���         
        }
    }
}
