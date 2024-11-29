using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    public Slider SpeedSlider;
    public Slider PowerSlider;
    public Slider ChargeSlider;
    public Slider WeightSlider;
    public Slider JumpPenaltySlider;
    public GameObject System;
    private CharacterSelection SystemScript;

    void Start()
    {
        SystemScript = System.GetComponent<CharacterSelection>();
        
    }
    // Update is called once per frame
    void Update()
    {
        int SelectIndex = SystemScript.GetSelectIndex();
        if(SelectIndex <= DataHolder.slimeStatusValues.Count - 1){
            SpeedSlider.value = DataHolder.slimeStatusValues[SelectIndex].Speed/DataHolder.MaxSpeed;
            PowerSlider.value = DataHolder.slimeStatusValues[SelectIndex].Power/DataHolder.MaxPower;
            ChargeSlider.value = DataHolder.slimeStatusValues[SelectIndex].Charge/DataHolder.MaxCharge;
            WeightSlider.value = DataHolder.slimeStatusValues[SelectIndex].Weight/DataHolder.MaxWeight;
            JumpPenaltySlider.value = 1 - DataHolder.slimeStatusValues[SelectIndex].JumpPenalty/DataHolder.MaxJumpPenalty;   
        }else{
            SpeedSlider.value = 0;
            PowerSlider.value = 0;
            ChargeSlider.value = 0;
            WeightSlider.value = 0;
            JumpPenaltySlider.value = 0;
        }
        
    }
}
