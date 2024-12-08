using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject m_selection;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SetClickedCell(this);
        EventOnClick();
    }

    private void EventOnClick()
    {
        m_selection.SetActive(true);
    }
}
