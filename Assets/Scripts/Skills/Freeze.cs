using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    public int quotaLeft = 0;

    public void IncreaseQuota() => quotaLeft += 1;
}
