using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInputField : MonoBehaviour
{
    public string player;
    public SelectCharacterController sceneController;
    
    public void OnValueChange()
    {
        TMP_InputField inputField = GetComponent<TMP_InputField>();
        if(player == "player01")
        {
            string value = inputField.text;
            sceneController.player01_name = value;
            Debug.Log(sceneController.player01_name);
        }
        else if(player == "player02")
        {
            string value = inputField.text;
            sceneController.player02_name = value;
        }
    }
}
