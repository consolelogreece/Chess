using System.Collections.Generic;
using Chess.Pieces;

namespace Chess.Helpers.Moves
{
    public static class PinHelpers
    {
        public static bool IsPinned(Piece target, PiecePosition currentTilePos)
        {
            var _board = target._board;

            var currentTile = _board[currentTilePos];

            var tiles = new List<BoardTile>();

            if (_board[currentTilePos].OccupyingPiece != null)
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
