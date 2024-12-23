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

    [SerializeField] private GameObject p_rook, p_knight, p_bishop, p_king, p_met, p_pawn;
    [SerializeField] private GameObject e_rook, e_knight, e_bishop, e_king, e_met, e_pawn;

    [Header("Board Setting")]
    public int size_x = 8, size_y = 8, row_to_generate = 2;
    public float generateBoardDelay = 0.05f, generateChessDelay = 0.1f;
    public bool isTesting = false;

    public Cell[] highlightCells = new Cell[3];
    public Cell[] possibleCellToMove = new Cell[2];

    Cell[,] cells;
    ChessPiece[] chessPieces;
    public ChessPiece testPiece;
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
        if (!isTesting)
        {
            for (int y = 0; y <= row_to_generate; y++)
            {
                for (int x = 0; x < size_x; x++)
                {
                    //First Row
                    if (y == 0)
                    {
                        //boat
                        if (x == 0 || x == size_x - 1)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, e_rook, ChessType.ENEMY, delay, ChessClass.ROOK);
                        }

                        //horse
                        if (x == 1 || x == size_x - 2)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, e_knight, ChessType.ENEMY, delay, ChessClass.KNIGHT);
                        }

                        //kone
                        if (x == 2 || x == size_x - 3)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, e_bishop, ChessType.ENEMY, delay, ChessClass.BISHOP);
                        }

                        //King
                        if (x == 4)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, e_king, ChessType.ENEMY, delay, ChessClass.KING);
                        }

                        //Med
                        if (x == 3)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, e_met, ChessType.ENEMY, delay, ChessClass.MET);
                        }
                    }

                    if (y == row_to_generate)
                    {
                        Cell cell = cells[x, y];
                        yield return SpawnChessPiece(cell, e_pawn, ChessType.ENEMY, delay, ChessClass.PAWN);
                    }
                }
            }

            for (int y = size_y - 1; y >= size_y - (row_to_generate + 1); y--)
            {
                for (int x = 0; x < size_x; x++)
                {
                    //Last row
                    if (y == size_y - 1)
                    {
                        //boat
                        if (x == 0 || x == size_x - 1)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, p_rook, ChessType.ALLY, delay, ChessClass.ROOK);
                        }

                        //horse
                        if (x == 1 || x == size_x - 2)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, p_knight, ChessType.ALLY, delay, ChessClass.KNIGHT);
                        }

                        //kone
                        if (x == 2 || x == size_x - 3)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, p_bishop, ChessType.ALLY, delay, ChessClass.BISHOP);
                        }

                        //King
                        if (x == 3)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, p_king, ChessType.ALLY, delay, ChessClass.KING);
                        }

                        //Med
                        if (x == 4)
                        {
                            Cell cell = cells[x, y];
                            yield return SpawnChessPiece(cell, p_met, ChessType.ALLY, delay, ChessClass.MET);
                        }
                    }

                    if (y == size_y - (row_to_generate + 1))
                    {
                        Cell cell = cells[x, y];
                        yield return SpawnChessPiece(cell, p_pawn, ChessType.ALLY, delay, ChessClass.PAWN);
                    }
                }
            }
        }
        else
        {
            Cell cell = cells[3, 4];
            yield return SpawnChessPiece(cell, p_pawn, ChessType.ALLY, delay, ChessClass.PAWN);
        }

    }
    IEnumerator SpawnChessPiece(Cell cell, GameObject prefab, ChessType type, float delay, ChessClass chessClass)
    {
        if (isTesting)
        {
            GameObject m_chess = Instantiate(prefab, cell.gameObject.transform);
            ChessPiece piece = m_chess.GetComponent<ChessPiece>();

            piece.SetChessData(type, chessClass);
            testPiece = piece;
            chessPieces.Append(piece);

            cell.SetChessOnCell(piece);

            m_chess.transform.localScale = Vector3.zero;
            StartCoroutine(ScaleOverTime(m_chess.transform, Vector3.zero, Vector3.one, 0.3f));

            yield return new WaitForSeconds(delay);
        }
        else
        {
            GameObject m_chess = Instantiate(prefab, cell.gameObject.transform);
            ChessPiece piece = m_chess.GetComponent<ChessPiece>();

            piece.SetChessData(type, chessClass);
            chessPieces.Append(piece);

            cell.SetChessOnCell(piece);

            m_chess.transform.localScale = Vector3.zero;
            StartCoroutine(ScaleOverTime(m_chess.transform, Vector3.zero, Vector3.one, 0.3f));

            yield return new WaitForSeconds(delay);
        }
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
    public Cell[] GetPossibleCellToMove(ChessType type, bool isKing, int current_x, int current_y, ChessClass chessClass)
    {
        List<Cell> possibleCells = new List<Cell>();

        //Non-Rook Move
        if (chessClass != ChessClass.ROOK)
        {
            int[][] moves;
            switch (chessClass)
            {
                case ChessClass.PAWN:
                    moves = type == ChessType.ALLY
                        ? new int[][]
                        {
                            new int[] { -1, -1 }, // Move diagonally left to kill
                            new int[] { 0, -1 },  // Move forward
                            new int[] { 1, -1 }   // Move diagonally right to kill
                        }
                        : new int[][]
                        {
                            new int[] { -1, 1 },  // Move diagonally left to kill
                            new int[] { 0, 1 },   // Move forward
                            new int[] { 1, 1 }    // Move diagonally right to kill
                        };

                    bool canKill = false;

                    foreach (var move in moves)
                    {
                        int newX = current_x + move[0];
                        int newY = current_y + move[1];

                        if (newX >= 0 && newX < size_x && newY >= 0 && newY < size_y)
                        {
                            Cell targetCell = cells[newX, newY];

                            if (move[0] != 0) // Diagonal moves (kill moves)
                            {
                                if (targetCell.GetChessPiece() != null && targetCell.GetChessPiece().type != GameManager.Instance.CurrentCell().GetChessPiece().type)
                                {
                                    possibleCells.Add(targetCell);
                                    canKill = true;
                                }
                            }
                        }
                    }

                    if (!canKill)
                    {
                        int forwardX = current_x + moves[1][0];
                        int forwardY = current_y + moves[1][1];

                        if (forwardX >= 0 && forwardX < size_x && forwardY >= 0 && forwardY < size_y)
                        {
                            Cell forwardCell = cells[forwardX, forwardY];
                            if (forwardCell.GetChessPiece() == null)
                            {
                                possibleCells.Add(forwardCell);
                            }
                        }
                    }

                    break;
                case ChessClass.KNIGHT:
                    moves =
                        new int[][]
                        {
                            new int[] { -2, -1},//Up-Left
                            new int[] { -1, -2},//Up-Left
                            new int[] { 1, -2 },//Up-Right
                            new int[] { 2, -1 },//Up-Right
                            new int[] { 2, 1},//Down-Right
                            new int[] { 1, 2 },//Down-Right
                            new int[] {-2, 1},//Down-Left
                            new int[] {-1, 2},//Down-Left
                        };

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
                                possibleCells.Add(firstCell);
                            }
                        }
                    }
                    break;
                case ChessClass.BISHOP:
                    moves = type == ChessType.ALLY ?
                        new int[][]
                        {
                            new int[] { -1, -1 },
                            new int[] { 0, -1 },
                            new int[] { 1, -1 },
                            new int[] { -1, 1},
                            new int[] { 1, 1 },
                        } :
                        new int[][]
                        {
                            new int[] { 1, 1},
                            new int[] { 0, 1},
                            new int[] { -1, 1},
                            new int[] { -1, -1},
                            new int[] { 1, -1}
                        };

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
                                possibleCells.Add(firstCell);
                            }
                        }
                    }
                    break;
                case ChessClass.MET:
                    moves =
                        new int[][]
                        {
                            new int[] { -1, -1 },
                            new int[] { 1, -1 },
                            new int[] { 1, 1 },
                            new int[] { -1, 1 }
                        };

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
                                possibleCells.Add(firstCell);
                            }
                        }
                    }
                    break;
                case ChessClass.KING:
                    moves =
                        new int[][]
                        {
                            new int[] { -1, -1 },
                            new int[] { 0, -1 },
                            new int[] { 1, -1 },
                            new int[] { 1, 0},
                            new int[] { 1, 1 },
                            new int[] { 0, 1 },
                            new int[] { -1, 1 },
                            new int[] { -1, 0 }
                        };

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
                                possibleCells.Add(firstCell);
                            }
                        }
                    }
                    break;
            }
        }
        else //Rook Move
        {
            int[][] directions = new int[][]
            {
            new int[] {0,-1}, // front
            new int[] {1,0}, // right
            new int[] {0,1}, // back
            new int[] {-1,0}  // down-right
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
                        possibleCells.Add(currentCell);
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
    public void ChangeTestPieceData(ChessClass new_testClass)
    {
        if(testPiece == null)
        {
            Debug.LogWarning("Test Piece is Null");
            return;
        }
        switch (new_testClass)
        {
            case ChessClass.PAWN:
                testPiece.chessClass = new_testClass;
                testPiece.ChangeImage(GameManager.Instance.ui.pieceSpriteList[0]);
                break;
            case ChessClass.KNIGHT:
                testPiece.chessClass = new_testClass;
                testPiece.ChangeImage(GameManager.Instance.ui.pieceSpriteList[1]);
                break;
            case ChessClass.BISHOP:
                testPiece.chessClass = new_testClass;
                testPiece.ChangeImage(GameManager.Instance.ui.pieceSpriteList[4]);
                break;
            case ChessClass.ROOK:
                testPiece.chessClass = new_testClass;
                testPiece.ChangeImage(GameManager.Instance.ui.pieceSpriteList[2]);
                break;
            case ChessClass.MET:
                testPiece.chessClass = new_testClass;
                testPiece.ChangeImage(GameManager.Instance.ui.pieceSpriteList[5]);
                break;
            case ChessClass.KING:
                testPiece.chessClass = new_testClass;
                testPiece.ChangeImage(GameManager.Instance.ui.pieceSpriteList[3]);
                break;
        }
    }

}
