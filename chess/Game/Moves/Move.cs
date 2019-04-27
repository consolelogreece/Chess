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
            _pieceTaken = To.OccupyingPiece;
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
            if (_pieceTaken != null)
            {
                OwningPiece._board.DeRegisterPiece(_pieceTaken);
            }

            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;

            OwningPiece.TimesMoved++;
        }

        public void UndoMove()
        {
            OwningPiece._board[To.Position].OccupyingPiece = null;

            if (_pieceTaken != null)
            {
                _pieceTaken.CurrentPosition = To.Position;
                OwningPiece._board.RegisterPiece(_pieceTaken);
            }

            OwningPiece.CurrentPosition = From.Position;
            OwningPiece._board[From.Position].OccupyingPiece = OwningPiece;

            OwningPiece.TimesMoved--;
        }
    }
}
