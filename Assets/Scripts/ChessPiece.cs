using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType
{
    ALLY,
    ENEMY
}
public class ChessPiece : ChessBaseClass
{
    public ChessType type;

    public void SetChessData(ChessType _type)
    {
        type = _type;
    }
}
