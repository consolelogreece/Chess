using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Helpers;
using Chess.Helpers.Moves;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(Player pieceOwner, Board board, PiecePosition startingPosition) : base(pieceOwner, board, startingPosition, "♛", "Queen", 90) { }

        protected override void GenBoardValueTable()
        {
            var boardValueTable = new float[8, 8]
            { 
                {-2, -1, -1, -0.5f, -0.5f, -1, -1, -2 }, 
                {-1, 0, 0, 0, 0, 0, 0, -1 }, 
                {-1, 0, 0.5f, 0.5f, 0.5f, 0.5f, 0, -1 }, 
                {-0.5f, 0, 0.5f, 0.5f, 0.5f, 0.5f, 0, -0.5f }, 
                { 0, 0, 0.5f, 0.5f, 0.5f, 0.5f, 0, -0.5f }, 
                {-1, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0, -1 }, 
                {-1, 0, 0.5f, 0, 0, 0, 0, -1 }, 
                {-2, -1, -1, -0.5f, -0.5f, -1, -1, -2 },
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
                    tiles = XRayHelpers.XRayHorizontalLR(this, target);

                }
                else
                {
                    tiles = XRayHelpers.XRayHorizontalRL(this, target);
                }
            }
            else if (pos.col == target.CurrentPosition.col)
            {
                if (pos.row < target.CurrentPosition.row)
                {
                    tiles = XRayHelpers.XRayVerticalUD(this, target);
                }
                else
                {
                    tiles = XRayHelpers.XRayVerticalDU(this, target);
                }
            }
            else if (pos.row < target.CurrentPosition.row)
            {
                if (pos.col < target.CurrentPosition.col)
                {
                    tiles = XRayHelpers.XRayDiagonalBLTR(this, target);
                }
                else
                {
                    tiles = XRayHelpers.XRayDiagonalTRBL(this, target);
                }
            }
            else
            {
                if (pos.col < target.CurrentPosition.col)
                {
                    tiles = XRayHelpers.XRayDiagonalTLBR(this, target);
                }
                else
                {
                    tiles = XRayHelpers.XRayDiagonalBRTL(this, target);
                }
            }

            return tiles;
        }
    }
}
