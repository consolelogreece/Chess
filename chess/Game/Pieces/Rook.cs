using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Rook")
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = this.CurrentPosition;

            while (++copy.col < _board.RowColLen)
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

            while (--copy.col >= 0)
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

            while (--copy.row >= 0)
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

            while (++copy.row < _board.RowColLen)
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

            List<BoardTile> tiles = new List<BoardTile>();

            if (pos.row == target.CurrentPosition.row)
            {
                if (pos.col < target.CurrentPosition.col)
                {
                    tiles = MoveHelpers.XRayHorizontalLR(this, target);
                }
                else
                {
                    tiles = MoveHelpers.XRayHorizontalRL(this, target);
                }
            }
            else if(pos.col == target.CurrentPosition.col)
            {
                if (pos.row < target.CurrentPosition.row)
                {
                    tiles = MoveHelpers.XRayVerticalUD(this, target);
                }
                else
                {
                   tiles = MoveHelpers.XRayVerticalDU(this, target); 
                }
            }

            return tiles;
        }
    } 
}