using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector2 rotationSpeed = new Vector2(-180, 180); //カメラの回転スピード（初期値は1秒間に180度）
    [SerializeField] float cameraDistance = 2f;                      // 平均的なカメラの距離
    [SerializeField] float lowAngleDistanceRatio = 0.5f;             // 低い時の距離の比率
    [SerializeField] float highAngleDistanceRatio = 3f;              // 高い時の距離の比率
    [SerializeField] float inputMappingCurve = 5f;
    [SerializeField] float minCameraAngle = -45f;                     //X軸回転の下限
    [SerializeField] float maxCameraAngle = 75f;　　　　　　　　　　  //Y軸回転の上限
    CinemachineVirtualCamera vCam = null;      　　                  //仮想カメラの参照
    Cinemachine3rdPersonFollow follow = null;　　　                  //仮想カメラの追跡対象の参照
    Vector2 cameraRotationInput = Vector2.zero;　　               　 //カメラのRotationの参照

    void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        if(vCam != null)
        {
            follow = vCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        }
    }
    void FixedUpdate()
    {
        if(vCam != null)
        {
            Transform target = vCam.Follow;
            if (target != null)
            {
                //対象の回転を取得
                Vector3 targetEulerAngles = target.rotation.eulerAngles;

                float curve = Mathf.Max(1, inputMappingCurve);
                Vector2 speed = new Vector2(
                    Mathf.Pow(Mathf.Abs(cameraRotationInput.x), curve) * Mathf.Sign(cameraRotationInput.x),
                    Mathf.Pow(Mathf.Abs(cameraRotationInput.y), curve) * Mathf.Sign(cameraRotationInput.y));
                //targetEulerAngles.y += cameraRotationInput.x * rotationSpeed.y * Time.fixedDeltaTime;
                //X軸、Y軸の回転を変える
                targetEulerAngles.y += cameraRotationInput.x * rotationSpeed.y * Time.fixedDeltaTime;
                targetEulerAngles.x += cameraRotationInput.y * rotationSpeed.x * Time.fixedDeltaTime;
                //target.rotation.eulerAnglesは0～360の角度を返す。これを-180～180に変える
                if (targetEulerAngles.x > 180f)
                {
                    targetEulerAngles.x -= 360f;
                }
                // この状態で値を制限する。
                // 「Clamp」は一つ目の引数を2つ目と3つ目の引数の間に制限するメソッド
                targetEulerAngles.x = Mathf.Clamp(targetEulerAngles.x, minCameraAngle, maxCameraAngle);

                // オイラー角度をクオータニオンに変換して追跡ターゲットの回転を変える
                target.transform.rotation = Quaternion.Euler(targetEulerAngles);
                //カメラの位置調整（カメラが低い位置に下がるほどキャラクターに近づいて、高くなるほど遠ざかる。）
                if(follow)
                {
                    float anglePhase = (targetEulerAngles.x - minCameraAngle) / (maxCameraAngle - minCameraAngle);
                    float lowCameraDistance = cameraDistance * lowAngleDistanceRatio;
                    float highCameraDistance = cameraDistance * highAngleDistanceRatio;
                    follow.CameraDistance = lowCameraDistance + (highCameraDistance - lowCameraDistance) * anglePhase;

                }
            }
        }
    }
    void Look(Vector2 input)
    {
        cameraRotationInput = input;
    }
    void OnLook(InputValue inputValue)
    {
        Look(inputValue.Get<Vector2>());
    }
}
