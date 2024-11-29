using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
    public GameObject TitleCanvas;
    public GameObject OptionCanvas;
    public RectTransform arrow;
    public Vector3 offsets;
    //タイトル画面のボタン
    public Button PlayButton;
    public Button OptionButton;
    public Button QuitButton;
    
    public GameObject TimerOption;
    public Button OptionTimer;
        public Button TimerPlus;
        public Button TimerMinus;
        public Button TimerSave;
        public Text Timer;
    public GameObject GameModeOption;
    public Button OptionMode;
        public Button ModeLeft;
        public Button ModeRight;
        public Button ModeSave;
        public Text Mode;
        private bool StockMode = false;
            public GameObject StockOption;
            public Button StockPlus;
            public Button StockMinus;
            public Text StockNum;
            private int StockValue;
    public Button OptionBack;
    
    
    
    private float TimerValue;

    public AudioSource audioSource;
    public AudioClip StartEF;
    public AudioClip Decide;
    public Image fadeImage;  // フェードアウト用のImage（黒画面）
    private bool isTitle = true;
    private Button[] OptionButtons; // 配列として定義

    void Start()
    {
        TitleCanvas.SetActive(true);
        OptionCanvas.SetActive(false);
        TimerOption.SetActive(false);
        GameModeOption.SetActive(false);

        GameTimer.timerReset();

        TimerValue = GameTimer.DefaultTime;
        Timer.text = Timer.text =Mathf.FloorToInt(GameTimer.DefaultTime / 60.0f) + "：" + Mathf.FloorToInt((GameTimer.DefaultTime) % 60.0f);
        
        StockMode = GameModeFrag.isStock;

        StockValue = StockGame.StockCount;

        StockNum.text = StockValue.ToString();

        ModeTextChange();

        PlayButton.onClick.AddListener(OnPlayButtonPressed);
        OptionButton.onClick.AddListener(OnOptionButtonPressed);
        QuitButton.onClick.AddListener(OnQuitButtonPressed);

        OptionTimer.onClick.AddListener(OnOptionTimerPressed);
            TimerPlus.onClick.AddListener(OnTimerPlusPressed);
            TimerMinus.onClick.AddListener(OnTimerMinusPressed);
            TimerSave.onClick.AddListener(OnTimerSavePressed);
        OptionMode.onClick.AddListener(OnOptionModePressed);
            ModeLeft.onClick.AddListener(OnModeLeftPressed);
            ModeRight.onClick.AddListener(OnModeRightPressed);
                StockPlus.onClick.AddListener(OnStockPlusPressed);
                StockMinus.onClick.AddListener(OnStockMinusPressed);
            ModeSave.onClick.AddListener(OnModeSavePressed);
        OptionBack.onClick.AddListener(OnOptionBackPressed);
        fadeImage.gameObject.SetActive(false);  // 初期状態では非表示

        //OptionButtonをリスト化する(Button型)
        OptionButtons = new Button[] {OptionTimer, OptionMode, OptionBack};
        foreach (Button button in OptionButtons){
            AddSelectEvent(button);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        if(selectedButton != null && selectedButton.GetComponent<Button>() != null && isTitle){
            RectTransform selectedButtonRect = selectedButton.GetComponent<RectTransform>();

            arrow.position = selectedButtonRect.position + new Vector3(-selectedButtonRect.sizeDelta.x/10,0,0) + offsets;
        }
    }

    public void OnPlayButtonPressed()
    {
        audioSource.PlayOneShot(StartEF);
        if(Gamepad.all.Count == 1){
            GameMode.isMulti = false;
        }
        Invoke("LoadSceneAsync", 0.5f);
    }

    public void OnOptionButtonPressed()
    {
        audioSource.PlayOneShot(Decide);
        isTitle = false;
        TitleCanvas.SetActive(false);
        OptionCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(OptionTimer.gameObject);        
    }

    //オプションのUI操作
    public void OnOptionTimerPressed()
    {
        audioSource.PlayOneShot(Decide);

        CanvasGroup canvasGroup = TimerOption.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        EventSystem.current.SetSelectedGameObject(TimerPlus.gameObject);
    }

    public void OnTimerPlusPressed()
    {
        audioSource.PlayOneShot(Decide);
        TimerValue += 10.0f;
        TimerValue = Mathf.Clamp(TimerValue, 10.0f, 99*60.0f + 60.0f);
        Timer.text = Mathf.FloorToInt(TimerValue / 60.0f) + "：" + Mathf.FloorToInt((TimerValue) % 60.0f);
    }

    public void OnTimerMinusPressed()
    {
        audioSource.PlayOneShot(Decide);
        TimerValue -= 10.0f;
        TimerValue = Mathf.Clamp(TimerValue, 10.0f, 99*60.0f + 60.0f);
        Timer.text = Mathf.FloorToInt(TimerValue / 60.0f) + "：" + Mathf.FloorToInt((TimerValue) % 60.0f);
    }

    public void OnTimerSavePressed()
    {
        audioSource.PlayOneShot(Decide);
        GameTimer.DefaultTime = TimerValue;
        Timer.text = Mathf.FloorToInt(GameTimer.DefaultTime / 60.0f) + "：" + Mathf.FloorToInt((GameTimer.DefaultTime) % 60.0f);
        EventSystem.current.SetSelectedGameObject(OptionTimer.gameObject);
    }

    public void OnOptionModePressed()
    {
        audioSource.PlayOneShot(Decide);
        CanvasGroup canvasGroup = GameModeOption.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        EventSystem.current.SetSelectedGameObject(ModeLeft.gameObject);
    }

    public void OnModeLeftPressed()
    {
        audioSource.PlayOneShot(Decide);
        ModeChange();
        ModeTextChange();
        if(StockMode){
            CanvasGroup canvasGroup = StockOption.GetComponent<CanvasGroup>();
            NavigateDown(ModeLeft, StockMinus);
            NavigateDown(ModeRight, StockPlus);
            NavigateUp(ModeSave, StockMinus);
            canvasGroup.alpha = 1.0f;

        }else{
            CanvasGroup canvasGroup = StockOption.GetComponent<CanvasGroup>();
            NavigateDown(ModeLeft, ModeSave);
            NavigateDown(ModeRight, ModeSave);
            NavigateUp(ModeSave, ModeLeft);
            canvasGroup.alpha = 0.5f;
        }
    }

    public void OnModeRightPressed()
    {
        audioSource.PlayOneShot(Decide);
        ModeChange();
        ModeTextChange();
        if(StockMode){
            CanvasGroup canvasGroup = StockOption.GetComponent<CanvasGroup>();
            NavigateDown(ModeLeft, StockMinus);
            NavigateDown(ModeRight, StockPlus);
            NavigateUp(ModeSave, StockMinus);
            canvasGroup.alpha = 1.0f;

        }else{
            CanvasGroup canvasGroup = StockOption.GetComponent<CanvasGroup>();
            NavigateDown(ModeLeft, ModeSave);
            NavigateDown(ModeRight, ModeSave);
            NavigateUp(ModeSave, ModeLeft);
            canvasGroup.alpha = 0.5f;
        }
    }
        public void NavigateDown(Button button, Button target)
        {
            Navigation navigation = button.navigation;
            navigation.selectOnDown = target;
            button.navigation = navigation;
        }

        public void NavigateUp(Button button, Button target)
        {
            Navigation navigation = button.navigation;
            navigation.selectOnUp = target;
            button.navigation = navigation;
        }

        public void OnStockPlusPressed()
        {
            audioSource.PlayOneShot(Decide);
            StockValue += 1;
            StockValue = Mathf.Clamp(StockValue, 1, 99);
            StockNum.text = StockValue.ToString();
        }

        public void OnStockMinusPressed()
        {
            audioSource.PlayOneShot(Decide);
            StockValue -= 1;
            StockValue = Mathf.Clamp(StockValue, 1, 99);
            StockNum.text = StockValue.ToString();
        }

    public void OnModeSavePressed()
    {
        audioSource.PlayOneShot(Decide);
        GameModeFrag.isStock = StockMode;
        StockGame.StockCount = StockValue;
        EventSystem.current.SetSelectedGameObject(OptionMode.gameObject);
    }

    private void ModeChange()
    {
        StockMode = !StockMode;
    }

    private void ModeTextChange()
    {
        if(StockMode){
            Mode.text = "ストック制";
        }else{
            Mode.text = "タイム制";
        }
    }

    public void OnOptionBackPressed()
    {
        audioSource.PlayOneShot(Decide);
        isTitle = true;
        TitleCanvas.SetActive(true);
        OptionCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
    }
    //
    public void LoadSceneAsync()
    {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        fadeImage.gameObject.SetActive(true);
        
        // フェードアウト処理（ここでは単純に表示する例）
        fadeImage.color = new Color(0, 0, 0, 1);  // 不透明に設定

        yield return new WaitForSeconds(0.5f);  // フェードアウト待機時間

        // 非同期でシーンを読み込む
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        asyncLoad.allowSceneActivation = false;  // シーンのアクティベーションを保留

        // シーンが読み込まれるまで待機
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)  // 90％以上読み込まれたら
            {
                // 新しいカメラの確認が済んだらシーンをアクティベート
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void OnQuitButtonPressed()
    {
        audioSource.PlayOneShot(Decide);
        Application.Quit();
        Debug.Log("Quit Game");
    }

    // 汎用的な関数: 任意のボタンにSelectイベントを追加
    private void AddSelectEvent(Button button)
    {
        // GameObject から EventTrigger コンポーネントを取得または追加
        EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = button.gameObject.AddComponent<EventTrigger>();
        }

        // Select イベントを作成
        EventTrigger.Entry selectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select // Select イベントを指定
        };

        // イベントにコールバック関数を追加
        selectEntry.callback.AddListener((eventData) => OnButtonSelected(button));
        eventTrigger.triggers.Add(selectEntry); // イベントをトリガーに追加
    }

    public void OnButtonSelected(Button button)
    {
    if (button.name == "Timer")
    {
        //TimerOptionキャンパスを半透明で表示
        CanvasGroup canvasGroup = TimerOption.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
        GameModeOption.SetActive(false);
        StockOption.SetActive(false);
        TimerOption.SetActive(true);
    }
    else if (button.name == "GameMode")
    {
        //GameModeOptionキャンパスを半透明で表示
        CanvasGroup canvasGroup = GameModeOption.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.5f;
        CanvasGroup canvasGroup_a = StockOption.GetComponent<CanvasGroup>();
        canvasGroup_a.alpha = 0.5f;
        TimerOption.SetActive(false);
        GameModeOption.SetActive(true);
        StockOption.SetActive(true);
    }
    else if (button.name == "Cancel")
    {
        //Optionキャンパスを非表示
        TimerOption.SetActive(false);
        GameModeOption.SetActive(false);
        StockOption.SetActive(false);
    }
    }
}