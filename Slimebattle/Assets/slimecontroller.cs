// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerController : MonoBehaviour
// {
//     private Rigidbody _rigidbody;
//     private Transform _transform;
//     private Animator _animator;
//     //
//     private float _horizontal;
//     private float _vertical;
//     private Vector3 _velocity;
//     [SerializeField] private float _speed = 10.0f;
//     private Vector3 _aim;
//     private Quaternion _playerRotation;
//     [SerializeField] private float jumpPower = 20.0f;
//     private bool isJumping = false;
//     private float chargeGage = 0.0f;
//     private bool isCharging = false;
//     private bool isCooling = false;
//     [SerializeField] private float AttackPower = 5.0f;

//     [SerializeField] private float AttackTime = 0.5f;


//     // game Input
//     private GameInput _gameInput;
//     private Vector2 _moveInputValue;


//     void Start()
//     {
//         Application.targetFrameRate = 60; 
//         _rigidbody = GetComponent<Rigidbody>();
//         _transform = GetComponent<Transform>();
//         _animator = GetComponentInChildren<Animator>();
//         _playerRotation = _transform.rotation;

//         // game input
//         _gameInput = new GameInput();

//         _gameInput.Player.Move.started += OnMove;
//         _gameInput.Player.Move.performed += OnMove;
//         _gameInput.Player.Move.canceled += OnMove;
//         _gameInput.Player.Jump.started += OnJump;
//         _gameInput.Player.Attack.started += OnCharge;
//         _gameInput.Player.Attack.canceled += OnAttack;

//         _gameInput.Enable();
//         //
//     }

//     private void OnDestroy()
//     {
//         _gameInput?.Dispose();
//     }

//     private void OnMove(InputAction.CallbackContext context)
//     {
//         _moveInputValue = context.ReadValue<Vector2>();
//     }

//     public void OnJump(InputAction.CallbackContext context)
//     {
//         if(!isJumping && !isCharging){
//             _rigidbody.velocity = Vector3.up * jumpPower;
//             isJumping = true;
//         }
//     }

//     private void OnCharge(InputAction.CallbackContext context)
//     {
//         isCharging = true;
//         Debug.Log("チャージ開始");
//     }

//     private void OnAttack(InputAction.CallbackContext context)
//     {
//         if(!isCooling){
//             _rigidbody.AddForce(_transform.forward * chargeGage*AttackPower,ForceMode.Impulse);
//             isCooling = true;
//             Invoke("CancelCharge", chargeGage * AttackTime);
//         }
//     }

//     private void CancelCharge()
//     {
//         chargeGage = 0.0f;
//         isCharging = false;
//         isCooling = false;
//     }
//     void Update()
//     {
//         var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
//         _velocity = _horizontalRotation * new Vector3(_moveInputValue.x, 0.0f, _moveInputValue.y);
//         _aim = _velocity.normalized; // _aimは移動方向
        
//         // アニメーションの状態更新
//         if (_velocity.magnitude > 0.1f) {
//             _animator.SetBool("walk", true);
//         } else {
//             _animator.SetBool("walk", false);
//         }

//         if (_aim.magnitude > 0.1f)
//         {
//             _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
//         }
//         // 回転をMoveRotationを使って行う
//         _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, _playerRotation, 600 * Time.fixedDeltaTime));
//         // 位置をMovePositionを使って移動
//         if(!isCharging){
//             Vector3 targetPosition = _rigidbody.position + _velocity * _speed * Time.fixedDeltaTime;
//             _rigidbody.MovePosition(targetPosition);
//         }
//         else if(!isCooling){
//             chargeGage = chargeGage + 0.1f;
//             chargeGage = Mathf.Clamp(chargeGage, 0.0f, 10.0f);
//             Debug.Log(chargeGage);
//         }
        
//     }


//     private void OnCollisionEnter(Collision collision){
//         if(collision.gameObject.CompareTag("Floor")){
//             isJumping = false;
//         }else if(collision.gameObject.CompareTag("Dead")){
//             _rigidbody.MovePosition(new Vector3(0f,0f,0f));
//             Debug.Log("dead");
//         }
//     }
// }


// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerController : MonoBehaviour
// {
//     private Rigidbody _rigidbody;
//     private Transform _transform;
//     private Animator _animator;
//     //
//     private float _horizontal;
//     private float _vertical;
//     private Vector3 _velocity;
//     [SerializeField] private float _speed = 10.0f;
//     private Vector3 _aim;
//     private Quaternion _playerRotation;
//     [SerializeField] private float jumpPower = 20.0f;
//     private bool isJumping = false;
//     private float chargeGage = 0.0f;
//     private bool isCharging = false;
//     private bool isCooling = false;
//     [SerializeField] private float AttackPower = 5.0f;
//     [SerializeField] private float AttackTime = 0.5f;

//     // プレイヤーごとの入力を管理するためのPlayerInput
//     private PlayerInput _playerInput;
//     private Vector2 _moveInputValue;

