using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState
{
    
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardController board;
    [SerializeField] private Cell clickedCell;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void OnClicked(Cell _clickedCell)
    {
        //Case: Clicked on Possible Cell to Move
        if (board.possibleCellToMove.Contains(_clickedCell))
        {
            board.ClearAllHighlightOnBoard();
            Debug.Log("Move from " + clickedCell.ToString() + " | Move to " + _clickedCell.ToString());
            return;
        }

        //Disable Previous clicked cell
        if (clickedCell != null)
        {
            clickedCell.DisableSelection();
        }

        //Clear All Highlight
        board.ClearAllHighlightOnBoard();

        //Highlight new clicked cell
        clickedCell = _clickedCell;
        board.InsertHighlightCell(clickedCell);
        //clickedCell.EnableSelection();

        // Highlight possible cells to move if there's a chess piece on those cell
        if (clickedCell.HasChessPiece())
        {
            ChessPiece chessPiece = clickedCell.GetChessPiece();
            Cell[] possibleCells = board.GetPossibleCellToMove(chessPiece.type, clickedCell.GetX(), clickedCell.GetY());
            board.SetPossibleCellToMove(possibleCells);

            foreach (Cell cell in possibleCells)
            {
                if (cell != null)
                {
                    board.InsertHighlightCell(cell);
                }
            }

            board.StartHighlightCell();
        }
    }



    private void ClearCellData()
    {
        if (clickedCell != null)
        {
            clickedCell.DisableSelection();
        }
        clickedCell = null;
    }

    private void EventOnClick(Cell _clickedCell)
    {
        clickedCell = _clickedCell;
        GameManager.Instance.board.InsertHighlightCell(clickedCell);
    }
}
