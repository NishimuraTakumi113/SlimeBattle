using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public GameObject StartPanel;
    // Start is called before the first frame update
    void Start()
    {
        StartPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        bool Ready = DataHolder.AllPlayerReady();
        if(Ready){
            StartPanel.SetActive(true);
        }else{
            StartPanel.SetActive(false);
        }
    }
}
