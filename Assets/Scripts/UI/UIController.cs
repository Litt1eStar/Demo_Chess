using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator player1_anim;
    public string player1_name;

    public Animator player2_anim;
    public string player2_name;

    public AnimatorController blueOrg;
    public AnimatorController greenOrg;
    public AnimatorController lightGreenOrg;
    public AnimatorController pinkOrg;
    public AnimatorController redOrg;
    public AnimatorController whiteOrg;

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
