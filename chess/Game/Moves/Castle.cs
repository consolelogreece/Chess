using Chess.Pieces;

namespace Chess.Moves
{
    public class Castle : IMove
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;
        private readonly Rook _castlingRook;

        public Castle(BoardTile to, Piece pieceOwner, Rook castlingRook)
        {
            To = to;
            OwningPiece = pieceOwner;
            _castlingRook = castlingRook;
        }

        public BoardTile GetMovePos()
        {
            return To;
        }

        public int MakeMove()
        {
            var _castlingRookPosOffset = OwningPiece.CurrentPosition.col < _castlingRook.CurrentPosition.col ? -1 : 1;

            OwningPiece._board[OwningPiece.CurrentPosition].OccupyingPiece = null;
            OwningPiece._board[To.Position].OccupyingPiece = OwningPiece;
            OwningPiece.CurrentPosition = To.Position;

            OwningPiece._board[_castlingRook.CurrentPosition].OccupyingPiece = null;
            var newRookPos = new PiecePosition(_castlingRook.CurrentPosition.row, To.Position.col + _castlingRookPosOffset);
            OwningPiece._board[newRookPos].OccupyingPiece = _castlingRook;
            _castlingRook.CurrentPosition = newRookPos;

            return 0;
        }
    }
}