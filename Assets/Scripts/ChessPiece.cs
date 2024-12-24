using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ChessType
{
    ALLY,
    ENEMY
}

public enum ChessClass
{
    PAWN,//0
    KNIGHT,//1
    BISHOP,//4
    ROOK,//2
    MET,//5
    KING//3
}
public class ChessPiece : MonoBehaviour
{
    public ChessType type;
    public ChessClass chessClass;
    public bool isKing = false;
    public bool isFrozen { get; private set; } = false;
    public int[][] pieceMoves;
    private int frozenTurnsRemaining = 0; // Counter for frozen turns

    [Header("Visual Info")]
    [SerializeField] private Image chessImage;
    [SerializeField] private Sprite checkerSprite;
    [SerializeField] private Sprite downPanwSprite;
    [SerializeField] private Color frozenColor;
    [SerializeField] private Color normalColor;

    public void SetChessData(ChessType _type, ChessClass _class)
    {
        type = _type;
        chessClass = _class;
    }
    public void TurnToAnotherVariant()
    {
        if (chessImage != null) 
        {
            chessImage.sprite = null;
            chessImage.sprite = downPanwSprite;

            AudioManager.Instance.PlaySFX(AudioManager.Instance.king);
            this.SetMoveByClass(ChessClass.MET);
            chessClass = ChessClass.MET;
        }
        isKing = true;
    }
    public void SetMoveByClass(ChessClass _class)
    {
        int[][] moves = null;
        switch (_class)
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
                break;
            case ChessClass.ROOK:
                moves = 
                     new int[][]
                     {
                        new int[] {0,-1}, // front
                        new int[] {1,0}, // right
                        new int[] {0,1}, // back
                        new int[] {-1,0}  // down-right
                     };
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
                break;
        }

        pieceMoves = moves;
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
    public void ChangeImage(Sprite _sprite)
    {
        chessImage.sprite = _sprite;    
    }
}

