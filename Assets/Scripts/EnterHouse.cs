using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))         //������E��
        {
            SceneManager.LoadScene("Menu");     //���ز˵�
        }
    }
}
