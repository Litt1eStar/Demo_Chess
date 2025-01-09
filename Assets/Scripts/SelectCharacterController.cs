using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacterController : MonoBehaviour
{
    public Animator player01_anim;
    public Animator player02_anim;

    public string avatarName_player01 = "";
    public string avatarName_player02 = "";

    public string player01_name;
    public string player02_name;

    public AnimatorController blueOrg;
    public AnimatorController greenOrg;
    public AnimatorController lightGreenOrg;
    public AnimatorController pinkOrg;
    public AnimatorController redOrg;
    public AnimatorController whiteOrg;

    public List<AvatarBtn> avatarButtons;

    private void Start()
    {
        avatarName_player01 = "lightGreenOrg";
        avatarName_player02 = "pinkOrg";

        // Refresh button states at start
        RefreshButtons();
    }

    private void Update()
    {
        HandleAvatar();
    }

    private void HandleAvatar()
    {
        if (avatarName_player01.Length > 0)
        {
            switch (avatarName_player01)
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
        }

        if (avatarName_player02.Length > 0)
        {
            switch (avatarName_player02)
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

    public void SetAvatarData(string _avatarName, string player)
    {
        if (player == "player01")
            this.avatarName_player01 = _avatarName;
        else if (player == "player02")
            this.avatarName_player02 = _avatarName;
    }

    public void RefreshButtons()
    {
        foreach (var btn in avatarButtons)
        {
            btn.UpdateButtonState();
        }
    }

    public void ChangePlayerName(string player, string value)
    {
        if (player == "player01")
        {
            player01_name = value;
        }
        else if (player == "player02")
        {
            player02_name = value;
        }
    }

    public void OnClickNextBtn()
    {
        PlayerPrefs.SetString("player01", avatarName_player01);
        PlayerPrefs.SetString("player02", avatarName_player02);
        PlayerPrefs.SetString("player01_name", player01_name);
        PlayerPrefs.SetString("player02_name", player02_name);
        SceneManager.LoadScene("Final");
    }
}
