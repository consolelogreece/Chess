using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Helpers;
using Chess.Moves;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        private bool _hasMoved = false;

        public bool EnPassant = false;

        private int _direction;

        public Pawn(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Pawn", 10)
        {
            _direction = pieceOwner.Side == "bottom" ? -1 : 1;
        }

        public override bool Move(PiecePosition movePos)
        {
            _hasMoved = true;

            if (Math.Abs(movePos.row - CurrentPosition.row) != 1) EnPassant = true;
            else EnPassant = false;

            var move = PossibleMoves.FirstOrDefault(m => m.GetMovePos().Position == movePos);

            if (move != null)
            {
                // // only time when this would be true is when taking another pawn en passant.
                // if (move.col != this.CurrentPosition.col && _board[move].OccupyingPiece == null)
                // {
                //     _board[new PiecePosition(move.row + (-1 * _direction), move.col)].OccupyingPiece = null;
                // }
                // _board[move].OccupyingPiece = this;
                // _board[CurrentPosition].OccupyingPiece = null;
                // CurrentPosition = move;

                move.MakeMove();

                Promote();
                return true;
            }

            return false;
        }

        private bool Promote()
        {
            // added one to row to normalize position.
            if (this.CurrentPosition.row != 0 && this.CurrentPosition.row + 1 != _board.RowColLen)
            {
                return false;
            }

            Console.WriteLine("Selecet piece to promote to (rook, knight, bishop, or queen):");

            string piece = Console.ReadLine();

            bool hasPromoted = false;

            while(!hasPromoted)
            {
                switch(piece)
                {
                    case "rook":
                        _board[this.CurrentPosition].OccupyingPiece = new Rook(this.PieceOwner, _board, this.CurrentPosition);
                        hasPromoted = true;
                        break;

                    case "knight":
                        _board[this.CurrentPosition].OccupyingPiece = new Knight(this.PieceOwner, _board, this.CurrentPosition);
                        hasPromoted = true;
                        break;

                    case "bishop":
                        _board[this.CurrentPosition].OccupyingPiece = new Bishop(this.PieceOwner, _board, this.CurrentPosition);
                        hasPromoted = true;
                        break;

                    case "queen":
                        _board[this.CurrentPosition].OccupyingPiece = new Queen(this.PieceOwner, _board, this.CurrentPosition);
                        hasPromoted = true;
                        break;
                }
            }

            return true;
        }

        public override bool CalculateMoves()
        {
            var possibleMoves = new List<IMove>();

            var pos = this.CurrentPosition;

            pos.row += _direction;

            if (_board[pos].OccupyingPiece == null)
            {
                possibleMoves.Add(new Move(_board[pos], this));

                if (!_hasMoved)
                {
                    pos.row += _direction;

                    if (_board[pos].OccupyingPiece == null)
                    {
                        possibleMoves.Add(new Move(_board[pos], this));
                    }
                }
            }

            //reset
            pos = this.CurrentPosition;

            pos.row += _direction;

            if (pos.row < _board.RowColLen)
            {
                if (pos.col + 1 < _board.RowColLen && _board[new PiecePosition(pos.row, pos.col + 1)].OccupyingPiece != null && _board[new PiecePosition(pos.row, pos.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                {
                    var tempPos = new PiecePosition(pos.row, pos.col + 1);
                    possibleMoves.Add(new Move(_board[tempPos], this));
                }

                if (pos.col - 1 >= 0 && _board[new PiecePosition(pos.row, pos.col - 1)].OccupyingPiece != null && _board[new PiecePosition(pos.row, pos.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                {
                    var tempPos = new PiecePosition(pos.row, pos.col - 1);
                    possibleMoves.Add(new Move(_board[tempPos], this));
                }
            }

            //reset
            pos = this.CurrentPosition;
            pos.col++;
            
            // TODO: Register threats for en passant.
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

            return base.CalculateMoves();
        }
    }
}

