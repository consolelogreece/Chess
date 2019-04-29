using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class Promotion : IMove
    {
        public List<string> GetMoveMeta()
        {
            return new List<string>(){"Promotion"};
        }

         public readonly Piece OwningPiece;
        public readonly BoardTile To;

        private BoardTile From;

        private Piece _pieceTaken;
        
        private Piece _promotion;

        public Promotion(BoardTile to, Piece owningPiece, Piece promotion)
        {
            To = to;
            From = owningPiece._board[owningPiece.CurrentPosition];
            OwningPiece = owningPiece;
            _promotion = promotion;
            _pieceTaken = To.OccupyingPiece;
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public void MakeMove()
        {
            if (_pieceTaken != null)
            {
                _pieceTaken.CurrentPosition = To.Position;
                OwningPiece._board.DeRegisterPiece(_pieceTaken);
            }

            From.OccupyingPiece = null;
            _promotion.CurrentPosition = To.Position;
            OwningPiece._board.RegisterPiece(_promotion);

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
            From.OccupyingPiece = OwningPiece;

            OwningPiece._board.DeRegisterPiece(_promotion);

            OwningPiece.TimesMoved--;
        }
    }
}