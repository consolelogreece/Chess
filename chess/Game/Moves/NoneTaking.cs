using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class NoneTaking : IMove
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;

        public NoneTaking(BoardTile to, Piece owningPiece)
        {
            To = to;
            OwningPiece = owningPiece;
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public List<string> GetMoveMeta()
        {
           return new List<string>()
            {
                "NonTaking"
            };
        }

        public void MakeMove()
        {
            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;
        }

        public float MoveVal()
        {
            float val = To.OccupyingPiece?.PieceValue ?? 0;

            val -= OwningPiece.BoardValueTable[OwningPiece.CurrentPosition.row, OwningPiece.CurrentPosition.col];

            val += OwningPiece.BoardValueTable[To.Position.row, To.Position.col];

            return val;
        }
    }
}