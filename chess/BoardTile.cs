using System.Collections.Generic;
using chess.pieces;

public class BoardTile
{
    public Piece OccupyingPiece;

    public List<Piece> ThreateningPieces;

    public BoardTile()
    {
        ThreateningPieces = new List<Piece>();
    }
}