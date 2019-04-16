using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public struct PiecePosition
    {
        public PiecePosition(int row, int col)
        {
            this.row = row;

            this.col = col;
        }

        public static bool operator ==(PiecePosition first, PiecePosition second)
        {
            return (first.row == second.row && first.col == second.col);
        }
        public static bool operator !=(PiecePosition first, PiecePosition second)
        {
            return (first == second) == false;
        }

        public int row;
        public int col;
    }
}