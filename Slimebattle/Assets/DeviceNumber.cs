using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceNumber : MonoBehaviour
{
    private PlayerInput playerInput;

    void Start()
    {
        // PlayerInputコンポーネントを取得
        playerInput = GetComponent<PlayerInput>();

        // プレイヤーに割り当てられたデバイスを取得
    }

    void Update()
    {
        foreach (var device in playerInput.devices)
        {
            Debug.Log($"デバイス名: {device.displayName}, ID: {device.deviceId}");
        }
    }
}
