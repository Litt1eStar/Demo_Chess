using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Cell clickedCell;

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
    public void SetClickedCell(Cell _clickedCell)
    {
        ClearCellData();
        EventOnClick(_clickedCell);
    }

    private void ClearCellData()
    {
        if (clickedCell != null)
        {
            clickedCell.ClearCell();
        }
        clickedCell = null;
    }

    private void EventOnClick(Cell _clickedCell)
    {
        clickedCell = _clickedCell;
        clickedCell.EventOnClick();
    }
}
