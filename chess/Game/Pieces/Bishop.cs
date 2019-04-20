using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Bishop")
        {

        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = this.CurrentPosition;

            while (++copy.row < _board.RowColLen && --copy.col >= 0)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = this.CurrentPosition;

            while (--copy.row >= 0 && --copy.col >= 0)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = this.CurrentPosition;

            while (--copy.row >= 0 && ++copy.col < _board.RowColLen)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = this.CurrentPosition;

            while (++copy.row < _board.RowColLen && ++copy.col < _board.RowColLen)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }

        public override List<BoardTile> XRay(Piece target)
        {
            var pos = this.CurrentPosition;

            var tiles = new List<BoardTile>();

            if (pos.row < target.CurrentPosition.row)
            {
                if (pos.col < target.CurrentPosition.col)
                {
                    tiles = MoveHelpers.XRayDiagonalBLTR(this, target);
                }
                else
                {
                    tiles = MoveHelpers.XRayDiagonalTRBL(this, target);
                }
            }
            else
            {
                if (pos.col < target.CurrentPosition.col)
                {
                    tiles = MoveHelpers.XRayDiagonalTLBR(this, target); 
                }
                else
                {
                   tiles = MoveHelpers.XRayDiagonalBRTL(this, target);
                }
            }

            return tiles;
        }
    }
}