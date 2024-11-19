using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;


public class SelectBgmPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip decide;
    public AudioClip selectBar;
    public Button[] Buttons;
    public MultiplayerEventSystem playerEventSystem;

    private GameObject previousSelectedObject; // 前のフレームで選択されていたオブジェクト
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // for(int i=0; i<Buttons.Length; i++){
        //     int index = i;
        //     Buttons[i].onClick.AddListener(() => PlayDecideSound());
        // }
    }

    // public void PlayDecideSound()
    // {
    //     if (audioSource != null && decide != null)
    //     {
    //         audioSource.PlayOneShot(decide);
    //     }
    // }

    void Update()
    {
        // 現在選択されているオブジェクトを取得
        GameObject currentSelectedObject = playerEventSystem.currentSelectedGameObject;

        // 選択が変更されたか確認
        if (currentSelectedObject != previousSelectedObject)
        {
            audioSource.PlayOneShot(selectBar);
            // 前の選択を更新
            previousSelectedObject = currentSelectedObject;
        }
    }
}
