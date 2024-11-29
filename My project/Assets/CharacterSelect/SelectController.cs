// using UnityEngine;
// using UnityEngine.UI;

// public class CharacterSelection : MonoBehaviour
// {
//     public GameObject[] characters;  // キャラクターの3Dモデル（複数）
//     public Button Decide;
//     public Button Right;
//     public Button Left;

//     private GameObject currentCharacter; // 現在表示されているキャラクター
//     public Vector3 offsets;
//     private int SelectIndex = 0;
//     private RectTransform decideRect;

//     void Start()
//     {
//         //ボタンの登録
//         Right.onClick.AddListener(OnRightPressed);
//         Left.onClick.AddListener(OnLeftPressed);

//         //決定ボタンの位置の取得
//         decideRect = Decide.GetComponent<RectTransform>();
//         Vector3 screenPosition = decideRect.position;
//         Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10f));
//         Debug.Log(worldPosition);

//         currentCharacter = characters[SelectIndex];
//         currentCharacter.SetActive(true);
//         currentCharacter.transform.position = worldPosition + offsets;
//     }

//     void Update()
//     {
//         currentCharacter.transform.Rotate(0,0.2f,0);
//     }

//     // キャラクター選択時に呼び出される関数
//     void OnRightPressed()
//     {
//         SelectIndex++;
//         if(SelectIndex >= characters.Length){
//             SelectIndex = 0;
//         }
//         // 現在表示されているキャラクターを非表示にする
//         if (currentCharacter != null)
//         {
//             currentCharacter.SetActive(false);
//         }

//         // 新しいキャラクターを表示
//         currentCharacter = characters[SelectIndex];
//         currentCharacter.SetActive(true);
//         currentCharacter.transform.position = decideRect.position + offsets;
//     }

//     void OnLeftPressed()
//     {
//         SelectIndex--;
//         if(SelectIndex < 0){
//             SelectIndex = characters.Length - 1;
//         }
//         // 現在表示されているキャラクターを非表示にする
//         if (currentCharacter != null)
//         {
//             currentCharacter.SetActive(false);
//         }

//         // 新しいキャラクターを表示
//         currentCharacter = characters[SelectIndex];
//         currentCharacter.SetActive(true);
//         currentCharacter.transform.position = decideRect.position + offsets;
//     }
// }

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CharacterSelection : MonoBehaviour
{
    public GameObject[] characterPrefabs;  // キャラクターのプレハブ（複数）
    public Button Decide;
    public Button Right;
    public Button Left;
    public Button Cancel;
    public Transform characterPositionTransform; // キャラクターを配置する位置
    public Vector3 offsets;
    public float prefabsSize ;

    private GameObject currentCharacter; // 現在表示されているキャラクター
    private int SelectIndex = 0;
    private PlayerInput playerInput;
    private int playerIndex;
    private Vector3 characterPosition;

    void Start()
    {
        // ボタンの登録
        Right.onClick.AddListener(OnRightPressed);
        Left.onClick.AddListener(OnLeftPressed);
        Decide.onClick.AddListener(OnDecidePressed);
        Cancel.onClick.AddListener(OnCancelPressed);

        playerInput = GetComponentInParent<PlayerInput>();
        playerIndex = playerInput.playerIndex;
        if(!GameMode.isMulti){
            characterPosition = characterPositionTransform.position + offsets + new Vector3(0,0,33.0f);
        }else{
            characterPosition = characterPositionTransform.position + offsets;
        }
        
        // 初期のキャラクターを生成して表示
        InstantiateCharacter(SelectIndex);
    }

    void Update()
    {
        if (currentCharacter != null)
        {
            currentCharacter.transform.Rotate(0, 0.5f, 0);
        }
    }

    // キャラクターを生成して表示する関数
    void InstantiateCharacter(int index)
    {
        // 既存のキャラクターがあれば削除
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        // 新しいキャラクターをプレハブからインスタンス化
        currentCharacter = Instantiate(characterPrefabs[index], characterPosition, new Quaternion(0,180,0,0));
        currentCharacter.transform.localScale = new Vector3(prefabsSize, prefabsSize, prefabsSize);
    }

    // 右ボタンが押された時の処理
    void OnRightPressed()
    {
        SelectIndex++;
        if(SelectIndex >= characterPrefabs.Length){
            SelectIndex = 0;
        }

        // キャラクターを生成して表示
        InstantiateCharacter(SelectIndex);
    }

    // 左ボタンが押された時の処理
    void OnLeftPressed()
    {
        SelectIndex--;
        if(SelectIndex < 0){
            SelectIndex = characterPrefabs.Length - 1;
        }

        // キャラクターを生成して表示
        InstantiateCharacter(SelectIndex);
    }

    void OnDecidePressed()
    {
        if(SelectIndex == characterPrefabs.Length - 1){
            int randomNumber = Random.Range(0, characterPrefabs.Length-1);
            DataHolder.playerValues[playerIndex].selectedCharacterIndex = randomNumber;
        }else{
            DataHolder.playerValues[playerIndex].selectedCharacterIndex = SelectIndex;
        }
        
        DataHolder.playerValues[playerIndex].isConfirmed = true;
        Decide.gameObject.SetActive(false);
        Cancel.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Cancel.gameObject);
    }

    void OnCancelPressed()
    {
        DataHolder.playerValues[playerIndex].isConfirmed = false;
        Cancel.gameObject.SetActive(false);
        Decide.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(Decide.gameObject);
    }

    public int GetSelectIndex(){
        return SelectIndex;
    }
}

