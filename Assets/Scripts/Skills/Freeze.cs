using UnityEngine;

public class Freeze : MonoBehaviour
{
    public int quotaLeft = 0;

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
    }

    public void IncreaseQuota() => quotaLeft++;
}
