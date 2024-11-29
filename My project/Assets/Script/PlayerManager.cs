// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerSpawner : MonoBehaviour
// {
//     public GameObject playerPrefab1;  // プレイヤー1用のプレハブ
//     public GameObject playerPrefab2;  // プレイヤー2用のプレハブ
//     private PlayerInputManager playerInputManager;

//     void Start()
//     {
//         // PlayerInputManagerのインスタンスを取得
//         playerInputManager = PlayerInputManager.instance;
//     }
//     void Update()
//     {
//         // 手動でプレイヤーを参加させる（例えば、スペースキーでトリガー）
//         if (Keyboard.current.spaceKey.wasPressedThisFrame)
//         {
//             // デフォルトの設定でプレイヤーを参加させる
//             PlayerInput playerInput = playerInputManager.JoinPlayer();
//         }
//     }

//     void OnEnable()
//     {
//         // プレイヤーが参加した時のイベントリスナーを登録
//         PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
//     }

//     void OnDisable()
//     {
//         // イベントリスナーを解除
//         PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
//     }

//     // プレイヤーが参加したときに呼び出される
//     void OnPlayerJoined(PlayerInput playerInput)
//     {
//         // 参加したプレイヤーのインデックスを取得
//         int playerIndex = playerInput.playerIndex;

//         // 参加したプレイヤーに対応するPrefabを生成
//         GameObject playerObject = Instantiate(
//             playerIndex == 1 ? playerPrefab1 : playerPrefab2,  // プレイヤーインデックスが1ならplayerPrefab1、それ以外はplayerPrefab2
//             Vector3.zero,  // 生成位置は (0, 0, 0)
//             Quaternion.identity // 回転はデフォルト
//         );

//         // 生成したオブジェクトの子としてPlayerInputを割り当てる
//         playerInput.transform.SetParent(playerObject.transform);

//         // 必要に応じて、追加の初期化やカスタマイズをここに記述
//         Debug.Log($"Player {playerIndex} joined with {playerInput.currentControlScheme}");
//     }
// }


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab1;  // プレイヤー1用のキャラPrefab
    public GameObject playerPrefab2;  // プレイヤー2用のキャラPrefab
    private PlayerInputManager playerInputManager;
    private bool P1active = false;
    private bool P2active = false;

    void Start()
    {
        // PlayerInputManagerを取得
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    // プレイヤーを手動で参加させるメソッド
    public void JoinPlayer(int playerIndex)
    {
        InputDevice inputDevice = null;

        // プレイヤーごとのデバイス設定
        if (playerIndex == 0 && Gamepad.all.Count > 0)
        {
            inputDevice = Gamepad.all[0];  // プレイヤー1には最初のGamepadを割り当て
        }
        else if (playerIndex == 1 && Gamepad.all.Count > 1)
        {
            inputDevice = Gamepad.all[1];  // プレイヤー2には2番目のGamepadを割り当て
        }

        if (inputDevice != null)
        {
            // プレイヤーを参加させる

            GameObject playerObject = Instantiate(
            playerIndex == 0 ? playerPrefab1 : playerPrefab2,  // playerIndexに応じてプレイヤープレハブを正しく選択
            Vector3.zero,
            Quaternion.identity
            );


        }
        else
        {
            Debug.LogError("無効なプレイヤーインデックスまたはデバイスが見つかりませんでした。");
        }
    }
    void Update()
{  
    var gamepads = Gamepad.all;
    // プレイヤー1がコントローラーのNorthボタンを押したときに参加
    if (gamepads[0].buttonNorth.wasPressedThisFrame && !P1active)
    {
        P1active = true;
        JoinPlayer(0);  // プレイヤー1を参加させる
    }

    // プレイヤー2が別のコントローラーでNorthボタンを押したときに参加
  
    if (gamepads[1].buttonNorth.wasPressedThisFrame && P1active && !P2active)  
    {
        P2active = true;
        JoinPlayer(1);  // プレイヤー2を参加させる
    }
}

}


//2ban


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerManager : MonoBehaviour
// {
//     public GameObject playerPrefab1;  // プレイヤー1用のキャラPrefab
//     public GameObject playerPrefab2;  // プレイヤー2用のキャラPrefab
//     private PlayerInputManager playerInputManager;

//     void Start()
//     {
//         // PlayerInputManagerを取得
//         playerInputManager = GetComponent<PlayerInputManager>();
//     }

//     // プレイヤーを手動で参加させるメソッド
//     public void JoinPlayer(int playerIndex)
// {
//     InputDevice inputDevice = null;

//     if (playerIndex == 1 && Gamepad.all.Count > 0)
//     {
//         inputDevice = Gamepad.all[0];  // プレイヤー1には最初のGamepadを割り当て
//     }
//     else if (playerIndex == 2 && Gamepad.all.Count > 1)
//     {
//         inputDevice = Gamepad.all[1];  // プレイヤー2には2番目のGamepadを割り当て
//     }

//     if (inputDevice != null)
//     {
//         // プレイヤーを参加させる
//         PlayerInput playerInput = playerInputManager.JoinPlayer(playerIndex, -1, null, inputDevice);

//         // 参加したプレイヤーに対応するPrefabを割り当てる
//         GameObject playerObject = Instantiate(playerIndex == 1 ? playerPrefab1 : playerPrefab2, Vector3.zero, Quaternion.identity);

//         // 生成されたPrefabにPlayerInputをセットする
//         playerInput.transform.SetParent(playerObject.transform);
//         playerInput.transform.localPosition = Vector3.zero;

//         // PlayerInputの対象オブジェクトをPrefabのRootに設定する
//         playerInput.gameObject.transform.SetParent(null);  // 親から外して、直接操作できるようにする
//         playerObject.GetComponent<PlayerController>().enabled = true;  // PlayerControllerを有効にする
//     }
//     else
//     {
//         Debug.LogError("無効なプレイヤーインデックスまたはデバイスが見つかりませんでした。");
//     }
// }
// }




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerManager : MonoBehaviour
// {
//     // プレイヤーが操作するキャラクターのPrefab
//     public GameObject playerPrefab;

//     // プレイヤー入室時にキャラクターを生成する
//     public void OnPlayerJoined(PlayerInput playerInput)
//     {
//         // プレイヤーに個別のキャラクターを生成して割り当てる
//         GameObject playerObject = Instantiate(playerPrefab, GetSpawnPosition(), Quaternion.identity);

//         // PlayerInputを生成したキャラクターに割り当てる
//         playerInput.transform.SetParent(playerObject.transform);
//         playerInput.transform.localPosition = Vector3.zero;

//         // プレイヤーごとに割り当てられたデバイスを表示
//         Debug.Log($"プレイヤー#{playerInput.playerIndex}が参加しました！ デバイス: {playerInput.currentControlScheme}");
//     }

//     // プレイヤー退室時の処理
//     public void OnPlayerLeft(PlayerInput playerInput)
//     {
//         Debug.Log($"プレイヤー#{playerInput.playerIndex}が退室しました！");
//     }

//     // キャラクターのスポーン位置を決定する関数
//     private Vector3 GetSpawnPosition()
//     {
//         // ここでスポーン位置を決定します（例: ランダム、指定された座標など）
//         // 現在はシンプルにランダムな位置を設定しています
//         return new Vector3(Random.Range(-5.0f, 5.0f), 0.0f, Random.Range(-5.0f, 5.0f));
//     }
// }
