using System.Collections.Generic;
using Chess.Helpers;
using Chess.Pieces;

namespace Chess.Moves
{
    public class Move : IMove
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;

        private BoardTile From;

        private Piece _pieceTaken;

        public Move(BoardTile to, Piece owningPiece)
        {
            To = to;
            From = owningPiece._board[owningPiece.CurrentPosition];
            OwningPiece = owningPiece;
            _pieceTaken = OwningPiece._board[To.Position].OccupyingPiece;
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

        public void UndoMove()
        {
            if (_pieceTaken != null)
            {
                _pieceTaken.CurrentPosition = To.Position;
            }

            // will either set it to null, or the taken piece, therefore no null check required.
            OwningPiece._board[To.Position].OccupyingPiece = _pieceTaken;
            OwningPiece.CurrentPosition = From.Position;
            OwningPiece._board[From.Position].OccupyingPiece = OwningPiece;

            OwningPiece.TimesMoved--;
        }
    }
}