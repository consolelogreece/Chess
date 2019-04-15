using chess.pieces;

public class MoveHelpers
{
    //TODO: start from pinned piece, not pinner.
    public void StripMovesPinnedHorizontalLR(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;

        var pos = pinner.CurrentPosition;

        while(++pos.col < _board.RowColLen)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.row != pinned.CurrentPosition.row);
            }
        }
    }

    public void StripMovesPinnedHorizontalRL(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;

        var pos = pinner.CurrentPosition;

        while(--pos.col >= 0)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.row != pinned.CurrentPosition.row);
            }
        }
    }

    // NOTE: when printed, the board is flipped veritcally, techincally rows go from top to bottom. This is why this method is UP-DOWN.z
    public void StripMovesPinnedVerticalUD(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;

        var pos = pinner.CurrentPosition;

        while(++pos.row < _board.RowColLen)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.col != pinned.CurrentPosition.col);
            }
        }
    }

    public void StripMovesPinnedVerticalDU(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;
        
        var pos = pinner.CurrentPosition;

        while(--pos.row < _board.RowColLen)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.col != pinned.CurrentPosition.col);
            }
        }
    }

    public void StripMovesPinnedDiagonalTLBR(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;
        
        var pos = pinner.CurrentPosition;

        while(++pos.row < _board.RowColLen && ++pos.col < _board.RowColLen)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.col - m.row != pinner.CurrentPosition.col - pinner.CurrentPosition.row);
            }
        }
    }

    public void StripMovesPinnedDiagonalBRTL(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;
        
        var pos = pinner.CurrentPosition;

        while(--pos.row >= 0 && --pos.col >= 0)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.col - m.row != pinner.CurrentPosition.col - pinner.CurrentPosition.row);
            }
        }
    }

    public void StripMovesPinnedDiagonalTRBL(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;
        
        var pos = pinner.CurrentPosition;

        while(++pos.row < _board.RowColLen && --pos.col >= 0)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.col - m.row != pinner.CurrentPosition.col - pinner.CurrentPosition.row);
            }
        }
    }

     public void StripMovesPinnedDiagonalBLTR(Piece pinner, Piece pinned)
    {
        var _board = pinned._board;
        
        var pos = pinner.CurrentPosition;

        while(--pos.row >= 0 && ++pos.col < _board.RowColLen)
        {
            if (IsPinned(pinned, pos))
            {
                pinned.PossibleMoves.RemoveAll(m => m.col - m.row != pinner.CurrentPosition.col - pinner.CurrentPosition.row);
            }
        }
    }

    private bool IsPinned (Piece pinned, PiecePosition currentTilePos)
    {
        var _board = pinned._board;

        var currentTile = _board[currentTilePos];

        if (_board[currentTilePos].OccupyingPiece != null )
        {
            // ignore piece if its the piece we're checking for pins
            if (_board[currentTilePos].OccupyingPiece != pinned)
            {
                if (_board[currentTilePos].OccupyingPiece.PieceName == "King" && _board[currentTilePos].OccupyingPiece.PieceOwner.Id == pinned.PieceOwner.Id)                               
                {
                    return true;
                }

                return false;
            }
        }
        return false;
    }
}