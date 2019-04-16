using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public abstract class Piece
    {
        public Player PieceOwner { get; private set; }

        public List<Piece> PinningPieces = new List<Piece>();

        public PiecePosition CurrentPosition;

        public readonly string PieceName;

        public List<PiecePosition> PossibleMoves { get; protected set; } = new List<PiecePosition>();

        public readonly Board _board;

        public Piece(Player pieceOwner, Board board, PiecePosition startingPosition, string pieceName = "Piece")
        {
            PieceOwner = pieceOwner;

            _board = board;

            PieceName = pieceName;

            CurrentPosition = startingPosition;
        }

        public virtual bool CalculateMoves(PiecePosition move)
        {
            foreach(var m in PossibleMoves)
            {
                _board[new PiecePosition(m.row, m.col)].ThreateningPieces.Add(this);
            }

            return true;
        }

        public virtual void EliminateIllegalMoves()
        {
        //     King king;

        //     for(var i = 0; i <  _board.RowColLen; i++)
        //     {
        //         for(var j = 0; j <  _board.RowColLen; j++)
        //         {
        //             if (_board[new PiecePosition(i,j)].OccupyingPiece != null && _board[new PiecePosition(i, j)].OccupyingPiece.PieceName == "King")
        //             {
        //                 king = (King)_board[new PiecePosition(i,j)].OccupyingPiece;
        //                 break;
        //             }
        //         }
        //     }

        //     var tile = _board[this.CurrentPosition];

            foreach(var piece in _board[this.CurrentPosition].ThreateningPieces)
            {
                // if this piece is pinned, remove any spaces it cant move to.
                piece.StripIllegalMovesPin(this);
            }
        }

        // Each piece eholds the logic on how it can pin other pieces here. a piece will ask this piece if it's pinned, and this piece will remove moves as necessary.
        public virtual void StripIllegalMovesPin(Piece piece)
        {

        }
        public virtual bool Move(PiecePosition move)
        {
            if (PossibleMoves.Contains(move))
            {
                _board[move].OccupyingPiece = this;
                _board[CurrentPosition].OccupyingPiece = null;
                CurrentPosition = move;
                return true;
            }

            return false;
        }
    }
}