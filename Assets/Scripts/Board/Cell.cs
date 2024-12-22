using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,IPointerClickHandler
{
    public GameObject m_selection;
    [SerializeField] private ChessPiece chessOnCell;
    private int x, y;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.OnClicked(this);
    }

    public void SetCellData(int _x, int _y)
    {
        x = _x; 
        y = _y;
    }

    public void SetChessOnCell(ChessPiece _chessOnCell)
    {
        chessOnCell = _chessOnCell;
    }
    public void EnableSelection()
    {
        m_selection.SetActive(true);
    }

    public void DisableSelection()
    {
        m_selection.SetActive(false);
    }

    public bool HasChessPiece()
    {
        return chessOnCell != null;
    }

    public ChessPiece GetChessPiece()
    {
        return chessOnCell;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public override string ToString()
    {
        return $"{chessOnCell.chessClass.ToString()} | {chessOnCell.type.ToString()}";
    }
}
