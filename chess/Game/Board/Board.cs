using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Chess.Pieces;

namespace Chess
{
    public class Board : IEnumerable
    {
        private BoardTile[, ] _board;

        private List<Piece> Pieces = new List<Piece>();

        public readonly int RowColLen = 8;

        public Board()
        {
            _board = new BoardTile[RowColLen, RowColLen];
            for (int i = 0; i < RowColLen; i++)
            {
                for (int j = 0; j < RowColLen; j++)
                {
                    _board[i, j] = new BoardTile(new PiecePosition(i, j));
                }
            }
        }

        public BoardTile this [PiecePosition p]
        {
            get { return _board[p.row, p.col]; }
            set { _board[p.row, p.col] = value; }
        }

        public void RegisterPiece(Piece piece)
        {
            this [piece.CurrentPosition].OccupyingPiece = piece;
            Pieces.Add(piece);
        }

        public void DeRegisterPiece(Piece piece)
        {
            this [piece.CurrentPosition].OccupyingPiece = null;
            Pieces.Remove(piece);
        }

        public List<Piece> GetPieces(params Predicate<Piece>[] predicates)
        {
            var pieces = new List<Piece>();

            foreach (var piece in this.Pieces)
            {
                var shouldAdd = true;

                foreach (var predicate in predicates)
                {
                    if (!predicate(piece))
                    {
                        shouldAdd = false;
                        break;
                    }
                }

                if (shouldAdd)pieces.Add(piece);
            }

            return pieces;
        }

        public void PrintBoard(Piece selectedPiece = null)
        {
            Console.Clear();
            var roundFlag = true;
            for (int i = _board.GetLength(0) - 1; i >= 0; i--)
            {
                Console.WriteLine("---------------------------------------------");
                Console.Write($"  {i + 1} |");
                for (int j = 0; j < _board.GetLength(1); j++)
                {
                    var piece = _board[i, j].OccupyingPiece;

                    var pos = new PiecePosition(i, j);

                    if (roundFlag)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }

                    roundFlag = !roundFlag;

                    if (selectedPiece != null && selectedPiece.GetMoves().Any(m => m.GetMovePos().Position == pos))
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                    }

                    if (piece == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write("     ");
                        continue;
                    }

                    Console.ForegroundColor = piece.PieceOwner.Side == "bottom" ? ConsoleColor.DarkBlue : ConsoleColor.DarkRed;
                    Console.Write($"  {piece.VisualID}  ");
                }

                // offset for checker board pattern.
                roundFlag = !roundFlag;

                Console.ResetColor();

                Console.WriteLine();
            }

            Console.WriteLine("---------------------------------------------");
            for (int i = 0; i < _board.GetLength(1) + 1; i++)
            {
                Console.Write($"| {i} |");
            }
            Console.WriteLine("\n---------------------------------------------");
        }

        public IEnumerator GetEnumerator()
        {
            return _board.Cast<BoardTile>().GetEnumerator();
        }

        public float EvaluateBoard(Player player, Player next)
        {
            var total = 0f;

            foreach (var piece in GetPieces())
            {
                var val = 0f;

                val += piece.PieceValue;

                val += piece.GetPosValue();

                if (piece.PieceOwner != player)total -= val;
                else total += val;
            }

            if (IsCheckMate(next))
            {
                Console.WriteLine("steeeerike wunn");
                return float.MaxValue;
            }
            else if(IsStalemate(next))
            {
                // if stalemate, this is good if losing, bad if not, so reverse score.
                return total * -1;
            }

            return total;
        }

        public bool IsCheckMate(Player next)
        {
            var kings = GetPieces(p => p.PieceName == "King", p => p.PieceOwner == next);

            foreach (King king in kings)
            {
                if (this[king.CurrentPosition].ThreateningPieces.Any(p => p.PieceOwner != king.PieceOwner))
                {
                    return king.IsInCheckmate();
                }
            }

            return false;
        }

        public bool IsStalemate(Player next)
        {
            var hasMoveAvailable = false;

            var pieces = GetPieces(p => p.PieceOwner == next);

            foreach (var piece in pieces)
            {
                if (piece.GetMoves().Count > 0)
                {
                    hasMoveAvailable = true;

                    break;
                }
            }

            return !hasMoveAvailable;
        }
    }
}
