using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PLAYER,
    ENEMY,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState state;
    public BoardController board;
    public UIController ui;
    
    [SerializeField] private Cell currentCell;

    private int playerDeadPieces = 0;
    private int enemyDeadPieces = 0;
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

    private void Update()
    {
        if (playerDeadPieces >= 8 && enemyDeadPieces < playerDeadPieces)
        {
            SceneManager.LoadScene("EnemyWin");
        }
        else if (enemyDeadPieces >= 8 && playerDeadPieces < enemyDeadPieces)
        {
            SceneManager.LoadScene("PlayerWin");
        }
    }
    public void OnClicked(Cell _clickedCell)
    {
        // Case: Move to a possible cell
        if (board.possibleCellToMove.Contains(_clickedCell))
        {
            // Check for "Kill" condition
            if (_clickedCell.GetChessPiece() != null && _clickedCell.GetChessPiece().type != currentCell.GetChessPiece().type)
            {
                HandleKill(_clickedCell);
            }
            else
            {
                MovePiece(_clickedCell);
                SwitchTurn();
            }

            return;
        }

        // Deselect previously selected cell
        if (currentCell != null)
        {
            currentCell.DisableSelection();
        }

        // Select the new cell based on the game state
        switch (state)
        {
            case GameState.PLAYER:
                if (_clickedCell.GetChessPiece()?.type == ChessType.ALLY)
                {
                    SelectCell(_clickedCell);
                }
                break;
            case GameState.ENEMY:
                if (_clickedCell.GetChessPiece()?.type == ChessType.ENEMY)
                {
                    SelectCell(_clickedCell);
                }
                break;
        }
    }

    private void HandleKill(Cell targetCell)
    {
        int currX = currentCell.GetX();
        int currY = currentCell.GetY();

        dir = targetCell.GetX() > currX ? 1 : -1; // Determine direction
        Cell killTarget = state == GameState.PLAYER
            ? board.GetCellBy(currX + 2 * dir, currY - 2)
            : board.GetCellBy(currX + 2 * dir, currY + 2);

        // If the kill target cell is occupied, return
        if (killTarget.GetChessPiece() != null) return;

        // Perform the kill and move the piece
        ChessPiece currChessPiece = currentCell.GetChessPiece();
        MovePieceToTarget(currChessPiece, killTarget);

        // Destroy the enemy piece
        Destroy(targetCell.GetChessPiece().gameObject);
        targetCell.SetChessOnCell(null);

        // Update game state
        if (state == GameState.PLAYER)
        {
            enemyDeadPieces++;
            ui.UpdateDeadPiecesArea(state);
            state = GameState.ENEMY;
        }
        else
        {
            playerDeadPieces++;
            ui.UpdateDeadPiecesArea(state);
            state = GameState.PLAYER;
        }

        board.ClearPossibleCellToMove();
        board.ClearAllHighlightOnBoard();
    }

    private void MovePiece(Cell targetCell)
    {
        ChessPiece currentChessPiece = currentCell.GetChessPiece(); // Temp reference
        MovePieceToTarget(currentChessPiece, targetCell);

        board.ClearPossibleCellToMove();
        board.ClearAllHighlightOnBoard();
    }

    private void MovePieceToTarget(ChessPiece piece, Cell targetCell)
    {
        // Move the piece to the new cell
        piece.gameObject.transform.SetParent(targetCell.transform);
        piece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Update cell data
        targetCell.SetChessOnCell(piece);
        currentCell.SetChessOnCell(null);
    }

    private void SelectCell(Cell cell)
    {
        currentCell = cell;
        board.ClearAllHighlightOnBoard();
        board.InsertHighlightCell(currentCell);

        if (currentCell.HasChessPiece())
        {
            ChessPiece chessPiece = currentCell.GetChessPiece();
            Cell[] possibleCells = board.GetPossibleCellToMove(chessPiece.type, currentCell.GetX(), currentCell.GetY());
            board.SetPossibleCellToMove(possibleCells);

            foreach (Cell cellToHighlight in possibleCells)
            {
                if (cellToHighlight != null)
                {
                    board.InsertHighlightCell(cellToHighlight);
                }
            }
            board.StartHighlightCell();
        }
    }

    private void SwitchTurn()
    {
        state = state == GameState.PLAYER ? GameState.ENEMY : GameState.PLAYER;
    }


    public Cell CurrentCell() => currentCell;
}
