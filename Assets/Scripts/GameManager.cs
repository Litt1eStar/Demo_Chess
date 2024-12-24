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

    [Header("Test Data")]
    public ChessClass testClass;
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
        if(board.isTesting) HandleTest();
    }

    private void HandleTest()
    {
        board.ChangeTestPieceData(testClass);
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
        ChessPiece deadPiece = targetCell.GetChessPiece();
            
        UpdateDataOnKill(targetCell);
        MovePieceToTarget(currentChessPiece, targetCell);
        UpdateUIOnKill();
        board.ClearPossibleCellToMove();
        board.ClearAllHighlightOnBoard();

        if(deadPiece.chessClass == ChessClass.KING)
        {
            HandleEndGameCase(current_turn);
            return;
        }

        SwitchTurn();
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
        piece.gameObject.transform.SetParent(targetCell.transform);
        piece.gameObject.GetComponent<RectTransform>().localPosition = Vector3.zero;

        AudioManager.Instance.PlaySFX(AudioManager.Instance.pieceWalk);

        targetCell.SetChessOnCell(piece);
        currentCell.SetChessOnCell(null);
    }
    private void SwitchTurn()
    {
        if (board.isTesting) return;

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
    private void HandleEndGameCase(Turn winner)
    {
        if (winner == Turn.ENEMY)
        {
            SceneManager.LoadScene("EnemyWin");
        }
        else if (winner == Turn.PLAYER)
        {
            SceneManager.LoadScene("PlayerWin");
        }
    }
    private void UpdateDataOnKill(Cell targetCell)
    {
        if (currentCell.GetChessPiece().chessClass == ChessClass.PAWN)
        {
            currentCell.GetChessPiece().TurnToAnotherVariant();
        }
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
