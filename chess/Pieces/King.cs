using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class King : Piece
    {
        public King(Player pieceOwner, Board board)
        : base(pieceOwner, board, "King")
        {
        }

        public override bool CalculateMoves(PiecePosition move)
        {
            var possibleMoves = new List<PiecePosition>();

            if (move.row + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col)] == null || _board[new PiecePosition(move.row + 1, move.col)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col));

            if (move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row , move.col + 1)] == null || _board[new PiecePosition(move.row, move.col + 1)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row, move.col + 1));

            if (move.col - 1 >= 0 && (_board[new PiecePosition(move.row, move.col - 1)] == null || _board[new PiecePosition(move.row, move.col - 1)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row, move.col - 1));

            if (move.row - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col)] == null || _board[new PiecePosition(move.row - 1, move.col)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col));

            if (move.row + 1 < _board.RowColLen && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col + 1)] == null || _board[new PiecePosition(move.row + 1, move.col + 1)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col + 1));

            if (move.row + 1 < _board.RowColLen && move.col - 1 >= 0 && (_board[new PiecePosition(move.row + 1, move.col - 1)] == null || _board[new PiecePosition(move.row + 1, move.col - 1)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col - 1));

            if (move.row - 1 >= 0 && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row - 1, move.col + 1)] == null || _board[new PiecePosition(move.row - 1, move.col + 1)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col + 1));

            if (move.row - 1 >= 0 && move.col - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col - 1)] == null || _board[new PiecePosition(move.row - 1, move.col - 1)].PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col - 1));

            PossibleMoves = possibleMoves;

            return true;
        }

        public override void EliminateIllegalMoves(PiecePosition move)
        {
            for(int i = 0; i < _board.RowColLen; i++)
            { 
                for(int j = 0; j < _board.RowColLen; j++)
                {
                    var piece = _board[new PiecePosition(i,j)];
                    if (piece != null && piece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        PossibleMoves.RemoveAll(m => piece.PossibleMoves.Contains(m));
                    }
                }
            }
        }
    }
}