using System;
using System.Linq;
using System.Collections.Generic;
using chess.pieces;

// TODO, ON MOVE UPDATE BOARD TILES AND STUFF/SET APPROPRAITE NULLS.
// TODO: find better way of taking a piece, just setting posiiton of piece to null isnt good enough
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
                    Console.Write("Select piece to move(format: <row>,<col>): ");

                    var move = Console.ReadLine();

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
                _board[i,j] = new BoardTile(new PiecePosition(i, j));
            }
        }
    }

    public bool IsCheck()
    {
        foreach(var tile in _board)
        {
            if (tile.OccupyingPiece?.PieceName == "King")
            {
                var king = (King)tile.OccupyingPiece;


                return king.IsInCheckmate();
            }
        }

        return false;
    }

    // public bool IsCheckMate()
    // {

    // }

    public BoardTile this[PiecePosition p]
    {
        get { return _board[p.row, p.col]; }
        set { _board[p.row, p.col] = value; }
    }

    public readonly int RowColLen = 8;

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
    public Player(string Id, string side)
    {
        this.Id = Id;

        this.Side = side;
    }

    public readonly string Id;

    public readonly string Side;
}

public class Game
{
    public Board Board { get; private set; }

    protected List<Player> _players = new List<Player>();

    public Game(List<Player> players)
    {
        if (players.Count != 2 || players[0].Id == players[1].Id)
        {
            throw new InvalidOperationException("Two unique players only");
        }

        Board = new Board();

        _players = players;
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

    public bool Move(PiecePosition from, PiecePosition to)
    {
        var selectedPiece = Board[from].OccupyingPiece;

        var moveSuccessful = selectedPiece.Move(to);

        if (moveSuccessful)
        {
            Board.CalculatePieceMoves();
        }

        return moveSuccessful;
    }
}