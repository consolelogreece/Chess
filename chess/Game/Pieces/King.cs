using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;

namespace Chess.Pieces
{
    public class King : Piece
    {
        private Dictionary<Move, List<BoardTile>> CastleMoves = new Dictionary<Move, List<BoardTile>>();

        private bool HasMoved = false;
        public King(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "King")
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<Move>();

            var castleMoves = new Dictionary<Move, List<BoardTile>>();

            #region surrounding 8 blocks
            var pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col);

            if (pos.row < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));
            
            pos = new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col + 1);

            if (pos.col< _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col - 1);

            if (pos.col >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col);

            if (pos.row >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col + 1);

            if (pos.row < _board.RowColLen && pos.col + 1 < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col - 1);

            if (pos.row < _board.RowColLen && pos.col - 1 >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col + 1);

            if (pos.row >= 0 && pos.col + 1 < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col - 1);

            if (pos.row >= 0 && pos.col - 1 >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));
            #endregion 

            // hasn't yet moved, so check for castles
            if (!this.HasMoved)
            {    
                var rooks = new List<Rook>();

                foreach(BoardTile tile in _board)
                    if (tile.OccupyingPiece?.PieceName == "Rook" && tile.OccupyingPiece?.PieceOwner == this.PieceOwner)
                        rooks.Add(tile.OccupyingPiece as Rook);

                foreach(var rook in rooks)
                {
                    if(rook.HasMoved) continue;

                    var isRookOnRight = this.CurrentPosition.col < rook.CurrentPosition.col;

                    var path = GetPath(rook);

                    if (path.Any(t => t.OccupyingPiece != null && t.OccupyingPiece != rook && t.OccupyingPiece != this)) continue;

                    var to = _board[new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col + (isRookOnRight ? 2 : -2))];

                    // we only want the path up to where the king will be, we dont want or need to check past that.
                    var trimmedPath = new List<BoardTile>();

                    var i = path.Count - 1;
                        
                    while(i >= 0)
                    {
                        if (path[i] != to) trimmedPath.Add(path[i]);
                        else 
                        {
                            trimmedPath.Add(path[i]);
                            break;
                        }

                        i--;
                    }

                    var move = new Move(to, this);

                    move.SetConsequence(new Action(() => 
                    {
                         var rookPosOffset = this.CurrentPosition.col < rook.CurrentPosition.col ? -1 : 1;

                        _board[this.CurrentPosition].OccupyingPiece = null;
                        _board[to.Position].OccupyingPiece = this;
                        this.CurrentPosition = to.Position;
     
                        _board[rook.CurrentPosition].OccupyingPiece = null;
                        var newRookPos = new PiecePosition(rook.CurrentPosition.row, to.Position.col + rookPosOffset);
                        _board[newRookPos].OccupyingPiece = rook;
                        rook.CurrentPosition = newRookPos;
                    }));

                    castleMoves.Add(move, trimmedPath);
                }
            }

            PossibleMoves = possibleMoves;
            CastleMoves = castleMoves;

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

            var castleMovesToRemove = new List<Move>();

            foreach(var move in CastleMoves)
            {
                if (move.Value.Any(bt => bt.ThreateningPieces.Any(p => p.PieceOwner != this.PieceOwner)))
                {
                    castleMovesToRemove.Add(move.Key);
                }
            }

            foreach(var move in castleMovesToRemove) CastleMoves.Remove(move);
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

            var path = GetPath(checkingPieces[0]);

            return path;
        }

        private List<BoardTile> GetPath(Piece piece)
        {
            var path = piece.XRay(this);

            var i = path.Count - 1;

            while (i >= 0)
            {
                if (path[i].OccupyingPiece != this) path.RemoveAt(i);
                else break;
                i--;
            }

            return path;
        }

        public override List<Move> GetMoves()
        {
            var moves = PossibleMoves.Concat(CastleMoves.Select(m => m.Key)).ToList();

            return moves;
        }

        public override void ClearMoves()
        {
            PossibleMoves.Clear();
            CastleMoves.Clear();
        }

        public override bool Move(PiecePosition movePos)
        {
            var wasSuccessful = base.Move(movePos);

            if (wasSuccessful) this.HasMoved = true;

            return wasSuccessful;
        }
    }
}