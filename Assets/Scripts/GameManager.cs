using System;
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
    public TimeController timeController;
    public SkillManager skill;

    public bool isUsingFreeze;
    public int currentKillStreak = 0;

    public Cell freezeCell;
    [SerializeField] private Cell currentCell;

    private int playerDeadPieces = 0;
    private int enemyDeadPieces = 0;

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
        if (isUsingFreeze)
        {
            switch (state)
            {
                case GameState.PLAYER:
                    if (_clickedCell.GetChessPiece()?.type == ChessType.ENEMY)
                    {
                        FreezePiece(_clickedCell);
                        isUsingFreeze = false;
                    }
                    break;

                case GameState.ENEMY:
                    if (_clickedCell.GetChessPiece()?.type == ChessType.ALLY)
                    {
                        FreezePiece(_clickedCell);
                        isUsingFreeze = false;
                    }
                    break;
            }
            return;
        }


        // Normal Logic for Movement and Selection
        if (board.possibleCellToMove.Contains(_clickedCell))
        {
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

        // Standard Piece Selection Logic
        switch (state)
        {
            case GameState.PLAYER:
                if (_clickedCell.GetChessPiece()?.type == ChessType.ALLY && _clickedCell.GetChessPiece().IsFrozenThisTurn() == false)
                {
                    SelectCell(_clickedCell);
                }
                break;

            case GameState.ENEMY:
                if (_clickedCell.GetChessPiece()?.type == ChessType.ENEMY && _clickedCell.GetChessPiece().IsFrozenThisTurn() == false)
                {
                    SelectCell(_clickedCell);
                }
                break;
        }
    }


    private void HandleKill(Cell targetCell)
    {
        ChessPiece currentChessPiece = currentCell.GetChessPiece();

        int dirX = targetCell.GetX() - currentCell.GetX();
        int dirY = targetCell.GetY() - currentCell.GetY();

        //Clamp value to not more than 1
        int nextCellX = targetCell.GetX() + dirX / Math.Max(Math.Abs(dirX), 1); 
        int nextCellY = targetCell.GetY() + dirY / Math.Max(Math.Abs(dirY), 1);
        Cell nextCell = board.GetCellBy(nextCellX, nextCellY);

        if (targetCell.GetChessPiece() == null || targetCell.GetChessPiece().type == currentChessPiece.type) return;

        Destroy(targetCell.GetChessPiece().gameObject);
        targetCell.SetChessOnCell(null);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pieceKill);
        currentKillStreak += 1;

        if (currentKillStreak == 2) skill.IncreaseFreezeQuota(state);

        if (currentChessPiece.isKing)
        {
            if (nextCell != null && nextCell.GetChessPiece() == null)
            {
                MovePieceToTarget(currentChessPiece, nextCell);
            }
            /*else
            {
                MovePieceToTarget(currentChessPiece, nextCell); 
            }*/
        }
        else
        {
            if (nextCell != null && nextCell.GetChessPiece() == null)
            {
                MovePieceToTarget(currentChessPiece, nextCell);
            }
            else
            {
                MovePieceToTarget(currentChessPiece, targetCell);
            }
        }

        if (state == GameState.PLAYER)
        {
            enemyDeadPieces++;
            ui.UpdateDeadPiecesArea(state);
        }
        else
        {
            playerDeadPieces++;
            ui.UpdateDeadPiecesArea(state);
        }

        //Get Killable Cell
        currentCell = nextCell;
        Cell[] newPossibleCellToMove = board.GetPossibleCellToMove(currentChessPiece.type, currentChessPiece.isKing, nextCellX, nextCellY);
        Cell[] possibleCellToKill = board.GetKillablePieceFromPossibleCellToMove(newPossibleCellToMove);

        board.ClearPossibleCellToMove();
        board.ClearAllHighlightOnBoard();

        if(possibleCellToKill.Length > 0)
        {
            Debug.Log(possibleCellToKill.Length);
            board.SetPossibleCellToMove(possibleCellToKill);
            foreach (Cell cellToHighlight in possibleCellToKill)
            {
                if (cellToHighlight != null)
                {
                    Debug.Log("Insert Highlight Cell");
                    board.InsertHighlightCell(cellToHighlight);
                }
            }
            board.StartHighlightCell();
        }
        else
        {
            board.ClearPossibleCellToMove();
            board.ClearAllHighlightOnBoard();
            SwitchTurn();
        }
    }

    private void SelectCell(Cell cell)
    {
        currentCell = cell;
        board.ClearAllHighlightOnBoard();
        board.InsertHighlightCell(currentCell);

        if (currentCell.HasChessPiece())
        {
            ChessPiece chessPiece = currentCell.GetChessPiece();
            Cell[] possibleCells = board.GetPossibleCellToMove(chessPiece.type, chessPiece.isKing, currentCell.GetX(), currentCell.GetY());
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

    private void FreezePiece(Cell targetCell)
    {
        if (targetCell != null && targetCell.HasChessPiece())
        {
            targetCell.GetChessPiece().FreezePiece();
            Debug.Log("Piece Frozen at: " + targetCell.GetX() + ", " + targetCell.GetY());
            board.ClearAllHighlightOnBoard();
        }
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
        if(targetCell.GetY() == 7 || targetCell.GetY() == 0)
        {
            piece.TurnToKing();
        }

        piece.gameObject.transform.SetParent(targetCell.transform);
        piece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pieceWalk);

        targetCell.SetChessOnCell(piece);
        currentCell.SetChessOnCell(null);
    }

    private void SwitchTurn()
    {
        currentKillStreak = 0;

        // Unfreeze all frozen pieces at the end of each turn
        foreach (var cell in board.GetAllCells())
        {
            if (cell.HasChessPiece())
            {
                ChessPiece chessPiece = cell.GetChessPiece();
                if (chessPiece.IsFrozenThisTurn())
                {
                    chessPiece.UnfreezePiece();
                }
            }
        }

        // Switch turns
        state = state == GameState.PLAYER ? GameState.ENEMY : GameState.PLAYER;
    }




    public Cell CurrentCell() => currentCell;
}
