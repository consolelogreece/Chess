using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public class Pawn : Piece
    {
        private bool _hasMoved = false;

        public bool EnPassant = false;

        private int _direction;

        // this is multiplier for first move
        private int _moveMultiplier = 2;

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

            return base.Move(move);
        }

        public override bool CalculateMoves(PiecePosition piecePosition)
        {
            var possibleMoves = new List<PiecePosition>();

            var copy = piecePosition;

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
            copy = piecePosition;

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
            copy = piecePosition;
            copy.col++;
            
            // TODO: Register threats for en passant.
            if (copy.col < _board.RowColLen && _board[copy].OccupyingPiece != null && _board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id && _board[copy].OccupyingPiece.PieceName == "Pawn" && ((Pawn)_board[copy].OccupyingPiece).EnPassant)
            possibleMoves.Add(new PiecePosition(piecePosition.row + _direction, piecePosition.col + 1));

            copy.col -= 2;

            if (copy.col >= 0 && _board[copy].OccupyingPiece != null && _board[copy].OccupyingPiece.PieceOwner.Id != this.PieceOwner.Id && _board[copy].OccupyingPiece.PieceName == "Pawn" && ((Pawn)_board[copy].OccupyingPiece).EnPassant)
            possibleMoves.Add(new PiecePosition(piecePosition.row + _direction, piecePosition.col - 1));

            PossibleMoves = possibleMoves;

            return base.CalculateMoves(piecePosition);
        }
    }
}

