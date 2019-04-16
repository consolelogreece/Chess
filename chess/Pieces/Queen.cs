using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class Queen : Piece
    {

        public Queen(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Queen")
        {
        }

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

            while (++copy.row < _board.RowColLen && --copy.col >= 0)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (--copy.row >= 0 && --copy.col >= 0)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (--copy.row >= 0 && ++copy.col < _board.RowColLen)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (++copy.row < _board.RowColLen && ++copy.col < _board.RowColLen)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (++copy.col < _board.RowColLen)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (--copy.col >= 0)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (--copy.row >= 0)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            //reset
            copy = piecePosition;

            while (++copy.row < _board.RowColLen)
            {
                if (_board[copy].OccupyingPiece != null)
                {
                    if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        // player can take this piece, so it is therefore a possible move.
                        possibleMoves.Add(copy);
                        //_board[copy].OccupyingPiece.RegisterThreat(this);
                    }

                    break;
                }

                possibleMoves.Add(copy);
            }

            PossibleMoves = possibleMoves;

            return base.CalculateMoves(piecePosition);
        }

        public override void StripIllegalMovesPin(Piece piece)
        {
            var helper = new MoveHelpers();

            var pos = this.CurrentPosition;

            // note: order of the checks here are important. if it were reversed, horizontal/vertical pins would trip diagonal pin checks.
            if (pos.row == piece.CurrentPosition.row)
            {
                if (pos.col < piece.CurrentPosition.col)
                {
                    helper.StripMovesPinnedHorizontalLR(this, piece);
                }
                else
                {
                    helper.StripMovesPinnedHorizontalRL(this, piece);
                }
            }
            else if(pos.col == piece.CurrentPosition.col)
            {
                if (pos.row < piece.CurrentPosition.row)
                {
                    helper.StripMovesPinnedVerticalUD(this, piece);
                }
                else
                {
                   helper.StripMovesPinnedVerticalDU(this, piece); 
                }
            }
            else if (pos.row < piece.CurrentPosition.col)
            {
                if (pos.col < piece.CurrentPosition.col)
                {
                    helper.StripMovesPinnedDiagonalBLTR(this, piece);
                }
                else
                {
                    helper.StripMovesPinnedDiagonalTRBL(this, piece);
                }
            }
            else
            {
                if (pos.col < piece.CurrentPosition.col)
                {
                    helper.StripMovesPinnedDiagonalTLBR(this, piece); 
                }
                else
                {
                   helper.StripMovesPinnedDiagonalBRTL(this, piece);
                }
            }
        }   
    }
}