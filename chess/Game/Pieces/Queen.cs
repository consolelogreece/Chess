using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Queen : Piece
    {

        public Queen(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Queen", 90)
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<IMove>();

            var pos = this.CurrentPosition;

            while (++pos.row < _board.RowColLen && --pos.col >= 0)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
            }

            //reset
            pos = this.CurrentPosition;

            while (--pos.row >= 0 && --pos.col >= 0)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(new Move(_board[pos], this));

                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
            }

            //reset
            pos = this.CurrentPosition;

            while (--pos.row >= 0 && ++pos.col < _board.RowColLen)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
            }

            //reset
            pos = this.CurrentPosition;

            while (++pos.row < _board.RowColLen && ++pos.col < _board.RowColLen)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
            }

            //reset
            pos = this.CurrentPosition;

            while (++pos.col < _board.RowColLen)
            {
                if (_board[pos].OccupyingPiece != null)
                {
                    if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
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
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
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
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
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
                        possibleMoves.Add(new Move(_board[pos], this));
                    }

                    break;
                }

                possibleMoves.Add(new Move(_board[pos], this));
            }

            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }

        public override List<BoardTile> XRay(Piece target)
        {
            var pos = this.CurrentPosition;

            
            List<BoardTile> tiles = new List<BoardTile>();

            // note: order of the checks here are important. if it were reversed, horizontal/vertical pins would trip diagonal pin checks.
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
            else if (pos.row < target.CurrentPosition.row)
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
                   tiles =MoveHelpers.XRayDiagonalBRTL(this, target);
                }
            }

            return tiles;
        }   
    }
}