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
                            possibleMoves.Add(pos);
                        }                        
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        }                   
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        } 
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        }           
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        }          
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        }
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        }
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
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
                            possibleMoves.Add(pos);
                        }     
                    }
                    else
                    {
                       possibleMoves.Add(pos); 
                    }    
                }
            }

            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }
    }
}