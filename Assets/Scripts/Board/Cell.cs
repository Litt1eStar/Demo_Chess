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

    private Cell[] possibleCellToMove = new Cell[2];    
    private int x, y;
    public void OnPointerClick(PointerEventData eventData)
    {
        //Set ClickedCell Data and highlight all possible cell to move
        GameManager.Instance.SetClickedCell(this);
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

}
