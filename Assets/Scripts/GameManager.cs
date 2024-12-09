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
    [SerializeField] private Cell currentCell;

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
        // Case: Move to a possible cell
        if (board.possibleCellToMove.Contains(_clickedCell))
        {
            board.ClearAllHighlightOnBoard();

            ChessPiece currentChessPiece = currentCell.GetChessPiece();
            currentChessPiece.gameObject.transform.SetParent(_clickedCell.transform);
            currentChessPiece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

            _clickedCell.SetChessOnCell(currentChessPiece);
            currentCell.SetChessOnCell(null);

            board.ClearPossibleCellToMove();

            return;
        }

        // Deselect previously selected cell
        if (currentCell != null)
        {
            currentCell.DisableSelection();
        }

        // Select the new cell
        currentCell = _clickedCell;
        board.ClearAllHighlightOnBoard();
        board.InsertHighlightCell(currentCell);

        // Highlight possible moves if the cell has a chess piece
        if (currentCell.HasChessPiece())
        {
            ChessPiece chessPiece = currentCell.GetChessPiece();
            Cell[] possibleCells = board.GetPossibleCellToMove(chessPiece.type, currentCell.GetX(), currentCell.GetY());
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
        if (currentCell != null)
        {
            currentCell.DisableSelection();
        }
        currentCell = null;
    }

    private void EventOnClick(Cell _clickedCell)
    {
        currentCell = _clickedCell;
        GameManager.Instance.board.InsertHighlightCell(currentCell);
    }
}
