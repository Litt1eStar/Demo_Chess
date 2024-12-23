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

    public Sprite[] pieceSpriteList = new Sprite[0];
    public void UpdateDeadPiecesArea(Turn killer)
    {
        switch (killer)
        {
            case Turn.PLAYER:
                Instantiate(m_enemyPiece, playerDeadHolder);
                break;
            case Turn.ENEMY:
                Instantiate(m_playerPiece, enemyDeadHolder);
                break;
        }
    }
}
