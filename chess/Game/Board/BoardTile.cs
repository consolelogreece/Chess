using System.Collections.Generic;
using Chess.Pieces;

namespace Chess
{
    public class BoardTile
    {
        public Piece OccupyingPiece;

        public List<Piece> ThreateningPieces;

        public readonly PiecePosition Position;

        public readonly int TileValue;

        public BoardTile(PiecePosition pos)
        {
            ThreateningPieces = new List<Piece>();

            Position = pos;
        }
    }
}