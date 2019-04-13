using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class Pawn : Piece
    {
        private bool _hasMoved = false;

        public bool _enPassant = false;

        private int _direction;

        // this is multiplier for first move
        private int _moveMultiplier = 2;

        public Pawn(Player pieceOwner, Board board) 
            : base(pieceOwner, board, "Pawn")
        {
            _direction = pieceOwner.color == ConsoleColor.DarkRed ? -1 : 1;
        }

        public override bool Move(PiecePosition currentPos, PiecePosition move)
        {
            _hasMoved = true;

            if (Math.Abs(move.row - currentPos.col) != 1) _enPassant = true;
            else _enPassant = false;

            return base.Move(currentPos, move);
        }

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

            copy.row += _direction;

            if (_board[copy] == null)
            {
                possibleMoves.Add(copy);

                if (!_hasMoved)
                {
                    copy.row += _direction;

                    if (_board[copy] == null)
                    {
                        possibleMoves.Add(copy);
                    }
                }
            }

            //reset
            copy = piecePosition;

            copy.row++;

            if (copy.row < _board.RowColLen)
            {
                if (copy.col + 1 < _board.RowColLen && _board[new PiecePosition(copy.row, copy.col + 1)] != null && _board[new PiecePosition(copy.row, copy.col + 1)].PieceOwner.Id != this.PieceOwner.Id)
                    possibleMoves.Add(new PiecePosition(copy.row, copy.col + 1));

                if (copy.col - 1 >= 0 && _board[new PiecePosition(copy.row, copy.col - 1)] != null && _board[new PiecePosition(copy.row, copy.col - 1)].PieceOwner.Id != this.PieceOwner.Id)
                    possibleMoves.Add(new PiecePosition(copy.row, copy.col - 1));
            }

            //reset
            copy = piecePosition;
            copy.col++;
            
            if (copy.col < _board.RowColLen && _board[copy] != null && _board[copy].PieceOwner.Id != this.PieceOwner.Id && _board[copy].PieceName == "Pawn" && ((Pawn)_board[copy])._enPassant)
            possibleMoves.Add(new PiecePosition(piecePosition.row + _direction, piecePosition.col + 1));

            copy.col -= 2;

            if (copy.col >= 0 && _board[copy] != null && _board[copy].PieceOwner.Id != this.PieceOwner.Id && _board[copy].PieceName == "Pawn" && ((Pawn)_board[copy])._enPassant)
            possibleMoves.Add(new PiecePosition(piecePosition.row + _direction, piecePosition.col - 1));

            PossibleMoves = possibleMoves;

            return true;
        }
    }
}

