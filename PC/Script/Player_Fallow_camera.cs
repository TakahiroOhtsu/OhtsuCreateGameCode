using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Fallow_camera : MonoBehaviour
{
    public GameObject targetObj;
    public float CameraMoveSpeed = 200f;
    Vector3 targetPos;


    void Start()
    {

        targetPos = targetObj.transform.position;
    }

    void Update()
    {
        // targetの移動量分、自分（カメラ）も移動する
        transform.position += targetObj.transform.position - targetPos;
        targetPos = targetObj.transform.position;


        // マウスの移動量
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");
        // targetの位置のY軸を中心に、回転（公転）する
        transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * CameraMoveSpeed);
        // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
        mouseInputY = mouseInputY * -1; //y軸反転
        transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * CameraMoveSpeed);

    }
}
