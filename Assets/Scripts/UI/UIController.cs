using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
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
