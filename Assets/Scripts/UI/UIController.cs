using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Animator player01_anim;
    public TextMeshProUGUI player01_txt;
    public string player01_name;

    public Animator player02_anim;
    public TextMeshProUGUI player02_txt;
    public string player02_name;

    public string player01_avatarName;
    public string player02_avatarName;

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

    private void Start()
    {
        HandleAvatar();
        HandlePlayerName();
    }
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
    private void HandlePlayerName()
    {
        string player01_name = PlayerPrefs.GetString("player01_name");
        string player02_name = PlayerPrefs.GetString("player02_name");

        player01_txt.text = player01_name;
        player02_txt.text = player02_name;
    }
    private void HandleAvatar()
    {
        player01_avatarName = PlayerPrefs.GetString("player01");
        player02_avatarName = PlayerPrefs.GetString("player02");

        Debug.Log(player01_avatarName);
        Debug.Log(player02_avatarName);

        switch (player01_avatarName)
        {
            case "blueOrg":
                player01_anim.runtimeAnimatorController = blueOrg;
                break;
            case "greenOrg":
                player01_anim.runtimeAnimatorController = greenOrg;
                break;
            case "lightGreenOrg":
                player01_anim.runtimeAnimatorController = lightGreenOrg;
                break;
            case "pinkOrg":
                player01_anim.runtimeAnimatorController = pinkOrg;
                break;
            case "redOrg":
                player01_anim.runtimeAnimatorController = redOrg;
                break;
            case "whiteOrg":
                player01_anim.runtimeAnimatorController = whiteOrg;
                break;
        }

        switch (player02_avatarName)
        {
            case "blueOrg":
                player02_anim.runtimeAnimatorController = blueOrg;
                break;
            case "greenOrg":
                player02_anim.runtimeAnimatorController = greenOrg;
                break;
            case "lightGreenOrg":
                player02_anim.runtimeAnimatorController = lightGreenOrg;
                break;
            case "pinkOrg":
                player02_anim.runtimeAnimatorController = pinkOrg;
                break;
            case "redOrg":
                player02_anim.runtimeAnimatorController = redOrg;
                break;
            case "whiteOrg":
                player02_anim.runtimeAnimatorController = whiteOrg;
                break;
        }
    }
}
