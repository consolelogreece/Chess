using System;
using Chess.Helpers;
using Chess.Pieces;

namespace Chess
{
    public class Move
    {
        public readonly Piece OwningPiece;
        public readonly BoardTile To;

        private Action Consequence;

        public Move(BoardTile to, Piece owningPiece)
        {
            To = to;
            OwningPiece = owningPiece;
        }

        public void SetConsequence(Action consequence)
        {
            Consequence = consequence;
        }

        public void MakeMove()
        {
            if (Consequence == null)
            {
                MoveHelpers.MoveNormal(OwningPiece, To);
            }
            else
            {
                Consequence();
            }
        }
    }
}