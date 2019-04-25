using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Moves
{
    public class Castle : IMove
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;

        private readonly BoardTile KingFrom;
        private readonly Rook _castlingRook;

        private readonly BoardTile RookFrom;

        public Castle(BoardTile to, Piece pieceOwner, Rook castlingRook)
        {
            To = to;
            OwningPiece = pieceOwner;
            _castlingRook = castlingRook;
            KingFrom = OwningPiece._board[OwningPiece.CurrentPosition];
            RookFrom = _castlingRook._board[_castlingRook.CurrentPosition];
        }

        public List<string> GetMoveMeta()
        {
            return new List<string>()
            {
                "NonTaking",
                "Castle"
            };
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public void MakeMove()
        {
            var _castlingRookPosOffset = OwningPiece.CurrentPosition.col < _castlingRook.CurrentPosition.col ? -1 : 1;

            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;

            OwningPiece._board[_castlingRook.CurrentPosition].OccupyingPiece = null;
            var newRookPos = new PiecePosition(_castlingRook.CurrentPosition.row, To.Position.col + _castlingRookPosOffset);
            OwningPiece._board[newRookPos].OccupyingPiece = _castlingRook;
            _castlingRook.CurrentPosition = newRookPos;
        }

        public float MoveVal()
        {
            var _castlingRookPosOffset = OwningPiece.CurrentPosition.col < _castlingRook.CurrentPosition.col ? -1 : 1;

            var newRookPos = new PiecePosition(_castlingRook.CurrentPosition.row, To.Position.col + _castlingRookPosOffset);

            float val = 0;

            val -= OwningPiece.BoardValueTable[OwningPiece.CurrentPosition.row, OwningPiece.CurrentPosition.col];

            val += OwningPiece.BoardValueTable[To.Position.row, To.Position.col];

            val -= _castlingRook.BoardValueTable[_castlingRook.CurrentPosition.row, _castlingRook.CurrentPosition.col];

            val += _castlingRook.BoardValueTable[newRookPos.row, newRookPos.col];

            return val;
        }

        public void UndoMove()
        {
            _castlingRook.CurrentPosition = RookFrom.Position;
            RookFrom.OccupyingPiece = _castlingRook;
            _castlingRook.TimesMoved--;

            OwningPiece.CurrentPosition = KingFrom.Position;
            KingFrom.OccupyingPiece = OwningPiece;
            OwningPiece.TimesMoved--;
        }
    }
}
