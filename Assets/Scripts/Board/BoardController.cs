using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cell_h, cell_nh;
    [SerializeField] private GameObject chess_enemy, chess_ally;
    [SerializeField] private Transform board_pos;

    [SerializeField] private int size_x = 8, size_y = 8, row_to_generate = 2;

    public Cell[] highlightCells = new Cell[3];
    public Cell[] possibleCellToMove = new Cell[2];

    Cell[,] cells;
    ChessPiece[] chessPieces;

    private void Awake()
    {
        cells = new Cell[size_x, size_y];
        chessPieces = new ChessPiece[size_x + size_y];
    }
    private void Start()
    {
        InitBoard(size_x, size_y);
        InitChess(size_x, size_y);
    }

    void InitBoard(int size_x, int size_y)
    {
        for (int y = 0; y < size_y; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
                if(x % 2 == 0)
                {
                    if(y % 2 == 0)
                    {
                        GameObject m_cell = Instantiate(cell_h, board_pos);
                        Cell cell = m_cell.GetComponent<Cell>();
                        cell.SetCellData(x, y);
                        cells[x, y] = cell;
                    }
                    else
                    {
                        GameObject m_cell = Instantiate (cell_nh, board_pos);
                        Cell cell = m_cell.GetComponent<Cell>();
                        cell.SetCellData(x, y);
                        cells[x, y] = cell;
                    }
                }else if(x % 2 != 0)
                {
                    if(y % 2 == 0)
                    {
                        GameObject m_cell = Instantiate(cell_nh, board_pos);
                        Cell cell = m_cell.GetComponent<Cell>();
                        cell.SetCellData(x, y);
                        cells[x, y] = cell;
                    }
                    else
                    {
                        GameObject m_cell = Instantiate(cell_h, board_pos);
                        Cell cell = m_cell.GetComponent<Cell>();
                        cell.SetCellData(x, y);
                        cells[x, y] = cell;
                    }
                }
            }
        }
    }

    void InitChess(int size_x, int size_y)
    {
        int board_max = size_x;
        int board_min = 0;

        for(int y = 0;y < size_y; y++)
        {
            for(int x = 0;x < size_x; x++)
            {
                if(y >= board_max - row_to_generate || y < board_min + row_to_generate)
                {
                    //even row
                    if (x % 2 == 0)
                    {
                        if (y % 2 == 0 && y >= board_max - row_to_generate)
                        {
                            Cell cell = cells[x, y];
                            Transform pos = cell.gameObject.transform;
                            GameObject m_chess = Instantiate(chess_ally, pos);
                            ChessPiece piece = m_chess.GetComponent<ChessPiece>();
                            piece.SetChessData(ChessType.ALLY);
                            chessPieces.Append(piece);

                            cell.SetChessOnCell(piece);
                        }
                        else if (y % 2 == 0 && y < board_min + row_to_generate)
                        {
                            Cell cell = cells[x, y];
                            Transform pos = cell.gameObject.transform;
                            GameObject m_chess = Instantiate(chess_enemy, pos);
                            ChessPiece piece = m_chess.GetComponent<ChessPiece>();
                            piece.SetChessData(ChessType.ENEMY);
                            chessPieces.Append(piece);

                            cell.SetChessOnCell(piece);
                        }
                    }
                    //odd row
                    else if (x % 2 != 0)
                    {
                        if (y % 2 != 0 && y >= board_max - row_to_generate)
                        {
                            Cell cell = cells[x, y];
                            Transform pos = cell.gameObject.transform;
                            GameObject m_chess = Instantiate(chess_ally, pos);
                            ChessPiece piece = m_chess.GetComponent<ChessPiece>();
                            piece.SetChessData(ChessType.ALLY);
                            chessPieces.Append(piece);

                            cell.SetChessOnCell(piece);
                        }
                        else if (y % 2 != 0 && y < board_min + row_to_generate)
                        {
                            Cell cell = cells[x, y];
                            Transform pos = cell.gameObject.transform;
                            GameObject m_chess = Instantiate(chess_enemy, pos);
                            ChessPiece piece = m_chess.GetComponent<ChessPiece>();
                            piece.SetChessData(ChessType.ENEMY);
                            chessPieces.Append(piece);

                            cell.SetChessOnCell(piece);
                        }
                    }
                }
            }
        }
    }

    public Cell[] GetPossibleCellToMove(ChessType type,int current_x, int current_y)
    {
        List<Cell> possibleCells = new List<Cell>();

        int[][] moves = type == ChessType.ALLY
            ? new int[][] { new int[] { -1, -1 }, new int[] { 1, -1 } }
            : new int[][] { new int[] { -1, 1 }, new int[] { 1, 1 } };

        foreach (var move in moves)
        {
            int newX = current_x + move[0];
            int newY = current_y + move[1];

            if (newX >= 0 && newX < size_x && newY >= 0 && newY < size_y && cells[newX, newY].GetChessPiece() == null)
            {
                possibleCells.Add(cells[newX, newY]);
            }
        }

        return possibleCells.ToArray();

    }

    public void InsertHighlightCell(Cell highlightCell)
    {
        List<Cell> tempHighlightCells = highlightCells.ToList();
        tempHighlightCells.Add(highlightCell);
        highlightCells = tempHighlightCells.ToArray();
    }


    public void StartHighlightCell()
    {
        foreach (Cell cell in highlightCells)
        {
            cell?.m_selection.SetActive(true);
        }
    }
    public void ClearAllHighlightOnBoard()
    {
        foreach (Cell cell in highlightCells)
        {
            if (cell != null)
            {
                cell.m_selection.SetActive(false);
            }
        }

        highlightCells = new Cell[0];
    }

    public void SetPossibleCellToMove(Cell[] _possibleCellToMove)
    {
        //Clear Prev value
        ClearPossibleCellToMove();
        //Set new value
        possibleCellToMove = _possibleCellToMove;
    }

    public void ClearPossibleCellToMove()
    {
        possibleCellToMove = new Cell[0]; // Use an empty array instead of `null`
    }

}
