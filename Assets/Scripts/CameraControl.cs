using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;

    //ʹ�����ÿһ֡������Player
    void Update()
    {
        transform.position = new Vector3(player.position.x ,0,-10f);    //X����Player,YZ��̶�
    }
}
