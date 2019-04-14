using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class Rook : Piece
    {
        public Rook(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Rook")
        {
        }

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

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

        public override void EliminateIllegalMoves()
        {
            foreach(var piece in _board[this.CurrentPosition].ThreateningPieces)
            {
                // if this piece is pinned, remove any spaces it cant move to.
                piece.StripIllegalMovesPin(this);
            }
        }
        public override void StripIllegalMovesPin(Piece piece)
        {
            if (this.CurrentPosition.row == piece.CurrentPosition.row)
            {
                if (this.CurrentPosition.col < piece.CurrentPosition.col)
                {
                    var pos = this.CurrentPosition;

                    while(++pos.col < _board.RowColLen)
                    {
                        var currentTile = _board[pos];
                        if (_board[pos].OccupyingPiece != null )
                        {
                            // ignore piece if its the piece we're checking for pins
                            if (_board[pos].OccupyingPiece != piece)
                            {
                                if (_board[pos].OccupyingPiece.PieceName == "King" && _board[pos].OccupyingPiece.PieceOwner.Id == piece.PieceOwner.Id)                               
                                {
                                    piece.PossibleMoves.RemoveAll(m => m.row != piece.CurrentPosition.row);
                                }

                                return;
                            }
                        }
                    }
                }
                else
                {

                }
            }

        }
    } 
}