using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;

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
            Board.CalculatePieceMoves();
        }

        public void SetupDefault()
        {
            var pieces = new List<Piece>();

            var owner1 = _players[0];

            var owner2 = _players[1];

            pieces.Add(new Rook(owner1, Board, new PiecePosition(0,0))); 
            pieces.Add(new Knight(owner1, Board, new PiecePosition(0,1))); 
            pieces.Add(new Bishop(owner1, Board, new PiecePosition(0,2))); 
            pieces.Add(new Queen(owner1, Board, new PiecePosition(0,3))); 
            pieces.Add(new King(owner1, Board, new PiecePosition(0,4))); 
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

            pieces.Add(new Rook(owner2, Board, new PiecePosition(7,0))); 
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
                Board.CalculatePieceMoves();

                this.NextMovePlayer = _players[(_players.IndexOf(NextMovePlayer) + 1) % _players.Count];
            }

            return moveSuccessful;
        }
    }
}