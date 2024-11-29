using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDataInitial : MonoBehaviour
{
    
    public GameObject[] characterPrefabs;
    void Start()
    {
        DataHolder.InitializeSlimeStatus(characterPrefabs);
    }
}
