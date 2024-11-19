// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [ExecuteInEditMode, DisallowMultipleComponent]
// public class cameraController : MonoBehaviour
// {
//     public GameObject target;
//     public Vector3 offset;
//     private bool FPSmode = false;

//     [SerializeField] private float distance = 10.0f;
//     [SerializeField] private float polarAngle = 45.0f;
//     [SerializeField] private float horizontalAngle = 45.0f;

//     [SerializeField] private float minDistance = 5.0f;
//     [SerializeField] private float maxDistance = 100.0f;
//     [SerializeField] private float minPolarAngle = 20.0f;
//     [SerializeField] private float maxPolarAngle = 75.0f;
//     [SerializeField] private float scrollXSensitivity = 5.0f;
//     [SerializeField] private float scrollYSensitivity = 5.0f;
//     [SerializeField] private float zoomSensitivity = 5.0f;

//     void LateUpdate()
//     {
//         if(Input.GetKeyDown(KeyCode.Alpha0)){
//             FPSmode = !FPSmode;
//         }

//         if(!FPSmode){
//             if(Input.GetscrollButton(0))
//         {
//             updateAngle(Input.GetAxis("scroll X"),Input.GetAxis("scroll Y"));
//         }

//         updateDistance(Input.GetAxis("scroll ScrollWheel"));

//         var lookAtPos = target.transform.position + offset;
//         updatePosition(lookAtPos);
//         transform.LookAt(lookAtPos);
//         }else{
//             Vector3 characterPosition = target.transform.position;

//     // キャラクターの前方ベクトルを取得（キャラクターが向いている方向）


//             Vector3 forwardDirection = target.transform.forward;

//             Quaternion slimeDirection = target.transform.rotation;

//         // キャラクターの前方にオフセットを設定（前方に3ユニット）
//             Vector3 poffset = forwardDirection * 3f;
//             poffset.y += 1.8f;

//             transform.position = characterPosition + poffset;
//             transform.rotation = slimeDirection;
//             // transform.position = target.transform.position;
//             // transform.localEulerAngles = target.transform.localEulerAngles;
//             // transform.position +=new Vector3(
//             //     3 * Mathf.Sin(target.transform.localEulerAngles.y * Mathf.Deg2Rad),
//             //     1.8f,
//             //     3 * Mathf.Cos(target.transform.localEulerAngles.y * Mathf.Deg2Rad)
//             // );
//         }
//     }

//     void updateAngle(float x,  float y)
//     {
//         x = horizontalAngle - x*scrollXSensitivity;
//         horizontalAngle = Mathf.Repeat(x,360);

//         y = polarAngle + y * scrollYSensitivity;
//         polarAngle = Mathf.Clamp(y, minPolarAngle, maxPolarAngle);
//     }

//     void updatePosition(Vector3 lookAtPos)
//     {
//         var dh = horizontalAngle * Mathf.Deg2Rad;
//         var dp = polarAngle * Mathf.Deg2Rad;

//         transform.position = new Vector3(
//             lookAtPos.x + distance * Mathf.Sin(dp) * Mathf.Cos(dh),
//             lookAtPos.y + distance * Mathf.Cos(dp),
//             lookAtPos.z + distance * Mathf.Sin(dp) * Mathf.Sin(dh)
//         );
//     }
//     void updateDistance(float scroll)
//     {
//     distance -= scroll * zoomSensitivity;
//     distance = Mathf.Clamp(distance, minDistance, maxDistance);
//     }

//     void fps(){
        
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ExecuteInEditMode, DisallowMultipleComponent]
public class CameraController : MonoBehaviour
{
    public GameObject target;  // カメラが追従する対象（プレイヤー）
    public Vector3 offset;     // TPSモードのオフセット
    private bool FPSmode = false;  // FPSモードの切り替え

    [SerializeField] private float distance = 20.0f;
    [SerializeField] private float polarAngle = 45.0f;   // 上下の角度
    [SerializeField] private float horizontalAngle = 45.0f;  // 水平角度

