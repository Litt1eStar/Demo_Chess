using UnityEngine;
using UnityEngine.UI;

public class Freeze : MonoBehaviour
{
    [SerializeField] private Image icon;
    public int quotaLeft = 0;

    private void Start()
    {
        icon = GetComponent<Image>();
    }
    private void Update()
    {
        if (quotaLeft <= 0) 
        {
            Color tempColor = icon.color;
            tempColor.a = 0.5f;
            icon.color = tempColor;
        }
        else
        {
            Color tempColor = icon.color;
            tempColor.a = 1;
            icon.color = tempColor;
        }
    }
    public void OnClick()
    {
        if (quotaLeft > 0)
        {
            UsingSkill();
        }
        else
        {
            Debug.Log("Can't Use Skill");
        }
    }

    void UsingSkill()
    {
        quotaLeft--;
        GameManager.Instance.isUsingFreeze = true;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.iceSkill);
    }

    public void IncreaseQuota() => quotaLeft++;
}
