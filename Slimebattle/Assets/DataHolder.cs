using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerSelectionData
{
    public int playerIndex;  // プレイヤーのインデックス
    public int selectedCharacterIndex;  // 選択されたキャラクターのインデックス
    public bool isConfirmed;  // キャラクター選択が確定しているかどうか
    public int playerScore; //戦闘時のスコア

    // コンストラクタ
    public PlayerSelectionData(int index)
    {
        playerIndex = index;
        selectedCharacterIndex = -1;  // 初期値として何も選択されていない状態
        isConfirmed = false;  // キャラクター選択が未確定の状態
        playerScore = 0;
    }
}

public class DataHolder
{
    // 各プレイヤーの選択状態を保持するリスト
    public static List<PlayerSelectionData> playerValues = new List<PlayerSelectionData>();

    // プレイヤーごとのデータを初期化
    public static void InitializePlayerData(int numberOfPlayers)
    {
        playerValues.Clear();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerValues.Add(new PlayerSelectionData(i));
        }
    }

    public static bool AllPlayerReady(){
        bool AllPlayerReady = true;
        for(int i=0; i < playerValues.Count; i++){
           AllPlayerReady = AllPlayerReady && DataHolder.playerValues[i].isConfirmed;  
        }
        return AllPlayerReady;
    }


    public static float MaxSpeed;
    public static float MaxPower;
    public static float MaxCharge;
    public static float MaxWeight;
    public static float MaxJumpPenalty;
    // 各キャラクターのステータスを保持するリスト
    public static List<SlimeStatus> slimeStatusValues = new List<SlimeStatus>();
    public static void InitializeSlimeStatus(GameObject[] characterObject){
        slimeStatusValues.Clear();
        for(int i=0;i < characterObject.Length;i++){
            slimeStatusValues.Add(new SlimeStatus(characterObject[i]));
        }
        
        MaxSpeed = SlimeStatus.GetMaxSpeed(slimeStatusValues);
        MaxPower = SlimeStatus.GetMaxPower(slimeStatusValues);
        MaxCharge = SlimeStatus.GetMaxCharge(slimeStatusValues);
        MaxWeight = SlimeStatus.GetMaxWeight(slimeStatusValues);
        MaxJumpPenalty = SlimeStatus.GetMaxJumpPenalty(slimeStatusValues);
    }


    //プレイヤーの中で，スコアが最大のプレイヤーを選択(最大のプレイヤーが複数いるときも考える)
    public static List<int> SelectWinner(){
        List<int> winnerIndex = new List<int>();
        int maxScore = 0;

        if(GameModeFrag.isStock){
            for(int i=0; i < playerValues.Count; i++){
                if(playerValues[i].playerScore != 0){
                    winnerIndex.Add(i);
                }
            }
            return winnerIndex;
        }else{
            for(int i=0; i < playerValues.Count; i++){
                if(playerValues[i].playerScore > maxScore){
                    maxScore = playerValues[i].playerScore;
            }
            }
            for(int i=0; i < playerValues.Count; i++){
                if(playerValues[i].playerScore == maxScore){
                    winnerIndex.Add(i);
            }
        }
            return winnerIndex;
        }
        
    }
}

public class isPaused
{
    public static bool isPausing = false; 
}

public class GameMode{
    public static bool isMulti;
}

public class SlimeStatus{
    public int characterIndex;
    public float Speed;
    public float Power;
    public float Charge;
    public float Weight;
    public float JumpPenalty;
    public SlimeStatus(GameObject character){
        Rigidbody characterRb = character.GetComponent<Rigidbody>();
        PlayerController characterScript = character.gameObject.GetComponent<PlayerController>();
        Speed = characterScript._speed;
        Power = characterScript.AttackPower;
        Charge = characterScript.ChargeSpeed;
        Weight = characterRb.mass;
        JumpPenalty = characterScript.jumpPenalty;
    }

    // SlimeStatusリストの最大値を取得するためのメソッド
    public static float GetMaxSpeed(List<SlimeStatus> slimeStatuses) => slimeStatuses.Max(s => s.Speed);
    public static float GetMaxPower(List<SlimeStatus> slimeStatuses) => slimeStatuses.Max(s => s.Power);
    public static float GetMaxCharge(List<SlimeStatus> slimeStatuses) => slimeStatuses.Max(s => s.Charge);
    public static float GetMaxWeight(List<SlimeStatus> slimeStatuses) => slimeStatuses.Max(s => s.Weight);
    public static float GetMaxJumpPenalty(List<SlimeStatus> slimeStatuses) => slimeStatuses.Max(s => s.JumpPenalty);
}

public class GameTimer{
    public static float DefaultTime = 90.0f;
    public static float CountdownTime = 3.0f;
    public static float timeRemaining;
    public static void timerReset(){
        timeRemaining = DefaultTime;
    }
    public static void CountDownStart(){
        CountdownTime = 3.0f;
    }
}

public class MatchStarted{
    public static bool isMatchStarted = false;
}

public class SuddenDeath{
    public static bool isSuddenDeath = false;
}

public class NowPlayable{
    public static PlayerInput[] players;
}

public class FragReset{
    public static void Reset(){
    GameTimer.CountDownStart();
    MatchStarted.isMatchStarted = false;
    SuddenDeath.isSuddenDeath = false;
    }
}

public class GameModeFrag{
    public static bool isStock = false;

    // public static void FragChange(){
    //     isStuck = !isStuck;
    // }
}

public class StockGame{
    public static int StockCount = 3;
}


