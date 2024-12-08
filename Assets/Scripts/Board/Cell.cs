using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] private GameObject m_selection;
    [SerializeField] private ChessPiece chessOnCell;

    private Cell[] possibleCellToMove = new Cell[2];
    private int x, y;
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SetClickedCell(this);
        Debug.Log("Current Position : x->" + x + ", y->" + y);

        if (chessOnCell)
        {
            possibleCellToMove = GameManager.Instance.board.GetPossibleCellToMove(chessOnCell.type, x, y);
            Debug.Log(possibleCellToMove[0].x + ", " + possibleCellToMove[0].y);
            Debug.Log(possibleCellToMove[1].x + ", " + possibleCellToMove[1].y);
        }
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
    public void EventOnClick()
    {
        m_selection.SetActive(true);
    }

    public void ClearCell()
    {
        m_selection.SetActive(false);
    }
}