//     void Start()
//     {
//         Application.targetFrameRate = 60; 
//         _rigidbody = GetComponent<Rigidbody>();
//         _transform = GetComponent<Transform>();
//         _animator = GetComponentInChildren<Animator>();
//         _playerRotation = _transform.rotation;

//         // PlayerInputを使って、各プレイヤーの入力を個別に管理
//         _playerInput = GetComponent<PlayerInput>();

//         _playerInput.actions["Move"].performed += OnMove;
//         _playerInput.actions["Move"].canceled += OnMove;
//         _playerInput.actions["Jump"].performed += OnJump;
//         _playerInput.actions["Attack"].started += OnCharge;
//         _playerInput.actions["Attack"].canceled += OnAttack;
//     }

//     private void OnDestroy()
//     {
//         _playerInput.actions["Move"].performed -= OnMove;
//         _playerInput.actions["Move"].canceled -= OnMove;
//         _playerInput.actions["Jump"].performed -= OnJump;
//         _playerInput.actions["Attack"].started -= OnCharge;
//         _playerInput.actions["Attack"].canceled -= OnAttack;
//     }

//     public void OnMove(InputAction.CallbackContext context)
//     {
//         _moveInputValue = context.ReadValue<Vector2>();
//     }

//     public void OnJump(InputAction.CallbackContext context)
//     {
//         if (!isJumping && !isCharging)
//         {
//             _rigidbody.velocity = Vector3.up * jumpPower;
//             isJumping = true;
//         }
//     }

//     private void OnCharge(InputAction.CallbackContext context)
//     {
//         isCharging = true;
//         Debug.Log("チャージ開始");
//     }

//     private void OnAttack(InputAction.CallbackContext context)
//     {
//         if (!isCooling)
//         {
//             _rigidbody.AddForce(_transform.forward * chargeGage * AttackPower, ForceMode.Impulse);
//             isCooling = true;
//             Invoke("CancelCharge", chargeGage * AttackTime);
//         }
//     }

//     private void CancelCharge()
//     {
//         chargeGage = 0.0f;
//         isCharging = false;
//         isCooling = false;
//     }

//     void Update()
//     {
//         var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
//         _velocity = _horizontalRotation * new Vector3(_moveInputValue.x, 0.0f, _moveInputValue.y);
//         _aim = _velocity.normalized;

//         // アニメーションの状態更新
//         if (_velocity.magnitude > 0.1f)
//         {
//             _animator.SetBool("walk", true);
//         }
//         else
//         {
//             _animator.SetBool("walk", false);
//         }

//         if (_aim.magnitude > 0.1f)
//         {
//             _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
//         }

//         // キャラクターの回転をMoveRotationで行う
//         _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, _playerRotation, 600 * Time.fixedDeltaTime));

//         // キャラクターの移動
//         if (!isCharging)
//         {
//             Vector3 targetPosition = _rigidbody.position + _velocity * _speed * Time.fixedDeltaTime;
//             _rigidbody.MovePosition(targetPosition);
//         }
//         else if (!isCooling)
//         {
//             chargeGage = chargeGage + 0.1f;
//             chargeGage = Mathf.Clamp(chargeGage, 0.0f, 10.0f);
//             Debug.Log(chargeGage);
//         }
//     }

