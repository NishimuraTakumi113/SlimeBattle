using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    public EventSystem playerEventSystem;
    public Canvas playerCanvas;

    void Start()
    {
        // プレイヤーごとのUI操作用の初期設定
        if (playerEventSystem != null)
        {
            EventSystem.current = playerEventSystem;
        }
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 navigationInput = context.ReadValue<Vector2>();
        // プレイヤーごとのUI操作をここで処理
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // プレイヤーがUIボタンを決定する処理
        }
    }
}
