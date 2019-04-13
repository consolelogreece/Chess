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

        public int row;
        public int col;
    }
}