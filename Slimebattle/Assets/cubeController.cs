using UnityEngine;
using UnityEngine.InputSystem;

public class cubeController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Move入力に対応するメソッド
    public void OnMoves(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // プレイヤーごとに個別の移動処理
        Vector3 move = new Vector3(_moveInput.x, 0, _moveInput.y);
        _rigidbody.AddForce(move * 100f);
    }
}
