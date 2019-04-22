using System.Collections.Generic;
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

        public List<string> GetMoveMeta()
        {
           return new List<string>()
            {
                "Taking"
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