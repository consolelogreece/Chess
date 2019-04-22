using System;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class EnPassant : IMove
    {
        public readonly Piece OwningPiece;

        public readonly BoardTile To;

        private int direction;

        public EnPassant(BoardTile to, Piece owningPiece)
        {
            if (To?.OccupyingPiece != null) throw new InvalidOperationException("Space occupied, not en passant.");

            direction = owningPiece.CurrentPosition.row < to.Position.row ? -1 : 1;

            To = to;

            OwningPiece = owningPiece;
        }

        public List<string> GetMoveMeta()
        {
           return new List<string>()
            {
                "Indirect",
                "Taking"
            };
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public void MakeMove()
        {
            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;

            var enPassantPieceTaken = OwningPiece._board[new PiecePosition(OwningPiece.CurrentPosition.row + direction, OwningPiece.CurrentPosition.col)].OccupyingPiece;

            OwningPiece._board[enPassantPieceTaken.CurrentPosition].OccupyingPiece = null;
        }

        public float MoveVal()
        {
            // can only en passant other pawns, so just get the value of the taker, which is a pawn.
            float val = OwningPiece.PieceValue;

            val -= OwningPiece.BoardValueTable[OwningPiece.CurrentPosition.row, OwningPiece.CurrentPosition.col];

            val += OwningPiece.BoardValueTable[To.Position.row, To.Position.col];

            return val;
        }
    }
}