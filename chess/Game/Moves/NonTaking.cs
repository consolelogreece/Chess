using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class NonTaking : IMove
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;

        private BoardTile From;

        private Piece _pieceTaken;

        public NonTaking(BoardTile to, Piece owningPiece)
        {
            To = to;
            From = owningPiece._board[owningPiece.CurrentPosition];
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

         public void UndoMove()
        {
            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece.CurrentPosition = From.Position;
            OwningPiece._board[From.Position].OccupyingPiece = OwningPiece;

            OwningPiece.TimesMoved--;
        }
    }
}