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
        private BoardTile[,] _board;

        public readonly int RowColLen = 8;

        public Board()
        {
            _board = new BoardTile[RowColLen, RowColLen];
            for(int i = 0; i < RowColLen; i++)
            {
                for(int j = 0; j < RowColLen; j++)
                {
                    _board[i,j] = new BoardTile(new PiecePosition(i, j));
                }
            }
        }

        public BoardTile this[PiecePosition p]
        {
            get { return _board[p.row, p.col]; }
            set { _board[p.row, p.col] = value; }
        }

        public void RegisterPiece(Piece piece)
        {
            this[piece.CurrentPosition].OccupyingPiece = piece;
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


                    if (selectedPiece != null && selectedPiece.GetMoves().Any(m => m.To.Position == pos))
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
                    Console.Write($"  {piece.PieceName[0]}  ");
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
    }
}