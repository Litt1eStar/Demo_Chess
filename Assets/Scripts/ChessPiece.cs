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
    [SerializeField] private Sprite checkerSprite;
    [SerializeField] private Sprite kingSprite;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color normalColor;

    public void SetChessData(ChessType _type)
    {
        type = _type;
    }

    public void TurnToKing()
    {
        if (chessImage != null) 
        {
            chessImage.sprite = null;
            chessImage.sprite = kingSprite;

            AudioManager.Instance.PlaySFX(AudioManager.Instance.king);
        }
        isKing = true;
    }

    public void FreezePiece()
    {
        isFrozen = true;
        frozenTurnsRemaining = 2;
        chessImage.color = frozenColor;
    }

    public void UnfreezePiece()
    {
        if (frozenTurnsRemaining <= 0) return;

        frozenTurnsRemaining--; 
        if (frozenTurnsRemaining <= 0)
        {
            isFrozen = false;
            chessImage.color = normalColor;
        }
    }

    public bool IsFrozenThisTurn()
    {
        return isFrozen;
    }
}

