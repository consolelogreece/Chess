using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Chess;
using Chess.Pieces;

namespace chess
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new Player(Guid.NewGuid().ToString(), "bottom");
            var player2 = new Player(Guid.NewGuid().ToString(), "top");

            var game = new Game(new List<Player>() { player1, player2 });

            game.SetupDefault();

            while (true)
            {
                try
                {
                    game.Board.PrintBoard();

                    Console.WriteLine(game.NextMovePlayer.Side);
                    Console.Write("\"undo\" to undo, \"ai\" to make computer take turn (unfinished), \"quit\" to quit, or");
                    Console.Write(" select piece to move(format: <row>,<col>): ");

                    var move = Console.ReadLine();

                    if (move == "undo")
                    {
                        game.Undo();
                        continue;
                    }
                    if (move == "ai")
                    {
                        game.AIMove();
                        continue;
                    }
                    if (move == "quit")return;

                    var deets = move.Split(",");

                    // MINUS ONE TO NORMALIZE INPUT. E.G 1,1 WILL BECOME 0,0 WHICH IS THE ACTUAL ARRAY INDEX
                    PiecePosition selectedPosition = new PiecePosition(int.Parse(deets[0]) - 1, int.Parse(deets[1]) - 1);

                    Piece selectedPiece = game.Board[selectedPosition].OccupyingPiece;

                    game.Board.PrintBoard(selectedPiece);

                    Console.Write("Select position to piece to (format: <row>,<col>): ");

                    move = Console.ReadLine();

                    deets = move.Split(",");

                    // MINUS ONE TO NORMALIZE INPUT. E.G 1,1 WILL BECOME 0,0 WHICH IS THE ACTUAL ARRAY INDEX
                    PiecePosition movePosition = new PiecePosition(int.Parse(deets[0]) - 1, int.Parse(deets[1]) - 1);

                    game.Move(selectedPosition, movePosition);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}