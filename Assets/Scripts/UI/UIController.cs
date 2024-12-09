using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnTxt;
    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;

    private void Update()
    {
        GameState state = GameManager.Instance.state;

        if(state == GameState.PLAYER)
        {
            turnTxt.text = "YOUR TURN";
        }else if(state == GameState.ENEMY)
        {
            turnTxt.text = "ENEMY TURN";
        }
    }
}
