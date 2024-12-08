using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cell_h, cell_nh;
    [SerializeField] private GameObject chess_r, chess_y;
    [SerializeField] private Transform board_pos;

    [SerializeField] private int size_x = 8, size_y = 8;

    GameObject[,] cells;
    GameObject[] chessPieces;

    private void Awake()
    {
        cells = new GameObject[size_x, size_y];
        chessPieces = new GameObject[size_x + size_y];
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
                        cells[x, y] = m_cell;
                    }
                    else
                    {
                        GameObject m_cell = Instantiate (cell_nh, board_pos);
                        cells[x, y] = m_cell;
                    }
                }else if(x % 2 != 0)
                {
                    if(y % 2 == 0)
                    {
                        GameObject m_cell = Instantiate(cell_nh, board_pos);
                        cells[x, y] = m_cell;
                    }
                    else
                    {
                        GameObject m_cell = Instantiate(cell_h, board_pos);
                        cells[x, y] = m_cell;
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
                if(y >= board_max - 2 || y < board_min + 2)
                {
                    if (x % 2 == 0)
                    {
                        if (y % 2 == 0)
                        {
                            Transform pos = cells[x, y].gameObject.transform;
                            GameObject m_chess = Instantiate(chess_y, pos);
                            chessPieces.Append(m_chess);
                        }
                    }
                    else if (x % 2 != 0)
                    {
                        if (y % 2 != 0)
                        {
                            Transform pos = cells[x, y].gameObject.transform;
                            GameObject m_chess = Instantiate(chess_y, pos);
                            chessPieces.Append(m_chess);
                        }
                    }
                }
            }
        }
    }
}
