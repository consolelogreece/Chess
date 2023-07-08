using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Helpers;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        public bool EnPassant = false;

        private int _direction;

        public Pawn(Player pieceOwner, Board board, PiecePosition startingPosition) : base(pieceOwner, board, startingPosition, "P", "Pawn", 10)
        {
            _direction = pieceOwner.Side == "top" ? -1 : 1;
        }

        protected override void GenBoardValueTable()
        {
            var boardValueTable = new float[8, 8]
            { 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 5, 5, 5, 5, 5, 5, 5, 5 }, 
                { 1, 1, 2, 3, 3, 2, 1, 1 }, 
                { 0.5f, 0.5f, 1, 2.5f, 2.5f, 1, 0.5f, 0.5f }, 
                { 0, 0, 0, 2, 2, 0, 0, 0 }, 
                { 0.5f, -0.5f, -1, 0, 0, -1, -0.5f, 0.5f }, 
                { 0.5f, 1, 1, -2, -2, 1, 1, 0.5f },
                 { 0, 0, 0, 0, 0, 0, 0, 0 }
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

        public override IMove Move(PiecePosition movePos)
        {
            if (Math.Abs(movePos.row - CurrentPosition.row) != 1)EnPassant = true;
            else EnPassant = false;

            var move = PossibleMoves.FirstOrDefault(m => m.GetMovePos().Position == movePos);

            if (move != null)
            {
                move.MakeMove();
            }

            return move;
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<IMove>();

            var pos = this.CurrentPosition;

            pos.row += _direction;

            if (pos.row < _board.RowColLen && _board[pos].OccupyingPiece == null)
            {
                if (pos.row == 0 || pos.row == _board.RowColLen - 1)
                {
                    possibleMoves.Add(new Promotion(_board[pos], this, new Queen(this.PieceOwner, this._board, pos)));
                }
                else
                {
                    possibleMoves.Add(new Move(_board[pos], this));   
                }
                
                if (TimesMoved == 0)
                {
                    pos.row += _direction;

                    if (pos.row < _board.RowColLen && _board[pos].OccupyingPiece == null)
                    {
                        if (pos.row == 0 || pos.row == _board.RowColLen - 1)
                        {
                            possibleMoves.Add(new Promotion(_board[pos], this, new Queen(this.PieceOwner, this._board, pos)));
                        }
                        else
                        {
                            possibleMoves.Add(new Move(_board[pos], this));   
                        }
                    }
                }
            }

            //reset
            pos = this.CurrentPosition;

            pos.row += _direction;

            if (pos.row < _board.RowColLen)
            {
                if (pos.col + 1 < _board.RowColLen)
                {
                    var tempPos = new PiecePosition(pos.row, pos.col + 1);

                    var tile = _board[tempPos];

                    tile.ThreateningPieces.Add(this);

                    if (tile.OccupyingPiece != null && tile.OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        if (tempPos.row == 0 || tempPos.row == _board.RowColLen - 1)
                        {
                            possibleMoves.Add(new Promotion(tile, this, new Queen(this.PieceOwner, this._board, tempPos)));
                        }
                        else
                        {
                            possibleMoves.Add(new Move(tile, this));
                        }
                    }
                }

                if (pos.col - 1 >= 0) 
                {
                    var tempPos = new PiecePosition(pos.row, pos.col - 1);

                    var tile = _board[tempPos];

                    tile.ThreateningPieces.Add(this);

                    if(tile.OccupyingPiece != null && tile.OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                    {
                        if (tempPos.row == 0 || tempPos.row == _board.RowColLen - 1)
                        {
                            possibleMoves.Add(new Promotion(tile, this, new Queen(this.PieceOwner, this._board, tempPos)));
                        }
                        else
                        {
                            possibleMoves.Add(new Move(tile, this));
                        }
                    }               
                }
            }

            //reset
            pos = this.CurrentPosition;
            pos.col++;

            if (pos.col < _board.RowColLen && _board[pos].OccupyingPiece != null && _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id && _board[pos].OccupyingPiece.PieceName == "Pawn" && ((Pawn)_board[pos].OccupyingPiece).EnPassant)
            {
                var tile = _board[new PiecePosition(pos.row + _direction, pos.col)];
                var move = new EnPassant(_board[tile.Position], this);
                possibleMoves.Add(move);
            }

            pos.col -= 2;

            if (pos.col >= 0 && _board[pos].OccupyingPiece != null && _board[pos].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id && _board[pos].OccupyingPiece.PieceName == "Pawn" && ((Pawn)_board[pos].OccupyingPiece).EnPassant)
            {
                var tile = _board[new PiecePosition(pos.row + _direction, pos.col)];
                var move = new EnPassant(tile, this);
                possibleMoves.Add(move);
            }

            PossibleMoves = possibleMoves;

            return true;
        }
    }
}