//     private void OnCollisionEnter(Collision collision)
//     {
//         if (collision.gameObject.CompareTag("Floor"))
//         {
//             isJumping = false;
//         }
//         else if (collision.gameObject.CompareTag("Dead"))
//         {
//             _rigidbody.MovePosition(new Vector3(0f, 0f, 0f));
//             Debug.Log("dead");
//         }
//     }
// }
/////////////////22222222
///

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private MeshRenderer _meshRenderer;
    //
    private float _horizontal;
    private float _vertical;
    private Vector3 _velocity;
    public float _speed = 10.0f;
    private Vector3 _aim;
    private Quaternion _playerRotation;
    public float jumpPower = 20.0f;
    public float jumpPenalty = 1.0f;
    private float jumpHold = 1.0f;
    private bool isJumping = false;
    private float chargeGage = 3.0f;
    public float IniChargeGage = 1.0f;
    private bool isCharging = false;
    private bool isCooling = false;
    private float HoldValue = 1.0f;
    public float AttackingHold = 1.0f;
    public float AttackPower = 5.0f;
    public float AttackTime = 0.5f;

    public float ImpulsePower = 1.0f;
    public float ChargeSpeed = 1.0f;
    [HideInInspector] public int LastAttackedPID = -1;

    // プレイヤーごとの入力を管理するためのPlayerInput
    private PlayerInput _playerInput;
    private int playerID;
    private Vector2 _moveInputValue;

    private Material objectMaterial;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponentInChildren<Animator>();
        
        _playerRotation = _transform.rotation;
        
        // _meshRenderer = GetComponentInChildren<MeshRenderer>();
        // objectMaterial = _meshRenderer.material;
        // SetTransparency(0); // 半透明


        // PlayerInputを使って、各プレイヤーの入力を個別に管理
        _playerInput = GetComponent<PlayerInput>();
        playerID = _playerInput.playerIndex;

        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        _playerInput.actions["Jump"].performed += OnJump;
        _playerInput.actions["Attack"].started += OnCharge;
        _playerInput.actions["Attack"].canceled += OnAttack;
    }

    // public void SetTransparency(float alpha)
    // {
    //     if (objectMaterial != null)
    //     {
    //         // マテリアルの色を取得
    //         Color color = objectMaterial.color;

    //         // アルファ値を変更
    //         color.a = Mathf.Clamp01(alpha); // 0～1に制限
    //         objectMaterial.color = color;

    //         // 透明度が反映されるように設定
    //         objectMaterial.SetFloat("_Mode", 3); // フェードモードに設定
    //         objectMaterial.renderQueue = 3000;   // レンダーキューを透明用に設定
    //     }
    // }
    public void SetTransparency(float alpha)
    {
        if (objectMaterial != null)
        {
            // マテリアルの色を取得
            Color color = objectMaterial.color;

            // アルファ値を設定
            color.a = Mathf.Clamp01(alpha); // 0～1の範囲に制限
            objectMaterial.color = color;
        }
    }
    
    private void SetMaterialToTransparent(Material material)
    {
        if (material != null)
        {
            // Standard Shader を使用している場合、透明に設定
            material.SetFloat("_Mode", 3); // 3 = Transparent モード
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000; // 透明用のレンダリングキュー
        }
    }

    private void OnDestroy()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
        _playerInput.actions["Jump"].performed -= OnJump;
        _playerInput.actions["Attack"].started -= OnCharge;
        _playerInput.actions["Attack"].canceled -= OnAttack;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInputValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isJumping && !isCharging)
        {
            _rigidbody.velocity = Vector3.up * jumpPower;
            jumpHold = jumpPenalty;
            isJumping = true;
        }
    }

    private void OnCharge(InputAction.CallbackContext context)
    {
        isCharging = true;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (!isCooling)
        {
            _rigidbody.AddForce(_transform.forward * chargeGage * AttackPower, ForceMode.Impulse);
            isCooling = true;
            HoldValue = AttackingHold;
            Invoke("CancelCharge", chargeGage * AttackTime);
        }
    }

    private void CancelCharge()
    {
        chargeGage = IniChargeGage;
        isCharging = false;
        isCooling = false;
        HoldValue = 1.0f;
    }

    public float GetHoldValue(){
        return HoldValue;
    }
    public float GetJumpHold()
    {
        return jumpHold;
    }

    public int GetLastAttackedPID()
    {
        return LastAttackedPID;
    }

    void FixedUpdate()
    {
        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        _velocity = _horizontalRotation * new Vector3(_moveInputValue.x, 0.0f, _moveInputValue.y);
        _aim = _velocity.normalized;

        // アニメーションの状態更新
        if (_velocity.magnitude > 0.1f)
        {
            _animator.SetBool("walk", true);
        }
        else
        {
            _animator.SetBool("walk", false);
        }

        if (_aim.magnitude > 0.1f)
        {
            _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
        }

        // キャラクターの回転をMoveRotationで行う
        _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, _playerRotation, 600 * Time.fixedDeltaTime));

        // キャラクターの移動とcチャージ時間の計測
        if (!isCharging)
        {
            Vector3 targetPosition = _rigidbody.position + _velocity * _speed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(targetPosition);
        }
        else if (!isCooling)
        {
            chargeGage = chargeGage + 0.1f * ChargeSpeed;
            chargeGage = Mathf.Clamp(chargeGage, 1.0f, 10.0f);
        }

        // if(_transform.position.y<-20.0f){
        //     if(LastAttackedPID != -1){
        //          DataHolder.playerValues[LastAttackedPID].playerScore++;
        //          LastAttackedPID = -1;
        //     }
        //     _rigidbody.MovePosition(new Vector3(Random.Range(-10.0f,10.0f), 0f, Random.Range(-10.0f,10.0f)));
        //     _rigidbody.velocity = Vector3.zero;
        // }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJumping = false;
            jumpHold = 1.0f;
        }else if (collision.gameObject.CompareTag("Player") )
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            PlayerController targetScript = collision.gameObject.GetComponent<PlayerController>();
            float targetHoldValue = targetScript.GetHoldValue();
            float targetJumpHold = targetScript.GetJumpHold();
            targetScript.LastAttackedPID = playerID; //衝突した相手にIDを送る

            Vector3 forceDirection = (collision.transform.position - transform.position).normalized;
            rb.AddForce(forceDirection * _rigidbody.velocity.magnitude * ImpulsePower * targetHoldValue * targetJumpHold, ForceMode.Impulse);
        }
        // else if (collision.gameObject.CompareTag("Dead"))
        // {
            
            
        //     if(LastAttackedPID != -1){
        //          DataHolder.playerValues[LastAttackedPID].playerScore++;
        //          LastAttackedPID = -1;
        //     }
        //     _rigidbody.MovePosition(new Vector3(Random.Range(-10.0f,10.0f), 0f, Random.Range(-10.0f,10.0f)));
        //     _rigidbody.velocity = Vector3.zero;
        // }
        
    }
}