    [SerializeField] private float minDistance = 10.0f;
    [SerializeField] private float maxDistance = 50.0f;
    [SerializeField] private float minPolarAngle = 20.0f;
    [SerializeField] private float maxPolarAngle = 75.0f;
    [SerializeField] private float scrollXSensitivity = 3.0f;
    [SerializeField] private float scrollYSensitivity = 3.0f;
    [SerializeField] private float zoomSensitivity = 3.0f;

    [SerializeField] private float cameraSmoothSpeed = 3.0f;  // カメラのスムーズさ

    // game Input
    private GameInput _gameInput;
    private Vector2 _angleInputValue;
    private float _zoomInputValue;

    private void Start()
    {
        // game Input
        _gameInput = new GameInput();

        _gameInput.Camera.Rotation.performed += GetAngle;

        _gameInput.Camera.Zoom.performed += GetZoomValue;

        _gameInput.Enable();
        //
    }

    // game input
    private void OnDestroy()
    {
        _gameInput?.Dispose();
    }

    private void GetAngle(InputAction.CallbackContext context)
    {
        _angleInputValue = context.ReadValue<Vector2>();
    }

    private void GetZoomValue(InputAction.CallbackContext context)
    {
        _zoomInputValue = context.ReadValue<float>();
    }

    void LateUpdate()
    {
        // FPSモードの切り替え
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            FPSmode = !FPSmode;
        }

        // FPSモードのカメラ挙動
        if(FPSmode){
            // FPSモードではキャラクターの目線にカメラを配置
            UpdateFPSCamera();
        }
        else {
            // TPSモードではカメラの角度や距離を調整

            UpdateDistance(_zoomInputValue);

            // プレイヤーを注視する位置を計算
            Vector3 lookAtPos = new Vector3(target.transform.position.x, 0.0f,target.transform.position.z) + offset;
            UpdatePosition(lookAtPos);

            // カメラがプレイヤーを注視
            transform.LookAt(lookAtPos);
        }
    }

    void UpdateFPSCamera()
    {
        // キャラクターの位置と回転を取得
        Vector3 characterPosition = target.transform.position;
        Vector3 forwardDirection = target.transform.forward;
        Quaternion characterRotation = target.transform.rotation;

        // FPSモードのカメラ位置をキャラクターの目線に合わせる
        Vector3 cameraPosition = characterPosition + forwardDirection * 0.5f;
        cameraPosition.y += 1.8f;  // キャラクターの目線の高さに合わせる

        // カメラ位置と回転をキャラクターと同じに設定
        transform.position = Vector3.Lerp(transform.position, cameraPosition, cameraSmoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, characterRotation, cameraSmoothSpeed * Time.deltaTime);
    }

    void UpdatePosition(Vector3 lookAtPos)
    {
        // 極座標を使ってカメラ位置を計算（距離と角度に基づく）
        horizontalAngle = Mathf.Repeat(horizontalAngle - _angleInputValue.x * 0.5f * scrollXSensitivity, 360);
        polarAngle = Mathf.Clamp(polarAngle - _angleInputValue.y * 0.5f * scrollYSensitivity, minPolarAngle, maxPolarAngle);

        var horizontalRadians = horizontalAngle * Mathf.Deg2Rad;
        var polarRadians = polarAngle * Mathf.Deg2Rad;

        // カメラの目標位置を計算
        Vector3 targetPosition = new Vector3(
            lookAtPos.x + distance * Mathf.Sin(polarRadians) * Mathf.Cos(horizontalRadians),
            lookAtPos.y + distance * Mathf.Cos(polarRadians),
            lookAtPos.z + distance * Mathf.Sin(polarRadians) * Mathf.Sin(horizontalRadians)
        );

        // スムーズにカメラを追従させる
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraSmoothSpeed * Time.deltaTime);
    }

    void UpdateDistance(float scroll)
    {
        // マウスホイールでカメラの距離を調整
        distance -= scroll * 0.5f * zoomSensitivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }
}
