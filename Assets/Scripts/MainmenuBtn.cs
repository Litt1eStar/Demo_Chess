using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuBtn : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("SelectCharacter");
    }
}
