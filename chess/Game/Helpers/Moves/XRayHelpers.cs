using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Helpers.Moves
{
    public static class XRayHelpers
    {
        public static List<BoardTile> XRayHorizontalLR(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.col < _board.RowColLen)
            {
                tiles.Add(_board[pos]);

                pos.col++;
            }

            return tiles;
        }
        public static List<BoardTile> XRayHorizontalRL(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.col >= 0)
            {
                tiles.Add(_board[pos]);

                pos.col--;
            }

            return tiles;
        }

        // NOTE: when printed, the board is flipped veritcally, techincally rows go from top to bottom. This is why this method is UP-DOWN.
        public static List<BoardTile> XRayVerticalUD(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.row < _board.RowColLen)
            {
                tiles.Add(_board[pos]);

                pos.row++;
            }

            return tiles;
        }

        public static List<BoardTile> XRayVerticalDU(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.row >= 0)
            {
                tiles.Add(_board[pos]);

                pos.row--;
            }

            return tiles;
        }

        public static List<BoardTile> XRayDiagonalTLBR(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.row >= 0 && pos.col < _board.RowColLen)
            {
                tiles.Add(_board[pos]);

                pos.row--;
                pos.col++;
            }

            return tiles;
        }

        public static List<BoardTile> XRayDiagonalBRTL(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.row >= 0 && pos.col >= 0)
            {
                tiles.Add(_board[pos]);

                pos.row--;
                pos.col--;
            }

            return tiles;
        }

        public static List<BoardTile> XRayDiagonalTRBL(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.row < _board.RowColLen && pos.col >= 0)
            {
                tiles.Add(_board[pos]);

                pos.row++;
                pos.col--;
            }

            return tiles;
        }

        public static List<BoardTile> XRayDiagonalBLTR(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while (pos.row < _board.RowColLen && pos.col < _board.RowColLen)
            {
                tiles.Add(_board[pos]);

                pos.row++;
                pos.col++;
            }

            return tiles;
        }
    }
}