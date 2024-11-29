using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var current = Keyboard.current;
        
        if (current == null){
            Debug.Log("Key boardが接続されていません．");
            return;
        }else{
            Debug.Log("Key boardが接続されました．");
        }
    }

    // Update is called once per frame
    void Update()
    {
        var current = Keyboard.current;
        
        var aKey = current.aKey;
        

        if(aKey.wasPressedThisFrame){
            Debug.Log("Aキーが押されました．");
        }

        if(aKey.wasReleasedThisFrame){
            Debug.Log("Aキーが話されました．");
        }

        if(aKey.isPressed){
            Debug.Log("Aキーが押されています．");
        }
    }
}
