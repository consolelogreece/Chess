using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Moves;

namespace Chess.Pieces
{
    public abstract class Piece
    {
        public readonly string PieceName;
        public readonly string VisualID;
        public int TimesMoved = 0;
        public readonly int PieceValue;
        public Player PieceOwner { get; private set; }
        public PiecePosition CurrentPosition;
        public List<IMove> PossibleMoves { get; protected set; } = new List<IMove>();
        public readonly Board _board;
        protected float[, ] BoardValueTable;

        public Piece(Player pieceOwner, Board board, PiecePosition startingPosition, string visualId = "X", string pieceName = "Piece", int pieceValue = 10)
        {
            PieceOwner = pieceOwner;

            _board = board;

            PieceValue = pieceValue;

            PieceName = pieceName;

            CurrentPosition = startingPosition;

            VisualID = visualId;

            GenBoardValueTable();
        }

        protected virtual void GenBoardValueTable()
        {
            if (this.BoardValueTable != null)return;

            var boardValueTable = new float[8, 8]
            { 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }, 
                { 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            this.BoardValueTable = boardValueTable;
        }

        public virtual void ClearMoves(List<IMove> illegalMoves = null)
        {
            if (illegalMoves == null) 
            {
                this.PossibleMoves.Clear();
                return;
            }

            foreach(var move in illegalMoves)
            {
                PossibleMoves.Remove(move);
            }
        }

        public float GetPosValue()
        {
            return this.BoardValueTable[this.CurrentPosition.row, this.CurrentPosition.col];
        }

        public virtual bool CalculateMoves()
        {
            // todo: move this out of calculating moves as this is not technically a part of the calculation bit.
            foreach (var m in PossibleMoves)
            {
                m.GetMovePos().ThreateningPieces.Add(this);
            }

            return true;
        }

        // todo: currently removes option to actually take checking piece. fix. think has to do with having ++pos.row instead of pos.row++ in helpers
        public virtual void EliminateIllegalMoves()
        {
            foreach (var threateningPiece in _board[this.CurrentPosition].ThreateningPieces)
            {
                var tiles = threateningPiece.XRay(this);

                var occupiedTiles = tiles.Where(t => t.OccupyingPiece != null).ToList();

                // occupied tile[0] is the tile where the scanning piece is. aka where the scan starts.
                // occupied tile[1] is the piece that the scanner is threatening.
                // occupied tile[2] is the first piece after the threatened piece. if this it not the king, the piece is not pinned.
                // only if tile[2] is the king is there a pin, so don't do anything if it isnt.
                if (occupiedTiles.Count >= 3 && occupiedTiles[2].OccupyingPiece.PieceName == "King" && occupiedTiles[2].OccupyingPiece.PieceOwner.Id == this.PieceOwner.Id)
                {
                    this.PossibleMoves.RemoveAll(m => !tiles.Any(t => t.Position == (m.GetMovePos().Position)));
                }
            }
        }

        // Each piece eholds the logic on how it can pin other pieces here. a piece will ask this piece if it's pinned, and this piece will remove moves as necessary.
        public virtual List<BoardTile> XRay(Piece target)
        {
            return new List<BoardTile>();
        }

        public virtual IMove Move(PiecePosition movePos)
        {
            var move = this.GetMoves().FirstOrDefault(m => m.GetMovePos().Position == movePos);

            if (move != null)
            {
                move.MakeMove();
            }

            return move;
        }

        public virtual List<IMove> GetMoves()
        {
            return this.PossibleMoves;
        }
    }
}
