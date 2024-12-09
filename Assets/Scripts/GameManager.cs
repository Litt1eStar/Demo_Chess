using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

public enum GameState
{
    PLAYER,
    ENEMY,
    WINNING,
    LOSING
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state;
    public BoardController board;
    
    [SerializeField] private Cell currentCell;

    private int dir; // -1 is left, 1 is right

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
        // Case: Enemy is on clicked Cell

        // Case: Move to a possible cell
        if (board.possibleCellToMove.Contains(_clickedCell))
        {
            if (_clickedCell.GetChessPiece() != null && _clickedCell.GetChessPiece().type != currentCell.GetChessPiece().type)
            {
                int currX = currentCell.GetX();
                int currY = currentCell.GetY();

                if (_clickedCell.GetX() - currentCell.GetX() > 0) dir = 1;
                else if (_clickedCell.GetX() - currentCell.GetX() < 0) dir = -1;

                board.ClearAllHighlightOnBoard();

                switch (state)
                {
                    case GameState.PLAYER:
                        if (dir == 1)
                        {
                            //Find Target Cell that current cell will move to
                            Cell targetCell = board.GetCellBy(currX + 2, currY - 2);

                            //If there is a piece on that cell then we can't kill
                            if (targetCell.GetChessPiece() != null) return;

                            //Keep Temp of Chess Piece of current cell
                            ChessPiece currChessPiece = currentCell.GetChessPiece();

                            //Set chess piece data on target cell by using temp
                            targetCell.SetChessOnCell(currChessPiece);

                            //Clear Data on current cell and move chess piece to target cell
                            currentCell.SetChessOnCell(null);
                            currChessPiece.gameObject.transform.SetParent(targetCell.transform);
                            currChessPiece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            Destroy(_clickedCell.GetChessPiece().gameObject);
                            _clickedCell.SetChessOnCell(null);

                            board.ClearPossibleCellToMove();
                        }
                        else if (dir == -1)
                        {
                            //Find Target Cell that current cell will move to
                            Cell targetCell = board.GetCellBy(currX - 2, currY - 2);

                            //If there is a piece on that cell then we can't kill
                            if (targetCell.GetChessPiece() != null) return;

                            //Keep Temp of Chess Piece of current cell
                            ChessPiece currChessPiece = currentCell.GetChessPiece();

                            //Set chess piece data on target cell by using temp
                            targetCell.SetChessOnCell(currChessPiece);

                            //Clear Data on current cell and move chess piece to target cell
                            currentCell.SetChessOnCell(null);
                            currChessPiece.gameObject.transform.SetParent(targetCell.transform);
                            currChessPiece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            Destroy(_clickedCell.GetChessPiece().gameObject);
                            _clickedCell.SetChessOnCell(null);

                            board.ClearPossibleCellToMove();
                        }
                        break;

                    case GameState.ENEMY:
                        if (dir == 1)
                        {
                            //Find Target Cell that current cell will move to
                            Cell targetCell = board.GetCellBy(currX + 2, currY + 2);

                            //If there is a piece on that cell then we can't kill
                            if (targetCell.GetChessPiece() != null) return;

                            //Keep Temp of Chess Piece of current cell
                            ChessPiece currChessPiece = currentCell.GetChessPiece();

                            //Set chess piece data on target cell by using temp
                            targetCell.SetChessOnCell(currChessPiece);

                            //Clear Data on current cell and move chess piece to target cell
                            currentCell.SetChessOnCell(null);
                            currChessPiece.gameObject.transform.SetParent(targetCell.transform);
                            currChessPiece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            Destroy(_clickedCell.GetChessPiece().gameObject);
                            _clickedCell.SetChessOnCell(null);

                            board.ClearPossibleCellToMove();
                        }
                        else if (dir == -1)
                        {
                            //Find Target Cell that current cell will move to
                            Cell targetCell = board.GetCellBy(currX - 2, currY + 2);

                            //If there is a piece on that cell then we can't kill
                            if (targetCell.GetChessPiece() != null) return;

                            //Keep Temp of Chess Piece of current cell
                            ChessPiece currChessPiece = currentCell.GetChessPiece();

                            //Set chess piece data on target cell by using temp
                            targetCell.SetChessOnCell(currChessPiece);

                            //Clear Data on current cell and move chess piece to target cell
                            currentCell.SetChessOnCell(null);
                            currChessPiece.gameObject.transform.SetParent(targetCell.transform);
                            currChessPiece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;
                            Destroy(_clickedCell.GetChessPiece().gameObject);
                            _clickedCell.SetChessOnCell(null);

                            board.ClearPossibleCellToMove();
                        }
                        break;
                    case GameState.WINNING:
                        break;
                    case GameState.LOSING:
                        break;
                }

                return;
            }


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

        switch (state)
        {
            case GameState.PLAYER:
                if(_clickedCell.GetChessPiece().type == ChessType.ALLY)
                {
                    currentCell = _clickedCell;
                    board.ClearAllHighlightOnBoard();
                    board.InsertHighlightCell(currentCell);
                }
                break;
            case GameState.ENEMY:
                if (_clickedCell.GetChessPiece().type == ChessType.ENEMY)
                {
                    currentCell = _clickedCell;
                    board.ClearAllHighlightOnBoard();
                    board.InsertHighlightCell(currentCell);
                }
                break;
            case GameState.WINNING:
                break;
            case GameState.LOSING:
                break;
        }
        // Select the new cell


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

    public Cell CurrentCell() => currentCell;
}
