using System.Collections.Generic;
using chess.pieces;

public class BoardTile
{
    public Piece OccupyingPiece;

    public List<Piece> ThreateningPieces;

    public readonly PiecePosition Position;

    public BoardTile(PiecePosition pos)
    {
        ThreateningPieces = new List<Piece>();

        Position = pos;
    }
}