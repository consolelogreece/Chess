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

            var pos = this.CurrentPosition;

            while (++pos.col < _board.RowColLen)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(pos);
                    }

                    break;
                }

                possibleMoves.Add(pos);
            }

            //reset
            pos = this.CurrentPosition;

            while (--pos.col >= 0)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(pos);
                    }

                    break;
                }

                possibleMoves.Add(pos);
            }

            //reset
            pos = this.CurrentPosition;

            while (--pos.row >= 0)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(pos);
                    }

                    break;
                }

                possibleMoves.Add(pos);
            }

            //reset
            pos = this.CurrentPosition;

            while (++pos.row < _board.RowColLen)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(pos);
                    }

                    break;
                }

                possibleMoves.Add(pos);
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