using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Helpers;
using Chess.Pieces;

// TODO: impement castling. will probably have to rethink moves. have a move class maybe instead of just using piece positions and null.
namespace Chess
{
    public class Game
    {
        public Board Board { get; private set; }

        protected List<Player> _players = new List<Player>();

        public Player NextMovePlayer;

        public Game(List<Player> players)
        {
            if (players.Count != 2 || players[0].Id == players[1].Id)
            {
                throw new InvalidOperationException("Two unique players only");
            }

            Board = new Board();

            _players = players;

            NextMovePlayer = _players[0];
        }

        public void Setup(List<Piece> pieces)
        {
            foreach(var piece in pieces)
            {
                if (!_players.Any(p => p.Id == piece.PieceOwner.Id)) 
                    throw new InvalidOperationException("Piece owner not registered");

                Board.RegisterPiece(piece);
            }

            // calculate initial moves.
            CalculatePieceMoves();
        }

        public void SetupDefault()
        {
            var pieces = new List<Piece>();

            var owner1 = _players[0];

            var owner2 = _players[1];

            pieces.Add(new Rook(owner1, Board, new PiecePosition(0,0))); 
            pieces.Add(new Knight(owner1, Board, new PiecePosition(0,1))); 
            pieces.Add(new Bishop(owner1, Board, new PiecePosition(0,2))); 
            pieces.Add(new King(owner1, Board, new PiecePosition(0,3)));
            pieces.Add(new Queen(owner1, Board, new PiecePosition(0,4)));  
            pieces.Add(new Bishop(owner1, Board, new PiecePosition(0,5))); 
            pieces.Add(new Knight(owner1, Board, new PiecePosition(0,6))); 
            pieces.Add(new Rook(owner1, Board, new PiecePosition(0,7))); 

            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,0)));       
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,1)));
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,2)));
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,3)));
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,4)));       
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,5)));
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,6)));
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1,7)));

            pieces.Add(new Rook(owner2, Board, new PiecePosition(7, 0))); 
            pieces.Add(new Knight(owner2, Board, new PiecePosition(7,1))); 
            pieces.Add(new Bishop(owner2, Board, new PiecePosition(7,2))); 
            pieces.Add(new Queen(owner2, Board, new PiecePosition(7,3))); 
            pieces.Add(new King(owner2, Board, new PiecePosition(7,4))); 
            pieces.Add(new Bishop(owner2, Board, new PiecePosition(7,5))); 
            pieces.Add(new Knight(owner2, Board, new PiecePosition(7,6))); 
            pieces.Add(new Rook(owner2, Board, new PiecePosition(7,7))); 

            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,0)));       
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,1)));  
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,2)));  
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,3)));  
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,4)));  
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,5)));  
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,6)));  
            pieces.Add(new Pawn(owner2, Board, new PiecePosition(6,7)));  
            
            this.Setup(pieces);
        }

        private bool IsPlayersGo(Player player)
        {
            return this.NextMovePlayer == player;
        }

        public bool Move(PiecePosition from, PiecePosition to)
        {
            var selectedPiece = Board[from].OccupyingPiece;

            if (!IsPlayersGo(selectedPiece.PieceOwner))
            {
                Console.WriteLine("It is not your go.\nPress enter to continue...");
                Console.Read();
                return false;
            }

            var moveSuccessful = selectedPiece.Move(to);

            if (moveSuccessful)
            {
                CalculatePieceMoves();

                HandleCheckStuff();

                this.NextMovePlayer = _players[(_players.IndexOf(NextMovePlayer) + 1) % _players.Count];
            }
            
            return moveSuccessful;
        }

        private void HandleCheckStuff()
        {
            var checkedPiece = DetectCheck();
            
            if (checkedPiece == null) return;

            if (checkedPiece.IsInCheckmate())
            {
                Console.WriteLine("Game over, it's Checkmate!\nPress any key to exit...");
                Console.Read();
                Environment.Exit(0);
            }

            var checkPath = checkedPiece.GetCheckPath();

            // remove all current moves as you can't move if the kind is in check.
            ClearPossibleMoves(new Predicate<Piece>(m => m != checkedPiece));

            // re-add moves that would block the check. may not work as intended as it might remove info to do with pinning. may need to rethink.
            foreach(var tile in checkPath)
            {
                foreach(var piece in tile.ThreateningPieces)
                {
                    if (piece == checkedPiece) continue;
                    
                    piece.PossibleMoves.Add(new Move(tile, piece));
                }
            }
        }

        public void ClearPossibleMoves(params Predicate<Piece>[] predicates)
        {
            foreach(BoardTile tile in Board)
            {
                var piece = tile.OccupyingPiece;

                if (piece == null) continue;

                var illegal = false;

                foreach(var predicate in predicates)
                {
                    if (predicate(piece)) continue;
                    
                    illegal = true;
                    break;
                }

                // if does not match all predicates, do not clear moves.
                if (illegal) continue;

                piece.ClearMoves();
            }
        }

        public void ClearMeta(params Predicate<BoardTile>[] predicates)
        {
            foreach(BoardTile tile in Board)
            {
                foreach(var predicate in predicates)
                {
                    if (!predicate(tile)) continue;
                }

                tile.ThreateningPieces.Clear();
            }
        }

        private void CalculatePieceMoves()
        {
            ClearMeta();

            ClearPossibleMoves();

            foreach(BoardTile tile in Board)
            {
                if (tile.Position == new PiecePosition(3,0))
                {
                    
                }
                tile.OccupyingPiece?.CalculateMoves();
            }

            foreach(BoardTile tile in Board)
            {
                tile.OccupyingPiece?.EliminateIllegalMoves();
            }
        }

        private King DetectCheck()
        {
            foreach(var tile in Board)
            {
                var piece = ((BoardTile)tile).OccupyingPiece;

                if (piece?.PieceName == "King")
                {
                    if (Board[piece.CurrentPosition].ThreateningPieces.Any(p => p.PieceOwner != piece.PieceOwner))
                    {
                        return (King)piece;
                    }
                }
            }

            return null;
        }
    }
}