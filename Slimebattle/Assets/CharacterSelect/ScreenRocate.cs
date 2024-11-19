
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ScreenRocate : MonoBehaviour
{
    public GameObject   screenPrefab;  //　選択画面のプレハブ
    // private PlayerInputManager[] playerInputManagers; // 複数のPlayerInputManager
    private Camera objectCamera;
    public InputActionAsset inputActions;

    private InputAction StartAction;              // 各プレイヤーのGameStartアクション
    private int gamepadCount;
    private Vector3[] ScreenLocate = new Vector3[]{
        new Vector3(0, 0, 0),
        new Vector3(0, 1000.0f, 0),
        new Vector3(0, -1000.0f, 0),
        new Vector3(0, 2000.0f, 0)
    };

    void Start()
    {
        Application.targetFrameRate = 60;

        var playerActionMap = inputActions.FindActionMap("UIcontrols");
        StartAction = playerActionMap.FindAction("GameStart");
        StartAction.performed += GameStart;
        StartAction.Enable();

        //繋がっているplayerの数
        gamepadCount = Gamepad.all.Count;
        DataHolder.InitializePlayerData(gamepadCount);
        if(gamepadCount >= 2){
            GameMode.isMulti = true;
        }        

        for(int i=0; i < gamepadCount; i++){
            JoinPlayer(i); 
        }
    }

    // プレイヤーを参加させる
    public void JoinPlayer(int playerIndex)
    {
            GameObject playerObject = Instantiate(
            screenPrefab,
            ScreenLocate[playerIndex],
            Quaternion.identity
            );

                if(gamepadCount == 1){
                objectCamera = playerObject.GetComponentInChildren<Camera>();
                objectCamera.rect = new Rect(0,0,1.0f,1.0f);
                }else if(gamepadCount == 2){
                if(playerIndex == 0)
                {
                    objectCamera = playerObject.GetComponentInChildren<Camera>();
                    objectCamera.rect = new Rect(0,0.5f,1.0f,0.5f);
                }
                else if(playerIndex == 1)
                {
                    objectCamera = playerObject.GetComponentInChildren<Camera>();
                    objectCamera.rect = new Rect(0,0,1.0f,0.5f);
                }
            }else{
                if(playerIndex == 0)
                {
                    objectCamera = playerObject.GetComponentInChildren<Camera>();
                    objectCamera.rect = new Rect(0,0.5f,0.5f,0.5f);
                }
                else if(playerIndex == 1)
                {
                    objectCamera = playerObject.GetComponentInChildren<Camera>();
                    objectCamera.rect = new Rect(0.5f,0.5f,0.5f,0.5f);
                }else if(playerIndex == 2)
                {
                    objectCamera = playerObject.GetComponentInChildren<Camera>();
                    objectCamera.rect = new Rect(0,0,0.5f,0.5f);
                }else if(playerIndex == 3){
                    objectCamera = playerObject.GetComponentInChildren<Camera>();
                    objectCamera.rect = new Rect(0.5f,0,0.5f,0.5f);
                }
            }
    }
    private void GameStart(InputAction.CallbackContext context){
        bool AllPlayerReady = DataHolder.AllPlayerReady();

        if(AllPlayerReady){
            SceneManager.LoadScene(2);
        }
    }

}
