using System;
using System.Linq;
using System.Collections.Generic;
using chess.pieces;

// make move, see if king now in check, if it is, the move is illegal.
// redesign. have a board that keeps track of what pieces can move where. i.e. each square has a list of pieces where it can move, and a piece that currently occupies the square.

// TODO, ON MOVE UPDATE BOARD TILES AND STUFF/SET APPROPRAITE NULLS.
// ON MOVE, MAKE SURE TO CLEAR THREATENING PIECES ON ALL TILES AS TEHY ARE RECALCULATED.

namespace chess
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();

            board.Setup();

            board.CalculatePieceMoves();

            board.PrintBoard();

            while (true)
            {
                try
                {
                    board.PrintBoard();
                    Console.Write("Select piece to move(format: <row>,<col>: ");

                    var move = Console.ReadLine();

                    if (move == "quit") return;

                    var deets = move.Split(",");

                    // MINUS ONE TO NORMALIZE INPUT. E.G 1,1 WILL BECOME 0,0 WHICH IS THE ACTUAL ARRAY INDEX
                    PiecePosition selectedPosition = new PiecePosition(int.Parse(deets[0]) - 1, int.Parse(deets[1]) - 1);

                    Piece selectedPiece = board[selectedPosition].OccupyingPiece;

                    board.PrintBoard(selectedPiece);

                    Console.Write("Select position to piece to (format: <row>,<col>: ");

                    move = Console.ReadLine();

                    deets = move.Split(",");

                    // MINUS ONE TO NORMALIZE INPUT. E.G 1,1 WILL BECOME 0,0 WHICH IS THE ACTUAL ARRAY INDEX
                    PiecePosition movePosition = new PiecePosition(int.Parse(deets[0]) - 1, int.Parse(deets[1]) - 1);

                    selectedPiece.Move(movePosition);

                    board.CalculatePieceMoves();
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

public class Board
{
    private BoardTile[,] _board;

    public Board()
    {
        _board = new BoardTile[RowColLen, RowColLen];
        for(int i = 0; i < RowColLen; i++)
        {
            for(int j = 0; j < RowColLen; j++)
            {
                _board[i,j] = new BoardTile();
            }
        }
    }

    public BoardTile this[PiecePosition p]
    {
        get { return _board[p.row, p.col]; }
        set { _board[p.row, p.col] = value; }
    }

    public readonly int RowColLen = 8;

    public void Setup()
    {
        var owner1 = new Player(Guid.NewGuid().ToString(), ConsoleColor.Green);

        var owner2 = new Player(Guid.NewGuid().ToString(), ConsoleColor.DarkRed);

        // this[new PiecePosition(0, 0)] = new Rook(owner1, this);
        // this[new PiecePosition(0, 1)] = new Knight(owner1, this);
        // this[new PiecePosition(0, 2)] = new Bishop(owner1, this);
        this[new PiecePosition(4, 4)].OccupyingPiece = new Bishop(owner1, this, new PiecePosition(4,4));
        //this[new PiecePosition(4, 2)].OccupyingPiece = new King(owner1, this, new PiecePosition(4,2));
        // this[new PiecePosition(0, 5)] = new Bishop(owner1, this);  
        // this[new PiecePosition(0, 6)] = new Knight(owner1, this);
        //this[new PiecePosition(5,3)].OccupyingPiece = new Rook(owner1, this, new PiecePosition(5,3));
        //this[new PiecePosition(1, 0)] = new Pawn(owner1, this);
        // this[new PiecePosition(1, 1)] = new Pawn(owner1, this, new PiecePosition(1,1));
        // // this[new PiecePosition(1, 2)] = new Pawn(owner1, this);
        // // this[new PiecePosition(1, 3)] = new Pawn(owner1, this);
        // // this[new PiecePosition(1, 4)] = new Pawn(owner1, this);
        // this[new PiecePosition(1, 5)] = new Pawn(owner1, this, new PiecePosition(1,5));
        // // this[new PiecePosition(1, 6)] = new Pawn(owner1, this);
        // // this[new PiecePosition(1, 7)] = new Pawn(owner1, this);

        // // this[new PiecePosition(4, 3)] = new Knight(owner1, this);

        // this[new PiecePosition(3, 0)] = newZ Rook(owner2, this, new PiecePosition(3, 0));
        // // this[new PiecePosition(7, 1)] = new Knight(owner2, this);
        // // this[new PiecePosition(7, 2)] = new Bishop(owner2, this);
        // // this[new PiecePosition(7, 3)] = new Queen(owner2, this);
       
        this[new PiecePosition(1,1)].OccupyingPiece = new King(owner2, this, new PiecePosition(1,1));
        this[new PiecePosition(3,3)].OccupyingPiece = new Bishop(owner2, this, new PiecePosition(3,3));
        this[new PiecePosition(1,7)].OccupyingPiece = new King(owner2, this, new PiecePosition(1,7));
        this[new PiecePosition(3,5)].OccupyingPiece = new Bishop(owner2, this, new PiecePosition(3,5));
        this[new PiecePosition(7,1)].OccupyingPiece = new King(owner2, this, new PiecePosition(7,1));
        this[new PiecePosition(5,3)].OccupyingPiece = new Bishop(owner2, this, new PiecePosition(5,3));
        this[new PiecePosition(7,7)].OccupyingPiece = new King(owner2, this, new PiecePosition(7,7));
        this[new PiecePosition(5,5)].OccupyingPiece = new Bishop(owner2, this, new PiecePosition(5,5));
        //this[new PiecePosition(6,4)].OccupyingPiece = new Bishop(owner2, this, new PiecePosition(6,4));
        // this[new PiecePosition(7, 6)] = new Knight(owner2, this, new PiecePosition(7,6));
        // // this[new PiecePosition(7, 7)] = new Rook(owner2, this);
        // this[new PiecePosition(6, 0)] = new Pawn(owner2, this, new PiecePosition(6, 0));
        // this[new PiecePosition(6, 1)] = new Pawn(owner2, this);
        // this[new PiecePosition(6, 2)] = new Pawn(owner2, this);
        // this[new PiecePosition(6, 3)] = new Pawn(owner2, this);
        // this[new PiecePosition(6, 4)] = new Pawn(owner2, this);
        // this[new PiecePosition(6, 5)] = new Pawn(owner2, this);
        // this[new PiecePosition(6, 6)] = new Pawn(owner2, this);
        // this[new PiecePosition(6, 7)] = new Pawn(owner2, this);

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


                if (selectedPiece != null && selectedPiece.PossibleMoves.Contains(pos))
                {
                    Console.BackgroundColor = ConsoleColor.Magenta;
                }

                if (piece == null)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("     ");
                    continue;
                }

                Console.ForegroundColor = piece.PieceOwner.color;

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

    public void ClearMeta()
    {
        for(var i = 0; i < RowColLen; i++)
        {
            for(var j = 0; j < RowColLen; j++)
            {
                _board[i,j].ThreateningPieces = new List<Piece>();
            }
        }
    }
    public void CalculatePieceMoves()
    {
        ClearMeta();

        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                var pos = new PiecePosition(i, j);

                if (_board[i, j].OccupyingPiece != null)
                {
                    _board[i, j].OccupyingPiece.CalculateMoves(pos);
                }
            }
        }
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                var pos = new PiecePosition(i, j);

                if (_board[i, j].OccupyingPiece != null)
                {
                    _board[i, j].OccupyingPiece.EliminateIllegalMoves();
                }
            }
        }
    }
}

public struct Player
{
    public Player(string Id, ConsoleColor color)
    {
        this.Id = Id;

        this.color = color;
    }

    public string Id;
    public ConsoleColor color;
}