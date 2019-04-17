using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class King : Piece
    {
        // NOTE. WHEN CHECKING FOR CHECKMATE. IF THE KING IS IN DOUBLE CHECK, AND THERE ARE NO PLACES IT CAN MOVE, IT IS CHECKMATE. NO NEED TO CHECK FOR BLOCKS/TAKES ETC.
        public King(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "King")
        {
        }

        public override bool CalculateMoves(PiecePosition move)
        {
            var possibleMoves = new List<PiecePosition>();

            if (move.row + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col)].OccupyingPiece == null || _board[new PiecePosition(move.row + 1, move.col)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col));

            if (move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row , move.col + 1)].OccupyingPiece == null || _board[new PiecePosition(move.row, move.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row, move.col + 1));

            if (move.col - 1 >= 0 && (_board[new PiecePosition(move.row, move.col - 1)].OccupyingPiece == null || _board[new PiecePosition(move.row, move.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row, move.col - 1));

            if (move.row - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col)].OccupyingPiece == null || _board[new PiecePosition(move.row - 1, move.col)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col));

            if (move.row + 1 < _board.RowColLen && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col + 1)].OccupyingPiece == null || _board[new PiecePosition(move.row + 1, move.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col + 1));

            if (move.row + 1 < _board.RowColLen && move.col - 1 >= 0 && (_board[new PiecePosition(move.row + 1, move.col - 1)].OccupyingPiece == null || _board[new PiecePosition(move.row + 1, move.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row + 1, move.col - 1));

            if (move.row - 1 >= 0 && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row - 1, move.col + 1)].OccupyingPiece == null || _board[new PiecePosition(move.row - 1, move.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col + 1));

            if (move.row - 1 >= 0 && move.col - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col - 1)].OccupyingPiece == null || _board[new PiecePosition(move.row - 1, move.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id))
                possibleMoves.Add(new PiecePosition(move.row - 1, move.col - 1));

            PossibleMoves = possibleMoves;

            return base.CalculateMoves(move);
        }


        // TODO: currently the king thinks it can move because the tile opposite a checking piece technically isnt threatened. will need to xray to remove all those options too.
        // for the todo above, consider having xray in its own function. it takes a starting position and a direction and returns all squares until it finds an occupied one and maybe ignores kings? idk
        public override void EliminateIllegalMoves()
        {
            PossibleMoves.RemoveAll(m => _board[new PiecePosition(m.row, m.col)].ThreateningPieces.Any(p => p.PieceOwner.Id != this.PieceOwner.Id));

            // base.EliminateIllegalMoves();

            foreach(var threateningPiece in _board[this.CurrentPosition].ThreateningPieces)
            {
                var tiles = threateningPiece.XRay(this);

                this.PossibleMoves.RemoveAll(m => tiles.Any(t => t.Position == m));
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
            if (tileOfChecker.ThreateningPieces.Any(m => m.PossibleMoves.Any(pm => pm == posOfChecker))) return false;

            // todo: check for blocks.
            var checkPath = tileOfChecker.OccupyingPiece.XRay(this);

            if (checkPath.Any(m => m.ThreateningPieces.Any(p => p.PieceOwner.Id == this.PieceOwner.Id && p.PossibleMoves.Contains(m.Position))))
            {
                return false;
            }

            return true;
        }
    }
}