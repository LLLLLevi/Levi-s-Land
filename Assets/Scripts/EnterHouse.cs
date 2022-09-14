using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))         //当按下E键
        {
            SceneManager.LoadScene("Menu");     //加载菜单
        }
    }
}
