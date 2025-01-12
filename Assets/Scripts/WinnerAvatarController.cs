using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class WinnerAvatarController : MonoBehaviour
{
    private string winner_avatarName;
    private Sprite avatarSprite;
    public Animator winner_anim;

    public AnimatorController blueOrg;
    public AnimatorController greenOrg;
    public AnimatorController lightGreenOrg;
    public AnimatorController pinkOrg;
    public AnimatorController redOrg;
    public AnimatorController whiteOrg;

    private void Start()
    {
        winner_avatarName = PlayerPrefs.GetString("Winner");
        Debug.Log(winner_avatarName);

        GetAvatarSprite();
    }

    private void GetAvatarSprite()
    {
        switch (winner_avatarName)
        {
            case "blueOrg":
                winner_anim.runtimeAnimatorController = blueOrg;
                break;
            case "greenOrg":
                winner_anim.runtimeAnimatorController = greenOrg;
                break;
            case "lightGreenOrg":
                winner_anim.runtimeAnimatorController = lightGreenOrg;
                break;
            case "pinkOrg":
                winner_anim.runtimeAnimatorController = pinkOrg;
                break;
            case "redOrg":
                winner_anim.runtimeAnimatorController = redOrg;
                break;
            case "whiteOrg":
                winner_anim.runtimeAnimatorController = whiteOrg;
                break;
        }
    }
}
