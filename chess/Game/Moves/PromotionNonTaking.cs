using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class PromotionNonTaking : IMove
    {
        public List<string> GetMoveMeta()
        {
            return new List<string>(){"Promotion", "NonTaking"};
        }

         public readonly Piece OwningPiece;
        public readonly BoardTile To;

        private BoardTile From;

        private Piece _pieceTaken;
        
        private Piece _promotion;

        public PromotionNonTaking(BoardTile to, Piece owningPiece, Piece promotion)
        {
            To = to;
            From = owningPiece._board[owningPiece.CurrentPosition];
            OwningPiece = owningPiece;
            _promotion = promotion;
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public void MakeMove()
        {
            From.OccupyingPiece = null;
            _promotion.CurrentPosition = To.Position;
            _promotion._board.RegisterPiece(_promotion);

            OwningPiece.TimesMoved++;
        }

        public void UndoMove()
        {
            From.OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = From.Position;

            To.OccupyingPiece = null;

            OwningPiece.TimesMoved--;
        }
    }
}
