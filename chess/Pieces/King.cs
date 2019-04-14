using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class King : Piece
    {
        public King(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "King")
        {
        }

        public override bool CalculateMoves(PiecePosition move)
        {
            var possibleMoves = new List<PiecePosition>();

            if (move.row + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col)].OccupyingPiece == null || _board[new PiecePosition(move.row + 1, move.col)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col));

            if (move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row , move.col + 1)].OccupyingPiece == null || _board[new PiecePosition(move.row, move.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row, move.col + 1));

            if (move.col - 1 >= 0 && (_board[new PiecePosition(move.row, move.col - 1)].OccupyingPiece == null || _board[new PiecePosition(move.row, move.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row, move.col - 1));

            if (move.row - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col)].OccupyingPiece == null || _board[new PiecePosition(move.row - 1, move.col)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col));

            if (move.row + 1 < _board.RowColLen && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col + 1)].OccupyingPiece == null || _board[new PiecePosition(move.row + 1, move.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col + 1));

            if (move.row + 1 < _board.RowColLen && move.col - 1 >= 0 && (_board[new PiecePosition(move.row + 1, move.col - 1)].OccupyingPiece == null || _board[new PiecePosition(move.row + 1, move.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col - 1));

            if (move.row - 1 >= 0 && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row - 1, move.col + 1)].OccupyingPiece == null || _board[new PiecePosition(move.row - 1, move.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col + 1));

            if (move.row - 1 >= 0 && move.col - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col - 1)].OccupyingPiece == null || _board[new PiecePosition(move.row - 1, move.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col - 1));

            PossibleMoves = possibleMoves;

            return base.CalculateMoves(move);
        }

        public override void EliminateIllegalMoves()
        {
            PossibleMoves.RemoveAll(m => _board[new PiecePosition(m.row, m.col)].ThreateningPieces.Any(p => p.PieceOwner.Id != this.PieceOwner.Id));
        }
    }
}