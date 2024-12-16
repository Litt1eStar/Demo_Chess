using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerDeadTxt;
    [SerializeField] private TextMeshProUGUI enemyDeadTxt;

    //[SerializeField] private TextMeshProUGUI turnTxt;
    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;

    private void Start()
    {
        playerDeadTxt.text = "0";
        enemyDeadTxt.text = "0";
    }
    private void Update()
    {
        GameState state = GameManager.Instance.state;
/*
        if(state == GameState.PLAYER)
        {
            turnTxt.text = "YOUR TURN";
        }else if(state == GameState.ENEMY)
        {
            turnTxt.text = "ENEMY TURN";
        }*/
    }

    public void SetPieceCountTxt(GameState _state, int amount)
    {
        switch (_state)
        {
            case GameState.PLAYER:
                playerDeadTxt.text = amount.ToString();
                break;
            case GameState.ENEMY:
                enemyDeadTxt.text = amount.ToString();
                break;
        }
    }
}
