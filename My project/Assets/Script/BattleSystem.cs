using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BattleSystem : MonoBehaviour
{
    public Text Message;
    public Text MenuIntro;
    private List<int> Winner;
    private bool WinnerDecided = false;
    private bool isSuddenDeath = false;
    private PlayerInput[] players;
    private Vector3[] SpawnLocate = new Vector3[] 
    {
        new Vector3(0, 0, -20.0f),
        new Vector3(0, 0, 20.0f),
        new Vector3(20.0f,0,0),
        new Vector3(-20.0f,0,0)
    };
    void Start()
    {
        Message.gameObject.SetActive(false);
        MenuIntro.gameObject.SetActive(false);
        // シーン内のすべての PlayerInput コンポーネントを取得
        players = FindObjectsOfType<PlayerInput>();
    }
    void FixedUpdate()
    {
        //勝者を決める
        
            if(GameModeFrag.isStock){
                Winner = DataHolder.StockWinner();
                if(Winner.Count == 1 && !WinnerDecided){
                    WinnerDecided = true;
                    GameTimer.timeRemaining = 0;
                    Invoke("WinnerAnnounce",2.0f);
                }else if(Winner.Count > 1 && !isSuddenDeath && GameTimer.timeRemaining <= 0){
                    Winner = DataHolder.TimeWinner();
                    if(Winner.Count == 1 && !WinnerDecided){
                        WinnerDecided = true;
                        GameTimer.timeRemaining = 0;
                        Invoke("WinnerAnnounce",2.0f);
                    }else if(Winner.Count > 1 && !isSuddenDeath){
                        Invoke("SuddenlyDeath",2.0f);
                        isSuddenDeath = true;
                    }
                }
            }else{
                if(GameTimer.timeRemaining <= 0 || isSuddenDeath){
                Winner = DataHolder.TimeWinner();
                if(Winner.Count == 1 && !WinnerDecided){
                if(isSuddenDeath){
                    WinnerDecided = true;
                    GameTimer.timeRemaining = 0;
                    Invoke("WinnerAnnounce",2.0f);
                }
                WinnerDecided = true;
                //2秒後勝者をアナウンス
                Invoke("WinnerAnnounce",2.0f);
            }else if(Winner.Count > 1 && !isSuddenDeath){
                Invoke("SuddenlyDeath",2.0f);
                isSuddenDeath = true;
            }
            }
            
        }
            //一定の低さにプレイヤーが来たら，そのプレイヤーをリスポーン
            foreach (PlayerInput player in players)
            {
                if(player.transform.position.y < -20.0f){
                    PlayerController playerScript = player.GetComponent<PlayerController>();
                    if(GameTimer.timeRemaining > 0){
                    if(GameModeFrag.isStock){
                        DataHolder.playerValues[player.playerIndex].playerScore--;
                    }else{
                        if(playerScript.LastAttackedPID != -1){
                            if(!WinnerDecided){
                                DataHolder.playerValues[playerScript.LastAttackedPID].playerScore++;
                            }
                            playerScript.LastAttackedPID = -1;
                        }
                    }
                    }
                    if(GameModeFrag.isStock){
                        if(DataHolder.playerValues[player.playerIndex].playerScore <= 0){
                            player.gameObject.SetActive(false);
                        }
                    }
                    player.transform.position = new Vector3(Random.Range(-10.0f,10.0f), 0f, Random.Range(-10.0f,10.0f));
                    player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }
            }
    }

    void SuddenlyDeath()
    {
        // Winner に入っているプレイヤー番号に対応するオブジェクトを探す
        foreach (PlayerInput player in players)
        {
            // Winner に入っているプレイヤーを初期地にリスポーン
            if (Winner.Contains(player.playerIndex))
            {
                //スクリプトを取得して，powerを高くする．
                PlayerController playerScript = player.GetComponent<PlayerController>();
                playerScript.ImpulsePower = 0.8f;

                player.transform.position = SpawnLocate[player.playerIndex];
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }else{
                player.gameObject.SetActive(false);
            }
        }

        if(GameModeFrag.isStock){
            foreach(int i in Winner){
                DataHolder.playerValues[i].playerScore = 1;
            }
        }
            SuddenDeath.isSuddenDeath = true;
    }
    void WinnerAnnounce()
    {
        Message.text = (Winner[0]+1) + "P　Win！";
                Message.gameObject.SetActive(true);
                MenuIntro.gameObject.SetActive(true);
                Time.timeScale = 0;
    }
}