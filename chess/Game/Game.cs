using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Chess.Helpers;
using Chess.Helpers.Moves;
using Chess.Moves;
using Chess.Pieces;

// TODO: add undo method to each move. this will help us implement minimax and will be a nice feature to boot.
// TODO: Have promoting as a valid move so ai can do it.
namespace Chess
{
    public class Game
    {
        public Board Board { get; private set; }

        protected List<Player> _players = new List<Player>();

        public Player NextMovePlayer;

        private List<IMove> History = new List<IMove>();

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
            foreach (var piece in pieces)
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

            // pieces.Add(new Rook(owner1, Board, new PiecePosition(0, 0)));
            // pieces.Add(new Knight(owner1, Board, new PiecePosition(0, 1)));
            // pieces.Add(new Bishop(owner1, Board, new PiecePosition(0, 2)));
            // pieces.Add(new King(owner1, Board, new PiecePosition(0, 3)));
            pieces.Add(new Queen(owner1, Board, new PiecePosition(0, 4)));
            // // pieces.Add(new Bishop(owner1, Board, new PiecePosition(0, 5)));
            // // pieces.Add(new Knight(owner1, Board, new PiecePosition(0, 6)));
            // // pieces.Add(new Rook(owner1, Board, new PiecePosition(0, 7)));

            // pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 0)));
            // pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 1)));
            // pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 2)));
            pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 3)));
            // pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 4)));
            // pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 5)));
            // pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 6)));
            //pieces.Add(new Pawn(owner1, Board, new PiecePosition(1, 7)));

            // pieces.Add(new Rook(owner2, Board, new PiecePosition(7, 0)));
            // pieces.Add(new Knight(owner2, Board, new PiecePosition(7, 1)));
            // pieces.Add(new Bishop(owner2, Board, new PiecePosition(7, 2)));
            pieces.Add(new Queen(owner2, Board, new PiecePosition(7, 3)));
            pieces.Add(new King(owner2, Board, new PiecePosition(3, 4)));
            // pieces.Add(new Bishop(owner2, Board, new PiecePosition(7, 5)));
            // pieces.Add(new Knight(owner2, Board, new PiecePosition(7, 6)));
            // pieces.Add(new Rook(owner2, Board, new PiecePosition(7, 7)));

            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 0)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 1)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 2)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 3)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 4)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 5)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 6)));
            // pieces.Add(new Pawn(owner2, Board, new PiecePosition(6, 7)));

            this.Setup(pieces);
        }

        private bool IsPlayersGo(Player player)
        {
            return this.NextMovePlayer == player;
        }

        public bool Move(IMove move, bool sw = true)
        {
            move.MakeMove();

            History.Add(move);

            CalculatePieceMoves();

            this.NextMovePlayer = GetNextPlayer(NextMovePlayer);

            if (sw)
            {
                if (Board.IsStalemate(this.NextMovePlayer))
                {
                    // TODO: FIGURE OUT WHY STALEMATE. I THINK IT IS CHECKING WRONG PLAYER
                    var sm = Board.IsStalemate(this.NextMovePlayer);

                    if (sm) {Console.WriteLine("stalemate"); Console.ReadLine();}
                }

                var cm = Board.IsCheckMate(NextMovePlayer);

                if (cm) {Console.WriteLine("stalemate"); Console.ReadLine();}
            }

            return true;
        }

        public bool Move(PiecePosition from, PiecePosition to, bool sw = true)
        {
            var selectedPiece = Board[from].OccupyingPiece;

            if (!IsPlayersGo(selectedPiece.PieceOwner))
            {
                Console.WriteLine("It is not your go.\nPress enter to continue...");
                Console.Read();
                return false;
            }

            var move = selectedPiece.Move(to);

            if (move != null)
            {
                Move(move, sw);
            }

            return move != null;
        }

        private void HandleCheckStuff()
        {
            var checkedPiece = DetectCheck();

            if (checkedPiece == null)return;

            if (checkedPiece.IsInCheckmate())
            {
                Console.WriteLine("Game over, it's Checkmate!\nPress any key to exit...");
                Console.Read();
                // Environment.Exit(0);
                return;
            }

            var checkPath = checkedPiece.GetCheckPath();

            var pieces = Board.GetPieces(p => p.PieceOwner == checkedPiece.PieceOwner, p => p != checkedPiece);

            foreach(var piece in pieces)
            {
                ClearPossibleMoves(piece, 
                    new Predicate<IMove>(m => !checkPath.Contains(m.GetMovePos()))
                );
            } 
        }


        public Player GetNextPlayer(Player player)
        {
            var pIndex = _players.IndexOf(player);

            if (pIndex == -1) throw new InvalidOperationException("Player given doesn't exist.");
            
            return _players[(pIndex + 1) % _players.Count];
        }

        public Player GetPrevPlayer(Player player)
        {
            var pIndex = _players.IndexOf(player);
            
            if (pIndex == -1) throw new InvalidOperationException("Player given doesn't exist.");

            if (pIndex == 0)pIndex = _players.Count - 1;
            else pIndex -= 1;

            return _players[pIndex];
        }

        // may have to remove 2 as undoing a turn is undoing both black and white.
        public void Undo()
        {
            if (History.Count == 0)return;

            var move = History.Last();

            move.UndoMove();

            History.Remove(move);

            this.NextMovePlayer = GetPrevPlayer(NextMovePlayer);

            CalculatePieceMoves();
        }

        public bool AIMove()
        {
            var move = AIHelpers.Minimax(this, 1);

            return this.Move(move);
        }

        public List<IMove> GetAllMoves(Player player)
        {
            var moves = new List<IMove>();

            var pieces = Board.GetPieces(p => p.PieceOwner == player);

            foreach (var piece in pieces)
            {
                moves.AddRange(piece.GetMoves());
            }

            return moves;
        }

        public void ClearPossibleMoves(Piece piece, params Predicate<IMove>[] predicates)
        {
            var illegalMoves = new List<IMove>();

            var moves = piece.GetMoves();

            foreach(var move in moves)
            {
                var illegal = false;

                foreach (var predicate in predicates)
                {
                    if (!predicate(move))continue;

                    illegal = true;
                    break;
                }
                
                if (illegal) illegalMoves.Add(move);
            }

            piece.ClearMoves(illegalMoves);
            
        }

        public void ClearMeta(params Predicate<BoardTile>[] predicates)
        {
            foreach (BoardTile tile in Board)
            {
                foreach (var predicate in predicates)
                {
                    if (!predicate(tile))continue;
                }

                tile.ThreateningPieces.Clear();
            }
        }

        private void CalculatePieceMoves()
        {
            ClearMeta();

            ClearAllMoves();

            var pieces = Board.GetPieces();

            foreach (var piece in pieces)
            {
                piece.CalculateMoves();
            }

            foreach (var piece in pieces)
            {
                piece.EliminateIllegalMoves();
            }

            HandleCheckStuff();
        }

        private void ClearAllMoves()
        {
            foreach(var piece in Board.GetPieces())
            {
                piece.ClearMoves(piece.GetMoves());
            }
        }

        private King DetectCheck()
        {
            var kings = Board.GetPieces(p => p.PieceName == "King");
            foreach (King king in kings)
            {
                if (Board[king.CurrentPosition].ThreateningPieces.Any(p => p.PieceOwner != king.PieceOwner))
                {
                    return king;
                }
            }

            return null;
        }
    }
}
