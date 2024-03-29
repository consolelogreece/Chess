using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Helpers;
using Chess.Moves;

namespace Chess.Pieces
{
    public class King : Piece
    {
        private Dictionary<IMove, List<BoardTile>> CastleMoves = new Dictionary<IMove, List<BoardTile>>();

        public King(Player pieceOwner, Board board, PiecePosition startingPosition) : base(pieceOwner, board, startingPosition, "K", "King", 900) { }

        protected override void GenBoardValueTable()
        {
            var boardValueTable = new float[8, 8]
            { 
                {-3, -4, -4, -5, -5, -4, -4, -3 }, 
                {-3, -4, -4, -5, -5, -4, -4, -3 }, 
                {-3, -4, -4, -5, -5, -4, -4, -3 }, 
                {-3, -4, -4, -5, -5, -4, -4, -3 },
                {-2, -3, -3, -4, -4, -2, -2, -1 }, 
                {-1, -2, -2, -2, -2, -2, -2, -1 }, 
                { 2, 2, 0, 0, 0, 0, 2, 2 }, 
                { 2, 3, 1, 0, 0, 1, 3, 0 }
            };

            if (this.PieceOwner.Side == "top")
            {
                var actual = new float[8, 8];

                for (int i = 7; i >= 0; i--)
                {
                    for (int j = 7; j >= 0; j--)
                    {
                        actual[7 - i, 7 - j] = boardValueTable[i, j];
                    }
                }

                boardValueTable = actual;
            }

            this.BoardValueTable = boardValueTable;
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<IMove>();

            var castleMoves = new Dictionary<IMove, List<BoardTile>>();

            #region surrounding 8 blocks
            var pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col);

            if (pos.row < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col + 1);

            if (pos.col < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col - 1);

            if (pos.col >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col);

            if (pos.row >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col + 1);

            if (pos.row < _board.RowColLen && pos.col < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row + 1, this.CurrentPosition.col - 1);

            if (pos.row < _board.RowColLen && pos.col >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col + 1);

            if (pos.row >= 0 && pos.col < _board.RowColLen && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));

            pos = new PiecePosition(this.CurrentPosition.row - 1, this.CurrentPosition.col - 1);

            if (pos.row >= 0 && pos.col >= 0 && (_board[pos].OccupyingPiece == null || _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new Move(_board[pos], this));
            #endregion 

            // hasn't yet moved, so check for castles
            if (this.TimesMoved == 0)
            {
                var rooks = _board.GetPieces(p => p.PieceName == "Rook", p => p.PieceOwner == this.PieceOwner);

                foreach (Rook rook in rooks)
                {
                    if (rook.TimesMoved != 0)continue;

                    var isRookOnRight = this.CurrentPosition.col < rook.CurrentPosition.col;

                    var path = GetPath(rook);

                    if (path.Any(t => t.OccupyingPiece != null && t.OccupyingPiece != rook && t.OccupyingPiece != this))continue;

                    var to = _board[new PiecePosition(this.CurrentPosition.row, this.CurrentPosition.col + (isRookOnRight ? 2 : -2))];

                    // we only want the path up to where the king will be, we dont want or need to check past that.
                    var trimmedPath = new List<BoardTile>();

                    var i = path.Count - 1;

                    while (i >= 0)
                    {
                        if (path[i] != to)trimmedPath.Add(path[i]);
                        else
                        {
                            trimmedPath.Add(path[i]);
                            break;
                        }

                        i--;
                    }

                    var move = new Castle(to, this, rook);

                    castleMoves.Add(move, trimmedPath);
                }
            }

            PossibleMoves = possibleMoves;
            CastleMoves = castleMoves;

            return base.CalculateMoves();
        }

        public override void EliminateIllegalMoves()
        {
            PossibleMoves.RemoveAll(m => m.GetMovePos().ThreateningPieces.Any(p => p.PieceOwner != this.PieceOwner));

            foreach (var threateningPiece in _board[this.CurrentPosition].ThreateningPieces)
            {
                var tiles = threateningPiece.XRay(this);

                this.PossibleMoves.RemoveAll(m => tiles.Any(t => t.Position == m.GetMovePos().Position));
            }

            var castleMovesToRemove = new List<IMove>();

            foreach (var move in CastleMoves)
            {
                if (move.Value.Any(bt => bt.ThreateningPieces.Any(p => p.PieceOwner != this.PieceOwner)))
                {
                    castleMovesToRemove.Add(move.Key);
                }
            }

            foreach (var move in castleMovesToRemove)CastleMoves.Remove(move);
        }

        public bool IsInCheckmate()
        {
            var checkingPieces = _board[this.CurrentPosition].ThreateningPieces;

            // if there are possible moves, can't be checkmate so no point making further checks.
            if (this.PossibleMoves.Count > 0)return false;

            // if there are no moves, and more than on checking piece, no blocks/takes will save from mate.
            if (checkingPieces.Count > 1)
            {
                return true;
            }

            var posOfChecker = checkingPieces[0].CurrentPosition;

            var tileOfChecker = _board[posOfChecker];

            // see if the checker can be taken by any of the threatening pieces
            if (tileOfChecker.ThreateningPieces.Any(m => m.PossibleMoves.Any(pm => pm.GetMovePos().Position == posOfChecker)))return false;

            var checkPath = tileOfChecker.OccupyingPiece.XRay(this);

            if (checkPath.Any(m => m.ThreateningPieces.Any(p => p.PieceOwner.Id == this.PieceOwner.Id && p.PossibleMoves.Any(x => x.GetMovePos().Position == (m.Position)))))
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
                if (path[i].OccupyingPiece != this)path.RemoveAt(i);
                else break;
                i--;
            }

            return path;
        }

        public override List<IMove> GetMoves()
        {
            var moves = PossibleMoves.Concat(CastleMoves.Select(m => m.Key)).ToList();

            return moves;
        }

        public override void ClearMoves(List<IMove> illegalMoves = null)
        {
            if (illegalMoves == null)
            {
                PossibleMoves.Clear();
                CastleMoves.Clear();
                return;
            }

            foreach(var move in illegalMoves)
            {
                if (move.GetMoveMeta().Contains("Castle"))
                    CastleMoves.Remove((Castle) move);

                else PossibleMoves.Remove(move);
            }
        }
    }
}
