using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "knight")
        {
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = this.CurrentPosition;

            copy.row += 2;

            if (copy.row < _board.RowColLen)
            {
                copy.col += 1;

                if (copy.col < _board.RowColLen)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        }                        
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }

                copy.col -= 2;

                if (copy.col >= 0)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        }                   
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }
            }

            //reset
            copy = this.CurrentPosition;

            copy.row -= 2;

            if (copy.row >= 0)
            {
                copy.col += 1;

                if (copy.col < _board.RowColLen)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        } 
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }

                copy.col -= 2;

                if (copy.col >= 0)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        }           
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }
            }

            //reset
            copy = this.CurrentPosition;

            copy.col -= 2;

            if (copy.col >= 0)
            {
                copy.row += 1;

                if (copy.row < _board.RowColLen)
                {
                   if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        }          
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }

                copy.row -= 2;

                if (copy.row >= 0)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.  
                            possibleMoves.Add(copy);
                        }
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }
            }

            //reset
            copy = this.CurrentPosition;

            copy.col += 2;

            if (copy.col < _board.RowColLen)
            {
                copy.row += 1;

                if (copy.row < _board.RowColLen)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        }
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }

                copy.row -= 2;

                if (copy.row >= 0)
                {
                    if (_board[copy].OccupyingPiece != null)
                    {
                        if (_board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                        {
                            // player can take this piece, so it is therefore a possible move.
                            possibleMoves.Add(copy);
                        }     
                    }
                    else
                    {
                       possibleMoves.Add(copy); 
                    }    
                }
            }

            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }
    }
}