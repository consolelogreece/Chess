using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class Knight : Piece
    {
        public Knight(Player pieceOwner, Board board)
            : base(pieceOwner, board, "knight")
        {
        }

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

            copy.row += 2;

            if (copy.row < _board.RowColLen)
            {
                copy.col += 1;

                if (copy.col < _board.RowColLen)
                {
                    possibleMoves.Add(copy);
                }

                copy.col -= 2;

                if (copy.col >= 0)
                {
                    possibleMoves.Add(copy);
                }
            }

            //reset
            copy = piecePosition;

            copy.row -= 2;

            if (copy.row >= 0)
            {
                copy.col += 1;

                if (copy.col < _board.RowColLen)
                {
                    possibleMoves.Add(copy);
                }

                copy.col -= 2;

                if (copy.col >= 0)
                {
                    possibleMoves.Add(copy);
                }
            }

            //reset
            copy = piecePosition;

            copy.col -= 2;

            if (copy.col >= 0)
            {
                copy.row += 1;

                if (copy.row < _board.RowColLen)
                {
                    possibleMoves.Add(copy);
                }

                copy.row -= 2;

                if (copy.row >= 0)
                {
                    possibleMoves.Add(copy);
                }
            }

            //reset
            copy = piecePosition;

            copy.col += 2;

            if (copy.col < _board.RowColLen)
            {
                copy.row += 1;

                if (copy.row < _board.RowColLen)
                {
                    possibleMoves.Add(copy);
                }

                copy.row -= 2;

                if (copy.row >= 0)
                {
                    possibleMoves.Add(copy);
                }
            }

            PossibleMoves = possibleMoves;

            return true;
        }
    }
}