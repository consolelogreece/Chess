using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Pieces
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
