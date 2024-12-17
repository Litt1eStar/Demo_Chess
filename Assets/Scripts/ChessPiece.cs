using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChessType
{
    ALLY,
    ENEMY
}
public class ChessPiece : ChessBaseClass
{
    public ChessType type;
    public bool isKing = false;
    public bool isFrozen { get; private set; } = false;
    private int frozenTurnsRemaining = 0; // Counter for frozen turns

    [SerializeField] private Image chessImage;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color normalColor;

    private void Start()
    {
        chessImage.color = normalColor;
        chessImage.sprite = sprite;
    }

    public void SetChessData(ChessType _type)
    {
        type = _type;
    }

    public void TurnToKing()
    {
        isKing = true;
    }

    public void FreezePiece()
    {
        isFrozen = true;
        frozenTurnsRemaining = 2; // Set to 1 turn of freezing
        chessImage.color = frozenColor;
    }

    public void UnfreezePiece()
    {
        if (frozenTurnsRemaining <= 0) return;

        frozenTurnsRemaining--;  // Decrease frozen turn count
        if (frozenTurnsRemaining <= 0)
        {
            isFrozen = false;  // Unfreeze when count reaches 0
            chessImage.color = normalColor;
        }
    }

    public bool IsFrozenThisTurn()
    {
        return isFrozen;
    }
}

