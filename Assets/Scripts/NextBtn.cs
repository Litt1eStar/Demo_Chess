using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBtn : MonoBehaviour
{
    public SelectCharacterController controller;

    public void OnClick()
    {
        controller.OnClickNextBtn();
    }
}
