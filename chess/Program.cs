using System;
using System.Linq;
using System.Collections.Generic;
using Chess.Pieces;
using Chess;
using System.Threading;

namespace chess
{
    class Program
    {
        static void Main(string[] args)
        {
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){player1, player2});

            game.SetupDefault();

            while (true)
            {
                try
                {
                    game.Board.PrintBoard();

                    // TODOOOOOO: FIND OUT WHY STALEMATE CALLED WHEN NOT STALEMATE. KING STILL HAD MOVES.
                    // game.AIMove();

                    // Thread.Sleep(100);
                    Console.Write("Select piece to move(format: <row>,<col>) or \"undo\" to undo: ");

                    var move = Console.ReadLine();

                    if (move == "undo")
                    {
                        game.Undo();
                    }

                    if (move == "quit") return;

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
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}

// after every move, go to the opposite king and see there is an enemy rook, bishop, queen, pawn etc horizontally, veritically... same wit



