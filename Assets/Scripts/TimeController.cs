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
    private GameState state;
    public TextMeshProUGUI playerTimerTxt;
    public TextMeshProUGUI enemyTimerTxt;

    void Start()
    {
        playerTimer = countdownTimeMinute * 60;
        enemyTimer = countdownTimeMinute * 60;
        UpdateTimerDisplay(GameState.PLAYER);
        UpdateTimerDisplay(GameState.ENEMY);
    }

    void Update()
    {
        state = GameManager.Instance.state;
        switch (state)
        {
            case GameState.PLAYER:
                if (playerTimer > 0)
                {
                    playerTimer -= Time.deltaTime;
                    UpdateTimerDisplay(state);
                }
                else
                {
                    playerTimer = 0;
                    UpdateTimerDisplay(state);
                    TimerEnded(GameState.ENEMY);
                }
                break;
            case GameState.ENEMY:
                if (enemyTimer > 0)
                {
                    enemyTimer -= Time.deltaTime;
                    UpdateTimerDisplay(state);
                }
                else
                {
                    enemyTimer = 0;
                    UpdateTimerDisplay(state);
                    TimerEnded(GameState.PLAYER); 
                }
                break;
            default:
                break;
        }
        
    }

    void UpdateTimerDisplay(GameState _state)
    {
        int minutes, seconds;
        switch (_state)
        {
            case GameState.PLAYER:
                UpdatePlayerTimerTxt();
                break;
            case GameState.ENEMY:
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

    void TimerEnded(GameState winner)
    {
        switch (winner)
        {
            case GameState.PLAYER:
                SceneManager.LoadScene("PlayerWin");
                break;
            case GameState.ENEMY:
                SceneManager.LoadScene("EnemyWin");
                break;
        }
    }

    public void IncreaseTime(GameState target, int second)
    {
        switch (target) 
        {
            case GameState.PLAYER:
                playerTimer += second;
                UpdatePlayerTimerTxt();
                break;
            case GameState.ENEMY:
                enemyTimer += second;
                UpdateEnemyTimerTxt();
                break;
        }
    }
}
