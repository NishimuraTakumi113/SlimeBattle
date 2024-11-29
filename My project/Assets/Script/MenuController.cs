using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


public class MenuController : MonoBehaviour
{
    public Canvas canvas;
    public Button Continue;
    public Button Rematch;
    public Button ToTitle;
    public EventSystem eventSystem;
    private InputAction MenuAction; 
    private bool Done = false;
  
    void Start()
    {
        Continue.onClick.AddListener(OnContinuePressed);
        Rematch.onClick.AddListener(OnRematchPressed);
        ToTitle.onClick.AddListener(OnToTitlePressed);
        
        
        canvas.gameObject.SetActive(false);

        
        // var playerActionMap = inputActions.FindActionMap("UIcontrolscontrols");
        // MenuAction = playerActionMap.FindAction("Menu");
        // MenuAction.performed += Paused;
        // MenuAction.Enable();
    }

    void Update()
    {
        if(!Done){
            var allPlayers = PlayerInput.all;
            foreach (PlayerInput player in allPlayers)
            {
                var menuAction = player.actions.FindAction("Menu");
                menuAction.performed += Paused;
                menuAction.Enable();
            }
            Done = true;
        }
        

    }
    void OnContinuePressed()
    {
        Time.timeScale = 1.0f;
        canvas.gameObject.SetActive(false);
        isPaused.isPausing = false;
    }

    void OnRematchPressed()
    {
        Time.timeScale = 1.0f;
        FragReset.Reset();
        isPaused.isPausing = false;
        var allPlayers = PlayerInput.all;
            foreach (PlayerInput player in allPlayers)
            {
                var menuAction = player.actions.FindAction("Menu");
                menuAction.Disable();
            }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnToTitlePressed()
    {
        Time.timeScale = 1.0f;
        FragReset.Reset();
        isPaused.isPausing = false;
        var allPlayers = PlayerInput.all;
            foreach (PlayerInput player in allPlayers)
            {
                var menuAction = player.actions.FindAction("Menu");
                menuAction.Disable();
            }
        SceneManager.LoadScene(0);
    }

    public void Paused(InputAction.CallbackContext context)
    {
        if(!isPaused.isPausing){
         // アクションが発生したデバイス
        var device = context.control.device;
        var allPlayers = PlayerInput.all;
        // すべてのプレイヤーを確認して、どのプレイヤーがこのデバイスを使用しているか調べる
        foreach (PlayerInput player in PlayerInput.all)
        {
            foreach (var playerDevice in player.devices)
            {
                if (playerDevice == device)
                {   
                    var inputModule = eventSystem.GetComponent<InputSystemUIInputModule>();
                    inputModule.actionsAsset = player.actions; // プレイヤーのアクションマップを設定
                    inputModule.move = InputActionReference.Create(player.actions.FindAction("UIcontrols/Navigate"));
                    inputModule.submit = InputActionReference.Create(player.actions.FindAction("UIcontrols/Submit"));
                    inputModule.cancel = InputActionReference.Create(player.actions.FindAction("UIcontrols/Cancel"));
                    break;
                }
            }   
        }
        
            Time.timeScale = 0;
            canvas.gameObject.SetActive(true);
            isPaused.isPausing = true;
        }
    }
}