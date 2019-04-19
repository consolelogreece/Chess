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

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
            copy = piecePosition;

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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
            copy = piecePosition;

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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
            copy = piecePosition;

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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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
                            //_board[copy].OccupyingPiece.RegisterThreat(this);
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

            return base.CalculateMoves(piecePosition);
        }
    }
}