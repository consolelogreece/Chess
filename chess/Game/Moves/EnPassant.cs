using System;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class EnPassant : IMove
    {
        public readonly Piece OwningPiece;

        public readonly BoardTile To;

        private readonly BoardTile From;

        private Piece _pieceTaken;

        public EnPassant(BoardTile to, Piece owningPiece)
        {
            if (To?.OccupyingPiece != null)throw new InvalidOperationException("Space occupied, not en passant.");

            var direction = owningPiece.CurrentPosition.row < to.Position.row ? -1 : 1;

            From = owningPiece._board[owningPiece.CurrentPosition];

            To = to;

            OwningPiece = owningPiece;

            _pieceTaken = OwningPiece._board[new PiecePosition(to.Position.row + direction, to.Position.col)].OccupyingPiece;
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
            if (_pieceTaken != null) 
            {   
                OwningPiece._board.DeRegisterPiece(_pieceTaken);
            }

            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;

            OwningPiece.TimesMoved++;
        }

        public float MoveVal()
        {
            // can only en passant other pawns, so just get the value of the taker, which is a pawn.
            float val = OwningPiece.PieceValue;

            val -= OwningPiece.BoardValueTable[OwningPiece.CurrentPosition.row, OwningPiece.CurrentPosition.col];

            val += OwningPiece.BoardValueTable[To.Position.row, To.Position.col];

            return val;
        }

        public void UndoMove()
        {
            OwningPiece._board.RegisterPiece(_pieceTaken);
            
            OwningPiece.CurrentPosition = From.Position;
            OwningPiece._board[From.Position].OccupyingPiece = OwningPiece;
            OwningPiece._board[To.Position].OccupyingPiece = null;

            OwningPiece.TimesMoved--;
        }
    }
}
