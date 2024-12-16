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

    public int size_x = 8, size_y = 8, row_to_generate = 2;

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
                if(x % 2 == 0) //even
                {
                    if(y % 2 == 0) // even
                    {
                        GameObject m_cell = Instantiate(cell_h, board_pos);
                        Cell cell = m_cell.GetComponent<Cell>();
                        cell.SetCellData(x, y);
                        cells[x, y] = cell;
                    }
                    else // odd
                    {
                        GameObject m_cell = Instantiate (cell_nh, board_pos);
                        Cell cell = m_cell.GetComponent<Cell>();
                        cell.SetCellData(x, y);
                        cells[x, y] = cell;
                    }
                }else if(x % 2 != 0) //odd
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

    public Cell[] GetPossibleCellToMove(ChessType type, bool isKing,int current_x, int current_y)
    {
        if (!isKing)
        {
            List<Cell> possibleCells = new List<Cell>();

            int[][] moves = type == ChessType.ALLY
                ? new int[][] { new int[] { -1, -1 }, new int[] { 1, -1 } }
                : new int[][] { new int[] { -1, 1 }, new int[] { 1, 1 } };

            foreach (var move in moves)
            {
                int newX = current_x + move[0];
                int newY = current_y + move[1];

                if (newX >= 0 && newX < size_x && newY >= 0 && newY < size_y)
                {
                    Cell firstCell = cells[newX, newY];

                    if (firstCell.GetChessPiece() == null)
                    {
                        possibleCells.Add(firstCell);
                    }
                    else if (firstCell.GetChessPiece().type != GameManager.Instance.CurrentCell().GetChessPiece().type)
                    {
                        int beyondX = newX + move[0];
                        int beyondY = newY + move[1];

                        if (beyondX >= 0 && beyondX < size_x && beyondY >= 0 && beyondY < size_y)
                        {
                            Cell beyondCell = cells[beyondX, beyondY];

                            if (beyondCell.GetChessPiece() == null)
                            {
                                possibleCells.Add(firstCell);
                            }
                        }
                    }
                }
            }

            return possibleCells.ToArray();
        }

        else if (isKing)
        {
            List<Cell> possibleCells = new List<Cell>();

            int[][] directions = new int[][]
            {
                new int[] {-1, -1}, // up-left
                new int[] { 1, -1}, // up-right
                new int[] {-1,  1}, // down-left
                new int[] { 1,  1}  // down-right
            };

            foreach (var direction in directions)
            {
                int x = current_x;
                int y = current_y;

                while (true)
                {
                    x += direction[0];
                    y += direction[1];

                    if (x < 0 || x >= size_x || y < 0 || y >= size_y)
                        break;

                    Cell cell = cells[x, y];

                    if (cell.GetChessPiece() == null)
                    {
                        possibleCells.Add(cell);
                    }
                    else if (cell.GetChessPiece() != null && cell.GetChessPiece().type != GameManager.Instance.CurrentCell().GetChessPiece().type)
                    {
                        possibleCells.Add(cell);
                        break;
                    }
                    else
                    {
                        break; // Stop if encountering player piece
                    }
                }
            }

            return possibleCells.ToArray();
        }

        else
        {
            return null;
        }
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
        possibleCellToMove = new Cell[0];
    }

    public Cell GetCellBy(int x, int y) => cells[x, y];

}
