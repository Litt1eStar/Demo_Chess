using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Freeze playerFreeze;
    [SerializeField] private Freeze enemyFreeze;

    public void IncreaseFreezeQuota(GameState _state)
    {
        switch (_state)
        {
            case GameState.PLAYER:
                playerFreeze.IncreaseQuota();
                break;
            case GameState.ENEMY:
                enemyFreeze.IncreaseQuota();
                break;
        }
    }
}
