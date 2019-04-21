using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public bool HasMoved { get; private set; } = false;
        
        public Rook(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Rook")
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<Move>();

            var pos = this.CurrentPosition;

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

        public override bool Move(PiecePosition movePos)
        {
            var wasSuccessful = base.Move(movePos);

            if (wasSuccessful) this.HasMoved = true;

            return wasSuccessful;
        }
    } 
}