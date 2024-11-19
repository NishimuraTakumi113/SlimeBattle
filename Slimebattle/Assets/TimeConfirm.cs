using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeConfirm : MonoBehaviour
{
    public Text Countdown;//カウントダウン表示用
    public Text Explain;//説明表示用
    private float fadeDuration = 1.0f;//フェード時間
    private float fadeSpeed;
    private Color fadeColor;

    void Start()
    {
        GameTimer.timerReset();
        GameTimer.CountDownStart();


        //初期値の色を保存し，フェード速度を計算
        fadeColor = Countdown.color;
        fadeColor.a = 1.0f; 
        fadeSpeed = fadeColor.a / fadeDuration;
        Time.timeScale = 0;
        isPaused.isPausing = true;
    }

    void Update()
    {
        if(SuddenDeath.isSuddenDeath){
            GameTimer.CountdownTime = 4.0f;
            GameTimer.timeRemaining = 30.0f;
            isPaused.isPausing = true;
            MatchStarted.isMatchStarted = false;
            Countdown.gameObject.SetActive(true);
            Countdown.text = "Sudden Death！";
            fadeColor.a = 1.0f;
            Countdown.color = fadeColor;
            Time.timeScale = 0; // Move this line before setting SuddenDeath.isSuddenDeath to false
            SuddenDeath.isSuddenDeath = false;
        }

        if(!MatchStarted.isMatchStarted){
        //３，２，１とカウントダウンし，切り替わるときはフェードする処理(数字が切り替わったら，α値を１に戻す)
        GameTimer.CountdownTime -= Time.unscaledDeltaTime;
        
        if (GameTimer.CountdownTime <= 3.0f && GameTimer.CountdownTime > 2.0f)
        {
            Countdown.text = "3";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.CountdownTime <= 2.0f && GameTimer.CountdownTime > 1.0f)
        {
            if (Countdown.text != "2")
            {
            fadeColor.a = 1.0f; // α値を１に戻す
            }
            Countdown.text = "2";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.CountdownTime <= 1.0f && GameTimer.CountdownTime > 0.0f)
        {
            if (Countdown.text != "1")
            {
            fadeColor.a = 1.0f;
            }
            Countdown.text = "1";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.CountdownTime <= 0.0f && GameTimer.CountdownTime > -1.0f)
        {
            if (Countdown.text != "Start！")
            {
            Time.timeScale = 1.0f;
            isPaused.isPausing = false;
            fadeColor.a = 1.0f; 
            }
            Countdown.text = "Start！";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.CountdownTime <= -1.0f)
        {
            //カウントダウンが終わったら，カウントダウンを非表示にする
            Countdown.gameObject.SetActive(false);
            MatchStarted.isMatchStarted = true;
        }
        }

        if(GameTimer.timeRemaining <= GameTimer.DefaultTime*2/3.0f){
            Explain.gameObject.SetActive(false);
        }

        //GameTimer.timeRemainingが5秒から5,4,3,2,1,Game Setとカウントダウンを開始(上記のフェードアウトと同様の処理)
        if (GameTimer.timeRemaining <= 5.0f && GameTimer.timeRemaining > 4.0f)
        {
            if (Countdown.text != "5")
            {
            fadeColor.a = 1.0f;
            }
            Countdown.gameObject.SetActive(true);
            Countdown.text = "5";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.timeRemaining <= 4.0f && GameTimer.timeRemaining > 3.0f)
        {
            if (Countdown.text != "4")
            {
            fadeColor.a = 1.0f;
            }
            Countdown.text = "4";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.timeRemaining <= 3.0f && GameTimer.timeRemaining > 2.0f)
        {
            if (Countdown.text != "3")
            {
            fadeColor.a = 1.0f;
            }
            Countdown.text = "3";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.timeRemaining <= 2.0f && GameTimer.timeRemaining > 1.0f)
        {
            if (Countdown.text != "2")
            {
            fadeColor.a = 1.0f;
            }
            Countdown.text = "2";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.timeRemaining <= 1.0f && GameTimer.timeRemaining > 0.0f)
        {
            if (Countdown.text != "1")
            {
            fadeColor.a = 1.0f;
            }
            Countdown.text = "1";
            fadeColor.a -= fadeSpeed * Time.unscaledDeltaTime;
            Countdown.color = fadeColor;
        }
        else if (GameTimer.timeRemaining <= 0.0f && GameTimer.timeRemaining > -1.5f)
        {
            GameSet();
        }else if(GameTimer.timeRemaining <= -1.5f){
            //カウントダウンが終わったら，カウントダウンを非表示にする
            Countdown.gameObject.SetActive(false);
        }
    }
    private void GameSet()
    {
        Countdown.gameObject.SetActive(true);
        fadeColor.a = 1.0f;
        Countdown.text = "Game Set！";
        Countdown.color = fadeColor;
    }
}
