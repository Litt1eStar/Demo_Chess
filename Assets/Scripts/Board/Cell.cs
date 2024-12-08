using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject m_selection;
    [SerializeField] private ChessPiece chessOnCell;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SetClickedCell(this);
        Debug.Log(chessOnCell.gameObject.name);
    }

    public void SetChessOnCell(ChessPiece _chessOnCell)
    {
        chessOnCell = _chessOnCell;
    }
    public void EventOnClick()
    {
        m_selection.SetActive(true);
    }

    public void ClearCell()
    {
        m_selection.SetActive(false);
    }
}
