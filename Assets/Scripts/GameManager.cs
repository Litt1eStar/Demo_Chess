using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Turn
{
    PLAYER,
    ENEMY,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Controller")]
    public BoardController board;
    public UIController ui;
    public TimeController timeController;
    public SkillManager skill;

    [Header("Data")]
    public Turn current_turn;
    public bool isUsingFreeze;
    public int currentKillStreak = 0;

    /*public Cell freezeCell;*/

    private Cell currentCell;
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
        HandleEndGameCase();
    }
    public void OnClicked(Cell _clickedCell)
    {
        //If using freezing skill
        if (isUsingFreeze)
        {
            switch (current_turn)
            {
                case Turn.PLAYER:
                    if (_clickedCell.GetChessPiece()?.type == ChessType.ENEMY)
                    {
                        FreezePiece(_clickedCell);
                        isUsingFreeze = false;
                    }
                    break;

                case Turn.ENEMY:
                    if (_clickedCell.GetChessPiece()?.type == ChessType.ALLY)
                    {
                        FreezePiece(_clickedCell);
                        isUsingFreeze = false;
                    }
                    break;
            }
            return;
        }
        // If clicked on one of possibleCellToMove
        if (board.possibleCellToMove.Contains(_clickedCell))
        {
            // If on clicked cell have chess piece and that piece have different type with current cell(opponent)
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

        // Selection Logic
        switch (current_turn)
        {
            case Turn.PLAYER:
                if (_clickedCell.GetChessPiece()?.type == ChessType.ALLY && _clickedCell.GetChessPiece().IsFrozenThisTurn() == false)
                {
                    SelectCell(_clickedCell);
                }
                break;

            case Turn.ENEMY:
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

        //Calculate position of next cell to move
        //NOTE: Use Math.Max() for clamp value to not less than 1 and used Math.Abs() for get only positive value of dirX or dirY
        //NOTE: dirX / Math.Max(Math.Abs(dirx), 1) is used for get direction of move (1 is right, -1 is left)
        //but there are a case that dirX will more than 1 so i just divide by itself and keep sign as same by divide dirX by positive value
        int nextCellX = targetCell.GetX() + dirX / Math.Max(Math.Abs(dirX), 1);
        int nextCellY = targetCell.GetY() + dirY / Math.Max(Math.Abs(dirY), 1);
        Cell nextCell = board.GetCellBy(nextCellX, nextCellY);

        UpdateDataOnKill(targetCell);
        Cell destinationCell = nextCell != null && nextCell.GetChessPiece() == null ? nextCell : targetCell;
        MovePieceToTarget(currentChessPiece, destinationCell);
        UpdateUIOnKill();

        HandleChainKilling(nextCell, currentChessPiece, nextCellX, nextCellY);
    }
    private void HandleChainKilling(Cell nextCell, ChessPiece currentChessPiece, int nextCellX, int nextCellY)
    {
        currentCell = nextCell;
        Cell[] newPossibleCellToMove = board.GetPossibleCellToMove(currentChessPiece.type, currentChessPiece.isKing, nextCellX, nextCellY, currentChessPiece.chessClass);
        Cell[] possibleCellToKill = board.GetKillablePieceFromPossibleCellToMove(newPossibleCellToMove);

        board.ClearPossibleCellToMove();
        board.ClearAllHighlightOnBoard();

        if (possibleCellToKill.Length > 0)
        {
            board.SetPossibleCellToMove(possibleCellToKill);
            foreach (Cell cellToHighlight in possibleCellToKill)
            {
                if (cellToHighlight != null)
                {
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
            Debug.Log(cell.ToString());
            ChessPiece chessPiece = currentCell.GetChessPiece();
            Cell[] possibleCells = board.GetPossibleCellToMove(chessPiece.type, chessPiece.isKing, currentCell.GetX(), currentCell.GetY(), chessPiece.chessClass);
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
            board.ClearAllHighlightOnBoard();
        }
    }
    private void MovePiece(Cell targetCell)
    {
        ChessPiece currentChessPiece = currentCell.GetChessPiece();

        MovePieceToTarget(currentChessPiece, targetCell);

        board.ClearPossibleCellToMove();
        board.ClearAllHighlightOnBoard();
    }
    private void MovePieceToTarget(ChessPiece piece, Cell targetCell)
    {
        //If targetCell is at last row or first row then piece will turn to be King
        if (targetCell.GetY() == 7 || targetCell.GetY() == 0)
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

        current_turn = current_turn == Turn.PLAYER ? Turn.ENEMY : Turn.PLAYER;
    }
    private void HandleEndGameCase()
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
    private void UpdateDataOnKill(Cell targetCell)
    {
        Destroy(targetCell.GetChessPiece().gameObject);
        targetCell.SetChessOnCell(null);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.pieceKill);
        currentKillStreak += 1;
        if (currentKillStreak == 2) skill.IncreaseFreezeQuota(current_turn);
    }
    private void UpdateUIOnKill()
    {
        if (current_turn == Turn.PLAYER)
        {
            enemyDeadPieces++;
            ui.UpdateDeadPiecesArea(current_turn);
        }
        else
        {
            playerDeadPieces++;
            ui.UpdateDeadPiecesArea(current_turn);
        }
    }
    public Cell CurrentCell() => currentCell;
}
