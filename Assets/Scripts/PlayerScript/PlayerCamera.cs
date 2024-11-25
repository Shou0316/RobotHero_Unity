using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    const float CameraPosY = 4.0f;
    const float Stage1Angle = 180.0f;
    const float Stage2Angle = 120.0f;
    const float Stage3Angle = 140.0f;

    //ターゲットとするオブジェクト
    [SerializeField] GameObject TargetObj;
    
    //回転速度
    private float MoveSpeed = 100.0f;
    private float AltiSpeed = 10.0f;

    //回転制限(上下)
    private float RotaLimitMin = 12;
    private float RotaLimitMax = 18;

    public float Angle;
    public float Altitude;
    [SerializeField]Vector3 TargetPos, CameraPos;
    Quaternion TargetRota, CameraRota;

    // Start is called before the first frame update
    private void Start()
    {
        // TargetObj = GameObject.FindWithTag("player");
        CameraRota = this.transform.rotation;
    }

    void OnEnable()
    {
        TargetPos = TargetObj.transform.position;
        Angle = Stage2Angle;
    }

    private void Update()
    {
        TargetPos = TargetObj.transform.position;

        CameraRota.y = 0.0f;
        CameraPos = this.transform.position;

        float r = 5.5f;
        this.transform.position = new Vector3(r * Mathf.Sin(Angle * Mathf.Deg2Rad), CameraPosY + Altitude, r * Mathf.Cos(Angle * Mathf.Deg2Rad));
        this.transform.position += TargetPos;
        this.transform.rotation = CameraRota;

        Angle += Input.GetAxis("Mouse X") * 1.5f;

        Altitude += Input.GetAxis("Mouse Y");

        this.transform.LookAt(TargetPos);
    }
}