using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public float countdownTimeMinute;
    private float playerTimer;
    private float enemyTimer;
    private Turn state;
    public TextMeshProUGUI playerTimerTxt;
    public TextMeshProUGUI enemyTimerTxt;

    void Start()
    {
        playerTimer = countdownTimeMinute * 60;
        enemyTimer = countdownTimeMinute * 60;
        UpdateTimerDisplay(Turn.PLAYER);
        UpdateTimerDisplay(Turn.ENEMY);
    }

    void Update()
    {
        state = GameManager.Instance.current_turn;
        switch (state)
        {
            case Turn.PLAYER:
                if (playerTimer > 0)
                {
                    playerTimer -= Time.deltaTime;
                    UpdateTimerDisplay(state);
                }
                else
                {
                    playerTimer = 0;
                    UpdateTimerDisplay(state);
                    TimerEnded(Turn.ENEMY);
                }
                break;
            case Turn.ENEMY:
                if (enemyTimer > 0)
                {
                    enemyTimer -= Time.deltaTime;
                    UpdateTimerDisplay(state);
                }
                else
                {
                    enemyTimer = 0;
                    UpdateTimerDisplay(state);
                    TimerEnded(Turn.PLAYER); 
                }
                break;
            default:
                break;
        }
        
    }

    void UpdateTimerDisplay(Turn _state)
    {
        switch (_state)
        {
            case Turn.PLAYER:
                UpdatePlayerTimerTxt();
                break;
            case Turn.ENEMY:
                UpdateEnemyTimerTxt();
                break;
            default:
                break;
        }

    }

    void UpdatePlayerTimerTxt()
    {
        int minutes, seconds;
        minutes = Mathf.FloorToInt(playerTimer / 60);
        seconds = Mathf.FloorToInt(playerTimer % 60);
        playerTimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateEnemyTimerTxt()
    {
        int minutes, seconds;
        minutes = Mathf.FloorToInt(enemyTimer / 60);
        seconds = Mathf.FloorToInt(enemyTimer % 60);
        enemyTimerTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded(Turn winner)
    {
        switch (winner)
        {
            case Turn.PLAYER:
                SceneManager.LoadScene("PlayerWin");
                break;
            case Turn.ENEMY:
                SceneManager.LoadScene("EnemyWin");
                break;
        }
    }

    public void IncreaseTime(Turn target, int second)
    {
        switch (target) 
        {
            case Turn.PLAYER:
                playerTimer += second;
                UpdatePlayerTimerTxt();
                break;
            case Turn.ENEMY:
                enemyTimer += second;
                UpdateEnemyTimerTxt();
                break;
        }
    }
}
