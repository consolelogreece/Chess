using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Helpers;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(Player pieceOwner, Board board, PiecePosition startingPosition) : base(pieceOwner, board, startingPosition, "♞", "Knight", 30) { }

        protected override void GenBoardValueTable()
        {
            var boardValueTable = new float[8, 8]
            { {-5, -4, -3, -3, -3, -3, -4, -5 }, 
            {-4, -2, 0, 0, 0, 0, -2, -4 }, 
            {-3, 0, 1, 1.5f, 1.5f, 1, 0, -3 }, 
            {-3, 0.5f, 1.5f, 2, 2, 1.5f, 0.5f, -3 }, 
            {-3, 0, 1.5f, 2, 2, 1.5f, 0, -3 }, 
            {-3, 0.5f, 1, 1.5f, 1.5f, 1, 0.5f, -3 }, 
            {-4, -2, 0, 0.5f, 0.5f, 0, -2, -4 }, 
            {-5, -4, -3, -3, -3, -3, -4, -5 },
            };

            if (this.PieceOwner.Side == "top")
            {
                var actual = new float[8, 8];

                for (int i = 7; i >= 0; i--)
                {
                    for (int j = 7; j >= 0; j--)
                    {
                        actual[7 - i, 7 - j] = boardValueTable[i, j];
                    }
                }

                boardValueTable = actual;
            }

            this.BoardValueTable = boardValueTable;
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
