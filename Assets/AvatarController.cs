using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public void FinishAnimation()
    {
        GameManager.Instance.FinishAvatarAnimation();
    }
}
