using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;

namespace Chess.Pieces
{
    public class King : Piece
    {
        public King(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "King")
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<Move>();

            //new Move(_board[new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col)], new Action(() => MoveHelpers.MoveNormal(this, _board[])));

            var pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col);

            if (pos.row + 1 < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));
            
            pos = new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col + 1);

            if (pos.col + 1 < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col - 1);

            if (pos.col - 1 >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col);

            if (pos.row - 1 >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col + 1);

            if (pos.row + 1 < _board.RowColLen && pos.col + 1 < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col - 1);

            if (pos.row + 1 < _board.RowColLen && pos.col - 1 >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col + 1);

            if (pos.row - 1 >= 0 && pos.col + 1 < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col - 1);

            if (pos.row - 1 >= 0 && pos.col - 1 >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));
                
            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }

        public override void EliminateIllegalMoves()
        {
            PossibleMoves.RemoveAll(m => _board[m.To.Position].ThreateningPieces.Any(p => p.PieceOwner.Id != this.PieceOwner.Id));

            foreach(var threateningPiece in _board[this.CurrentPosition].ThreateningPieces)
            {
                var tiles = threateningPiece.XRay(this);

                this.PossibleMoves.RemoveAll(m => tiles.Any(t => t.Position == m.To.Position));
            }
        }

        public bool IsInCheckmate()
        {
            var checkingPieces = _board[this.CurrentPosition].ThreateningPieces;

            // if there are possible moves, can't be checkmate so no point making further checks.
            if (this.PossibleMoves.Count > 0) return false;

            // if there are no moves, and more than on checking piece, no blocks/takes will save from mate.
            if (checkingPieces.Count > 1)
            {
                return true;
            }

            var posOfChecker = checkingPieces[0].CurrentPosition;

            var tileOfChecker = _board[posOfChecker];

            // see if the checker can be taken by any of the threatening pieces
            if (tileOfChecker.ThreateningPieces.Any(m => m.PossibleMoves.Any(pm => pm.To.Position == posOfChecker))) return false;

            var checkPath = tileOfChecker.OccupyingPiece.XRay(this);

            if (checkPath.Any(m => m.ThreateningPieces.Any(p => p.PieceOwner.Id == this.PieceOwner.Id && p.PossibleMoves.Any(x => x.To.Position == (m.Position)))))
            {
                return false;
            }

            return true;
        }

        public List<BoardTile> GetCheckPath()
        {
            var checkingPieces = _board[this.CurrentPosition].ThreateningPieces;

            var path = checkingPieces[0].XRay(this);

            var i = path.Count - 1;

            // as xray returns the pieces after the king, we want to remove those extra tiles as they are not part of the check path.
            while (i >= 0)
            {
                if (path[i].OccupyingPiece != this) path.RemoveAt(i);
                else break;
                i--;
            }

            return path;
        }
    }
}