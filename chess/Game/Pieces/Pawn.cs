using System;
using System.Linq;
using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Pawn : Piece
    {
        private bool _hasMoved = false;

        public bool EnPassant = false;

        private int _direction;

        public Pawn(Player pieceOwner, Board board, PiecePosition startingPosition)
            : base(pieceOwner, board, startingPosition, "Pawn")
        {
            _direction = pieceOwner.Side == "bottom" ? -1 : 1;
        }

        public override bool Move(PiecePosition move)
        {
            _hasMoved = true;

            if (Math.Abs(move.row - CurrentPosition.row) != 1) EnPassant = true;
            else EnPassant = false;

            if (PossibleMoves.Contains(move))
            {
                // only time when this would be true is when taking another pawn en passant.
                if (move.col != this.CurrentPosition.col && _board[move].OccupyingPiece == null)
                {
                    _board[new PiecePosition(move.row + (-1 * _direction), move.col)].OccupyingPiece = null;
                }
                _board[move].OccupyingPiece = this;
                _board[CurrentPosition].OccupyingPiece = null;
                CurrentPosition = move;
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
            var possibleMoves = new List<PiecePosition>();

            var copy = this.CurrentPosition;

            copy.row += _direction;

            if (_board[copy].OccupyingPiece == null)
            {
                possibleMoves.Add(copy);

                if (!_hasMoved)
                {
                    copy.row += _direction;

                    if (_board[copy].OccupyingPiece == null)
                    {
                        possibleMoves.Add(copy);
                    }
                }
            }

            //reset
            copy = this.CurrentPosition;

            copy.row += _direction;

            if (copy.row < _board.RowColLen)
            {
                if (copy.col + 1 < _board.RowColLen && _board[new PiecePosition(copy.row, copy.col + 1)].OccupyingPiece != null && _board[new PiecePosition(copy.row, copy.col + 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                {
                    possibleMoves.Add(new PiecePosition(copy.row, copy.col + 1));
                }

                if (copy.col - 1 >= 0 && _board[new PiecePosition(copy.row, copy.col - 1)].OccupyingPiece != null && _board[new PiecePosition(copy.row, copy.col - 1)].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id)
                {
                    possibleMoves.Add(new PiecePosition(copy.row, copy.col - 1));
                }
            }

            //reset
            copy = this.CurrentPosition;
            copy.col++;
            
            // TODO: Register threats for en passant.
            if (copy.col < _board.RowColLen && _board[copy].OccupyingPiece != null && _board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id && _board[copy].OccupyingPiece.PieceName == "Pawn" && ((Pawn)_board[copy].OccupyingPiece).EnPassant)
                possibleMoves.Add(new PiecePosition(this.CurrentPosition.row + _direction, this.CurrentPosition.col + 1));

            copy.col -= 2;

            if (copy.col >= 0 && _board[copy].OccupyingPiece != null && _board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id && _board[copy].OccupyingPiece.PieceName == "Pawn" && ((Pawn)_board[copy].OccupyingPiece).EnPassant)
                possibleMoves.Add(new PiecePosition(this.CurrentPosition.row + _direction, this.CurrentPosition.col - 1));

            PossibleMoves = possibleMoves;

            return base.CalculateMoves();
        }
    }
}

