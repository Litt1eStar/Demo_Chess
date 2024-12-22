using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Freeze playerFreeze;
    [SerializeField] private Freeze enemyFreeze;

    public void IncreaseFreezeQuota(Turn _state)
    {
        switch (_state)
        {
            case Turn.PLAYER:
                playerFreeze.IncreaseQuota();
                break;
            case Turn.ENEMY:
                enemyFreeze.IncreaseQuota();
                break;
        }
    }
}
