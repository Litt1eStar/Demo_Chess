using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseTime : MonoBehaviour
{
    public Turn state;
    public float cooldownDurationInMinute;

    [SerializeField] private int timeToAddInSecond = 10;
    private float cooldownDuration;
    private bool isOnCooldown = false;
    private float remainingCooldownTime = 0f;

    private Image icon;
    public TextMeshProUGUI cooldownText;

    private void Start()
    {
        cooldownDuration = cooldownDurationInMinute * 60;
        cooldownText.text = "";
    }

    public void OnClick()
    {
        if (!isOnCooldown && this.state == GameManager.Instance.current_turn)
        {
            AddTime(timeToAddInSecond);
            StartCoroutine(CooldownRoutine());
        }
        else
        {
            Debug.Log($"Action is on cooldown. Please wait {Mathf.CeilToInt(remainingCooldownTime)} seconds.");
        }
    }

    void AddTime(int second = 10)
    {
        GameManager.Instance.timeController.IncreaseTime(state, second);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.timeSkill);
    }

    IEnumerator CooldownRoutine()
    {
        isOnCooldown = true;
        remainingCooldownTime = cooldownDuration;

        while (remainingCooldownTime > 0)
        {
            if (GameManager.Instance.current_turn == this.state)
            {
                remainingCooldownTime -= Time.deltaTime;

                if (cooldownText != null)
                {
                    int minutes = Mathf.FloorToInt(remainingCooldownTime / 60);
                    int seconds = Mathf.FloorToInt(remainingCooldownTime % 60);
                    cooldownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

                    icon = GetComponent<Image>();
                    var tempColor = icon.color;
                    tempColor.a = 0.5f;
                    icon.color = tempColor;
                }
            }
            else
            {
                Debug.Log("Paused cooldown: Waiting for player's turn.");
            }

            yield return null;
        }

        remainingCooldownTime = 0f;
        isOnCooldown = false;

        if (cooldownText != null && icon != null)
        {
            cooldownText.text = "";
            var tempColor = icon.color;
            tempColor.a = 1f;
            icon.color = tempColor;
        }
    }
}
