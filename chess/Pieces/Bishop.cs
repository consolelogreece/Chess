using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player pieceOwner, Board board)
        : base(pieceOwner, board, "Bishop")
        {

        }

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

            while (++copy.row < _board.RowColLen && --copy.col >= 0)
            {
                if (_board[copy] != null)
                {
                    if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (--copy.row >= 0 && --copy.col >= 0)
            {
                if (_board[copy] != null)
                {
                    if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (--copy.row >= 0 && ++copy.col < _board.RowColLen)
            {
                if (_board[copy] != null)
                {
                    if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (++copy.row < _board.RowColLen && ++copy.col < _board.RowColLen)
            {
                if (_board[copy] != null)
                {
                    if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            PossibleMoves = possibleMoves;

            return true;
        }
    }
}