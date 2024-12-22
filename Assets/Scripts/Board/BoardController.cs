using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class BoardController : MonoBehaviour
{
    [Header("Object Information")]
    [SerializeField] private GameObject cell_h, cell_nh;
    [SerializeField] private GameObject chess_enemy, chess_ally;
    [SerializeField] private Transform board_pos;

    [Header("Board Setting")]
    public int size_x = 8, size_y = 8, row_to_generate = 2;
    public float generateBoardDelay = 0.05f, generateChessDelay = 0.1f;

    public Cell[] highlightCells = new Cell[3];
    public Cell[] possibleCellToMove = new Cell[2];

    Cell[,] cells;
    ChessPiece[] chessPieces;

    private void Awake()
    {
        cells = new Cell[size_x, size_y];
        chessPieces = new ChessPiece[size_x + size_y];
    }
    private IEnumerator Start()
    {
        yield return StartCoroutine(InitBoardAnimated(size_x, size_y, generateBoardDelay));
        yield return StartCoroutine(InitChessAnimated(size_x, size_y, generateChessDelay));
    }
    IEnumerator InitBoardAnimated(int size_x, int size_y, float delay)
    {
        for (int y = 0; y < size_y; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
                GameObject prefab = (x % 2 == 0 && y % 2 == 0) || (x % 2 != 0 && y % 2 != 0) ? cell_h : cell_nh;
                GameObject m_cell = Instantiate(prefab, board_pos);
                Cell cell = m_cell.GetComponent<Cell>();
                cell.SetCellData(x, y);
                cells[x, y] = cell;

                m_cell.transform.localScale = Vector3.zero;
                StartCoroutine(ScaleOverTime(m_cell.transform, Vector3.zero, Vector3.one, 0.3f));

                yield return new WaitForSeconds(delay);
            }
        }
    }
    IEnumerator InitChessAnimated(int size_x, int size_y, float delay)
    {
        for (int y = 0; y < row_to_generate; y++)
        {
            for(int x = 0; x < size_x; x++)
            {
                if((y%2==0 && x % 2 == 0) || (y%2!=0 && x%2!=0))
                {
                    Cell cell = cells[x, y];
                    yield return SpawnChessPiece(cell, chess_enemy, ChessType.ENEMY, delay);
                }
            } 
        }

        for (int y = size_y - row_to_generate; y < size_y; y++)
        {
            for (int x = 0; x < size_x; x++)
            {
                if ((y % 2 == 0 && x % 2 == 0) || (y % 2 != 0 && x % 2 != 0))
                {
                    Cell cell = cells[x, y];
                    yield return SpawnChessPiece(cell, chess_ally, ChessType.ALLY, delay);
                }
            }
        }
    }
    IEnumerator SpawnChessPiece(Cell cell, GameObject prefab, ChessType type, float delay)
    {
        GameObject m_chess = Instantiate(prefab, cell.gameObject.transform);
        ChessPiece piece = m_chess.GetComponent<ChessPiece>();

        piece.SetChessData(type);
        chessPieces.Append(piece);

        cell.SetChessOnCell(piece);

        m_chess.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleOverTime(m_chess.transform, Vector3.zero, Vector3.one, 0.3f));

        yield return new WaitForSeconds(delay);
    }
    IEnumerator ScaleOverTime(Transform target, Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.localScale = Vector3.Lerp(fromScale, toScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localScale = toScale;
    }
    public Cell[] GetPossibleCellToMove(ChessType type, bool isKing, int current_x, int current_y)
    {
        List<Cell> possibleCells = new List<Cell>();

        if (!isKing)
        {
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
        }
        else
        {
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
                        break; //Stop if out of board

                    Cell currentCell = cells[x, y];

                    if (currentCell.GetChessPiece() == null)
                    {
                        possibleCells.Add(currentCell);
                    }
                    else if (currentCell.GetChessPiece().type != GameManager.Instance.CurrentCell().GetChessPiece().type)
                    {
                        int beyondX = x + direction[0];
                        int beyondY = y + direction[1];

                        if (beyondX >= 0 && beyondX < size_x && beyondY >= 0 && beyondY < size_y)
                        {
                            Cell beyondCell = cells[beyondX, beyondY];
                            if (beyondCell.GetChessPiece() == null)
                            {
                                possibleCells.Add(currentCell);
                            }
                        }
                        break; // Stop after finding an enemy piece
                    }
                    else
                    {
                        break; // Stop if encountering own piece
                    }
                }
            }
        }

        return possibleCells.ToArray();
    }
    public Cell[] GetKillablePieceFromPossibleCellToMove(Cell[] _possibleCellToMove)
    {
        List<Cell> killableCells = new List<Cell>();

        foreach (Cell cell in _possibleCellToMove)
        {
            if(cell.GetChessPiece() != null)
            {
                killableCells.Add(cell);
            }
        }

        return killableCells.ToArray();
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
        ClearPossibleCellToMove();
        possibleCellToMove = _possibleCellToMove;
    }
    public void ClearPossibleCellToMove()
    {
        possibleCellToMove = new Cell[0];
    }
    public Cell GetCellBy(int x, int y) => cells[x, y];
    public Cell[,] GetAllCells() => cells;

}
