using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private GameObject cell_h, cell_nh;
    [SerializeField] private Transform board_pos;
    GameObject[,] cells = new GameObject[8,8];

    private void Start()
    {
        InitBoard(8, 8);
    }

    void InitBoard(int size_x, int size_y)
    {
        for (int x = 0; x < size_x; x++)
        {
            for (int y = 0; y < size_y; y++)
            {
                if(y % 2 == 0)
                {
                    if(x % 2 == 0)
                    {
                        GameObject m_cell = Instantiate(cell_h, board_pos);
                        cells[x, y] = m_cell;
                    }
                    else
                    {
                        GameObject m_cell = Instantiate (cell_nh, board_pos);
                        cells[x, y] = m_cell;
                    }
                }else if(y % 2 != 0)
                {
                    if(x % 2 == 0)
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
}
