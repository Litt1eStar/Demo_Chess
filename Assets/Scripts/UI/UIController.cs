using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{

    public TextMeshProUGUI killTxt;

    [SerializeField] private Transform playerDeadHolder;
    [SerializeField] private Transform enemyDeadHolder;
    [SerializeField] private GameObject m_playerPiece;
    [SerializeField] private GameObject m_enemyPiece;

    private void Start()
    {
    }
    private void Update()
    {
        GameState state = GameManager.Instance.state;
    
        killTxt.text = GameManager.Instance.currentKillStreak.ToString();
/*
        if(state == GameState.PLAYER)
        {
            turnTxt.text = "YOUR TURN";
        }else if(state == GameState.ENEMY)
        {
            turnTxt.text = "ENEMY TURN";
        }*/
    }
    public void UpdateDeadPiecesArea(GameState killer)
    {
        switch (killer)
        {
            case GameState.PLAYER:
                Instantiate(m_enemyPiece, playerDeadHolder);
                break;
            case GameState.ENEMY:
                Instantiate(m_playerPiece, enemyDeadHolder);
                break;
        }
    }
}
