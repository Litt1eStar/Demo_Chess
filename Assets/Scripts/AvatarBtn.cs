using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarBtn : MonoBehaviour
{
    public SelectCharacterController controller;
    public Color selectedColor;
    public Color disabledColor;

    public string avatarName;
    public string player;

    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        UpdateButtonState();
    }

    public void SetAvatarData()
    {
        if (avatarName == controller.avatarName_player01 || avatarName == controller.avatarName_player02)
        {
            return;
        }

        var colors = btn.colors;
        colors.normalColor = selectedColor;
        btn.colors = colors;

        controller.SetAvatarData(avatarName, player);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ui_select);

        controller.RefreshButtons();
    }

    public void UpdateButtonState()
    {
        if (btn == null)
        {
            btn = GetComponent<Button>();
        }

        bool isSelected = avatarName == controller.avatarName_player01 || avatarName == controller.avatarName_player02;
        btn.interactable = !isSelected;

        var colors = btn.colors;
        colors.normalColor = isSelected ? disabledColor : Color.white;
        btn.colors = colors;
    }

}
