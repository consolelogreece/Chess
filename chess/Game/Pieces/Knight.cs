using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "knight", 30)
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<IMove>();

            var pos = this.CurrentPosition;

            pos.row += 2;

            if (pos.row < _board.RowColLen)
            {
                pos.col += 1;

                if (pos.col < _board.RowColLen)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        }                        
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }

                pos.col -= 2;

                if (pos.col >= 0)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        }                   
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }
            }

            //reset
            pos = this.CurrentPosition;

            pos.row -= 2;

            if (pos.row >= 0)
            {
                pos.col += 1;

                if (pos.col < _board.RowColLen)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        } 
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }

                pos.col -= 2;

                if (pos.col >= 0)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        }           
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }
            }

            //reset
            pos = this.CurrentPosition;

            pos.col -= 2;

            if (pos.col >= 0)
            {
                pos.row += 1;

                if (pos.row < _board.RowColLen)
                {
                   if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        }          
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }

                pos.row -= 2;

                if (pos.row >= 0)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.  
                            possibleMoves.Add(new Move(_board[pos], this));
                        }
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }
            }

            //reset
            pos = this.CurrentPosition;

            pos.col += 2;

            if (pos.col < _board.RowColLen)
            {
                pos.row += 1;

                if (pos.row < _board.RowColLen)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        }
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }

                pos.row -= 2;

                if (pos.row >= 0)
                {
                    if (_board[pos].OccupyingPiece != null)
                    {
                        if (_board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(new Move(_board[pos], this));
                        }     
                    }
                    else
                    {
                       possibleMoves.Add(new Move(_board[pos], this)); 
                    }    
                }
            }

            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }
    }
}