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

        // todo: currently removes option to actually take checking piece. fix. think has to do with having ++pos.row instead of pos.row++ in helpers
        public virtual void EliminateIllegalMoves()
        {
            foreach(var threateningPiece in _board[this.CurrentPosition].ThreateningPieces)
            {
                var tiles = threateningPiece.XRay(this);

                var occupiedTiles = tiles.Where(t => t.OccupyingPiece != null).ToList();
                
                // occupied tile[0] is the tile where the scanning piece is. aka where the scan starts.
                // occupied tile[1] is the piece that the scanner is threatening.
                // occupied tile[2] is the first piece after the threatened piece. if this it not the king, the piece is not pinned.
                // only if tile[2] is the king is there a pin, so don't do anything if it isnt.
                if (occupiedTiles.Count >= 3 && occupiedTiles[2].OccupyingPiece.PieceName == "King" && occupiedTiles[2].OccupyingPiece.PieceOwner.Id == this.PieceOwner.Id)
                {
                    this.PossibleMoves.RemoveAll(m => !tiles.Any(t => t.Position == m));
                }
            }
        }

        // Each piece eholds the logic on how it can pin other pieces here. a piece will ask this piece if it's pinned, and this piece will remove moves as necessary.
        public virtual List<BoardTile> XRay(Piece target)
        {
            return new List<BoardTile>();
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