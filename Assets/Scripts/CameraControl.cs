using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;

    //使摄像机每一帧都跟随Player
    void Update()
    {
        transform.position = new Vector3(player.position.x ,0,-10f);    //X跟随Player,YZ轴固定
    }
}
