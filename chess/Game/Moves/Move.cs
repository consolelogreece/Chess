using Chess.Helpers;
using Chess.Pieces;

namespace Chess.Moves
{
    public class Move : IMove
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;

        public Move(BoardTile to, Piece owningPiece)
        {
            To = to;
            OwningPiece = owningPiece;
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public int MakeMove()
        {
            var val = To.OccupyingPiece?.PieceValue ?? 0;

            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;

            return val;
        }
    }
}