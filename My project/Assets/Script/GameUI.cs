using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text[] playerScores;
    public Text Timer;
    // private bool isUpdateEnabled = true; // このフラグでUpdateの処理を制御
    private PlayerInput[] players;
    void Start()
    {
        Timer.enabled = true;
    }

    void Update()
    {
        //playerinputが存在するプレイヤーのみスコアを表示
        players = FindObjectsOfType<PlayerInput>();
        //playerinputにないものは非表示
        for(int i=0; i < 4; i++){
            playerScores[i].enabled = false;
        }
        
        foreach (PlayerInput player in players)
        {
            playerScores[player.playerIndex].enabled = true;
        }
        // for(int i=0; i < 4; i++){
        //     if(players.playerIndex.Contains(i)){
        //         playerScores[i].enabled = true;
        //     }else{
        //         playerScores[i].enabled = false;
        //     }
        // }

        // if (!isUpdateEnabled)
        // {
        //     return; // フラグがfalseのときはUpdateをスキップ
        // }


        foreach (PlayerInput player in players)
        {
            playerScores[player.playerIndex].text = (player.playerIndex+1) + "P： " + DataHolder.playerValues[player.playerIndex].playerScore;
        }

        // for(int i=0; i < 4; i++){
        //     if(players.playerIndex.Contains(i)){
        //         playerScores[i].text = (i+1) + "P： " + DataHolder.playerValues[i].playerScore;
        //     }
        // }

        GameTimer.timeRemaining -= Time.deltaTime;
        
        Timer.text =Mathf.FloorToInt((GameTimer.timeRemaining+1) / 60.0f) + "：" + Mathf.FloorToInt((GameTimer.timeRemaining+1) % 60.0f);

        if(GameTimer.timeRemaining <= 0){
            //0:00秒にする
            Timer.text = "0：0";
            // TimeEnded();
        }
    }

    // void TimeEnded()
    // {
    //     isUpdateEnabled = false; // Updateを無効にする
    // }
}
