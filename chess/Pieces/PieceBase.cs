using System;
using System.Linq;
using System.Collections.Generic;

namespace chess.pieces
{
    public abstract class Piece
    {
        public Player PieceOwner { get; private set; }

        public List<Piece> ThreateningPieces = new List<Piece>();

        public PiecePosition CurrentPosition;

        public readonly string PieceName;

        public List<PiecePosition> PossibleMoves { get; protected set; } = new List<PiecePosition>();

        protected readonly Board _board;

        public Piece(Player pieceOwner, Board board string pieceName = "Piece")
        {
            PieceOwner = pieceOwner;

            _board = board;

            PieceName = pieceName;

            CurrentPosition = startingPosition;
        }

        public virtual bool CalculateMoves(PiecePosition move)
        {
            return false;
        }

        public virtual void EliminateIllegalMoves(PiecePosition currentPos, PiecePosition KingPosition)
        {

        }

        public virtual bool Move(PiecePosition currentPos, PiecePosition move)
        {
            return PossibleMoves.Contains(move);
        }
    }
}