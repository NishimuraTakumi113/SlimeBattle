using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerEnter : MonoBehaviour
{
    public GameObject[] playerPrefabs;  // プレイヤー用のキャラPrefab
    private PlayerInputManager playerInputManager;
    private Vector3[] SpawnLocate = new Vector3[] 
    {
        new Vector3(0, 0, -20.0f),
        new Vector3(0, 0, 20.0f),
        new Vector3(20.0f,0,0),
        new Vector3(-20.0f,0,0)
    };
    public GameObject canvas; //プレイヤー番号を表示するためのCanvas
    public Text[] NumberText; //プレイやー番号が書かれたテキスト
    private List<GameObject> characters = new List<GameObject>(); // 生成されたキャラクターと対応するラベルを保持するリスト
    [SerializeField] private Vector3 offsets;
    private PlayerInput[] players;

    void Start()
    {
        // PlayerInputManagerを取得
        playerInputManager = GetComponent<PlayerInputManager>();
        for(int i=0; i < 4; i++){
            NumberText[i].gameObject.SetActive(false);
        }
        for(int i=0; i < DataHolder.playerValues.Count; i++){
            LocatePlayer(i,DataHolder.playerValues[i].selectedCharacterIndex);
            NumberText[i].gameObject.SetActive(true);
        }

        if(GameModeFrag.isStock){
            for(int i=0; i < DataHolder.playerValues.Count; i++){
                DataHolder.playerValues[i].playerScore = StockGame.StockCount;
            }
        }else{
            for(int i=0; i < DataHolder.playerValues.Count; i++){
                DataHolder.playerValues[i].playerScore = 0;
            }
        }
        
    }

    // プレイヤーを手動で参加させるメソッド
    private void LocatePlayer(int playerIndex, int prefabNumber)
    {
            GameObject playerObject = Instantiate(
            playerPrefabs[prefabNumber],  // playerIndexに応じてプレイヤープレハブを正しく選択
            SpawnLocate[playerIndex],
            Quaternion.identity
            );
            
            characters.Add(playerObject);
    }

     void Update()
     {
        for(int i=0; i < 4; i++){
            NumberText[i].gameObject.SetActive(false);
        }
        players = FindObjectsOfType<PlayerInput>();
        foreach (PlayerInput player in players)
        {
            if(player.playerIndex >= 0){
            NumberText[player.playerIndex].gameObject.SetActive(true);
            UpdateLabelPosition(NumberText[player.playerIndex], player.gameObject);
            }
        }
        // for (int i = 0; i < characters.Count; i++)
        // {
        //     if (characters[i] != null)
        //     {
        //         UpdateLabelPosition(NumberText[i], characters[i]);
        //     }
        // }
     }

     private void UpdateLabelPosition(Text playerNum, GameObject character)
     {
        Camera mainCamera = Camera.main;

        // キャラクターの頭上にラベルを表示（スクリーン座標に変換）
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(character.transform.position + offsets);

        RectTransform rectTransform = playerNum.GetComponent<RectTransform>();

        // CanvasのRender ModeがScreen Space - CameraまたはWorld Spaceのときに使う
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPosition, mainCamera, out anchoredPosition);

        // RectTransformの位置を更新
        rectTransform.anchoredPosition = anchoredPosition;
     }
}