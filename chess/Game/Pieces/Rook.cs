using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Helpers;
using Chess.Helpers.Moves;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(Player pieceOwner, Board board, PiecePosition startingPosition) : base(pieceOwner, board, startingPosition, "♜", "Rook", 50) { }

        protected override void GenBoardValueTable()
        {
            var boardValueTable = new float[8, 8]
            { 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0.5f, 1, 1, 1, 1, 1, 1, 0.5f }, 
                {-0.5f, 0, 0, 0, 0, 0, 0, -0.5f }, 
                {-0.5f, 0, 0, 0, 0, 0, 0, -0.5f }, 
                {-0.5f, 0, 0, 0, 0, 0, 0, -0.5f }, 
                {-0.5f, 0, 0, 0, 0, 0, 0, -0.5f }, 
                {-0.5f, 0, 0, 0, 0, 0, 0, -0.5f }, 
                { 0, 0, 0, 0.5f, 0.5f, 0, 0, 0 }
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

            return tiles;
        }
    }
}
