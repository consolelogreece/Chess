using System;
using System.Collections.Generic;
using Chess;
using Chess.Pieces;

// todo: change from static and pass in to classes via DI.

namespace Chess.Helpers
{
    public static class MoveHelpers
    {
        public static List<BoardTile> XRayHorizontalLR(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while(pos.col < _board.RowColLen)
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

            while(pos.col >= 0)
            {
                tiles.Add(_board[pos]);

                pos.col--;
            }

            return tiles;
        }

        // NOTE: when printed, the board is flipped veritcally, techincally rows go from top to bottom. This is why this method is UP-DOWN.z
        public static List<BoardTile> XRayVerticalUD(Piece pinner, Piece target)
        {
            var _board = target._board;

            var pos = pinner.CurrentPosition;

            var tiles = new List<BoardTile>();

            while(pos.row < _board.RowColLen)
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

            while(pos.row >= 0)
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

            while(pos.row >= 0 && pos.col < _board.RowColLen)
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

            while(pos.row >= 0 && pos.col >= 0)
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

            while(pos.row < _board.RowColLen && pos.col >= 0)
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

            while(pos.row < _board.RowColLen && pos.col < _board.RowColLen)
            {
                tiles.Add(_board[pos]);

                pos.row++;
                pos.col++;
            }

            return tiles;
        }

        public static bool IsPinned (Piece target, PiecePosition currentTilePos)
        {
            var _board = target._board;

            var currentTile = _board[currentTilePos];

            var tiles = new List<BoardTile>();

            if (_board[currentTilePos].OccupyingPiece != null )
            {
                // ignore piece if its the piece we're checking for pins
                if (_board[currentTilePos].OccupyingPiece != target)
                {
                    if (_board[currentTilePos].OccupyingPiece.PieceName == "King" && _board[currentTilePos].OccupyingPiece.PieceOwner.Id == target.PieceOwner.Id)                               
                    {
                        return true;
                    }

                    return false;
                }
            }
            return false;
        }
    }
}