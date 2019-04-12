using System;
using System.Collections.Generic;

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

                    Piece selectedPiece = board[selectedPosition];

                    board.PrintBoard(selectedPiece);

                    Console.Write("Select position to piece to (format: <row>,<col>: ");

                    move = Console.ReadLine();

                    deets = move.Split(",");

                    // MINUS ONE TO NORMALIZE INPUT. E.G 1,1 WILL BECOME 0,0 WHICH IS THE ACTUAL ARRAY INDEX
                    PiecePosition movePosition = new PiecePosition(int.Parse(deets[0]) - 1, int.Parse(deets[1]) - 1);

                    if (selectedPiece.Move(movePosition))
                    {
                        board[movePosition] = selectedPiece;

                        board[selectedPosition] = null;

                        board.CalculatePieceMoves();
                    }
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
    private Piece[,] _board;

    public Piece this[PiecePosition p]
    {
        get { return _board[p.row, p.col]; }
        set { _board[p.row, p.col] = value; }
    }

    public int RowColLen { get; private set; } = 8;

    public Board()
    {
        _board = new Piece[RowColLen, RowColLen];
    }

    public void Setup()
    {
        var owner1 = new Player(Guid.NewGuid().ToString(), ConsoleColor.DarkBlue);

        var owner2 = new Player(Guid.NewGuid().ToString(), ConsoleColor.DarkRed);

        this[new PiecePosition(0, 0)] = new Rook(owner1, this);
        this[new PiecePosition(0, 1)] = new Knight(owner1, this);
        this[new PiecePosition(0, 2)] = new Bishop(owner1, this);
        this[new PiecePosition(0, 3)] = new Queen(owner1, this);
        this[new PiecePosition(0, 4)] = new King(owner1, this);
        this[new PiecePosition(0, 5)] = new Bishop(owner1, this);  
        this[new PiecePosition(0, 6)] = new Knight(owner1, this);
        this[new PiecePosition(0, 7)] = new Rook(owner1, this);
        this[new PiecePosition(1, 0)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 1)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 2)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 3)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 4)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 5)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 6)] = new Pawn(owner1, this);
        this[new PiecePosition(1, 7)] = new Pawn(owner1, this);

        this[new PiecePosition(4, 3)] = new Knight(owner1, this);

        this[new PiecePosition(7, 0)] = new Rook(owner2, this);
        this[new PiecePosition(7, 1)] = new Knight(owner2, this);
        this[new PiecePosition(7, 2)] = new Bishop(owner2, this);
        this[new PiecePosition(7, 3)] = new Queen(owner2, this);
        this[new PiecePosition(7, 4)] = new King(owner2, this);
        this[new PiecePosition(7, 5)] = new Bishop(owner2, this);
        this[new PiecePosition(7, 6)] = new Knight(owner2, this);
        this[new PiecePosition(7, 7)] = new Rook(owner2, this);
        this[new PiecePosition(6, 0)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 1)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 2)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 3)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 4)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 5)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 6)] = new Pawn(owner2, this);
        this[new PiecePosition(6, 7)] = new Pawn(owner2, this);

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
                var piece = _board[i, j];

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
                    Console.Write("  E  ");
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

    public void CalculatePieceMoves()
    {
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                var pos = new PiecePosition(i, j);

                if (_board[i, j] != null)
                {
                    _board[i, j].CalculateMoves(pos);
                }
            }
        }
    }
}

public struct PiecePosition
{
    public PiecePosition(int row, int col)
    {
        this.row = row;

        this.col = col;
    }

    public int row;
    public int col;
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

public abstract class Piece
{
    public Player PieceOwner { get; private set; }

    public readonly string PieceName;

    public List<PiecePosition> PossibleMoves { get; protected set; } = new List<PiecePosition>();

    protected readonly Board _board;

    public Piece(Player pieceOwner, Board board, string pieceName = "Piece")
    {
        PieceOwner = pieceOwner;

        _board = board;

        PieceName = pieceName;
    }

    public virtual bool CalculateMoves(PiecePosition move)
    {
        return false;
    }

    public virtual bool Move(PiecePosition move)
    {
        return PossibleMoves.Contains(move);
    }
}

public class Pawn : Piece
{
    private bool _hasMoved = false;

    private int _direction;

    // this is multiplier for first move
    private int _moveMultiplier = 2;

    public Pawn(Player pieceOwner, Board board) 
        : base(pieceOwner, board)
    {
        _direction = pieceOwner.color == ConsoleColor.DarkRed ? -1 : 1;
    }

    public override bool Move(PiecePosition move)
    {
        _hasMoved = true;
        return base.Move(move);
    }

    public override bool CalculateMoves(PiecePosition piecePosition)
    {
        var possibleMoves = new List<PiecePosition>();

        var copy = piecePosition;

        copy.row += _direction;

        if (_board[copy] == null)
        {
            possibleMoves.Add(copy);

            if (!_hasMoved)
            {
                copy.row += _direction;

                if (_board[copy] == null)
                {
                    possibleMoves.Add(copy);
                }
            }
        }

        //reset
        copy = piecePosition;

        copy.row++;

        if (copy.row < _board.RowColLen)
        {
            if (copy.col + 1 < _board.RowColLen && _board[new PiecePosition(copy.row, copy.col + 1)] != null && _board[new PiecePosition(copy.row, copy.col + 1)].PieceOwner.Id != this.PieceOwner.Id)
                possibleMoves.Add(new PiecePosition(copy.row, copy.col + 1));

            if (copy.col - 1 >= 0 && _board[new PiecePosition(copy.row, copy.col - 1)] != null && _board[new PiecePosition(copy.row, copy.col - 1)].PieceOwner.Id != this.PieceOwner.Id)
                possibleMoves.Add(new PiecePosition(copy.row, copy.col - 1));
        }

        PossibleMoves = possibleMoves;

        return true;
    }
}

public class Rook : Piece
{
    public Rook(Player pieceOwner, Board board)
        : base(pieceOwner, board, "Rook")
    {
    }

    public override bool CalculateMoves(PiecePosition piecePosition)
    {
        var possibleMoves = new List<PiecePosition>();

        var copy = piecePosition;

        while (++copy.col < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.col >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.row >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (++copy.row < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        PossibleMoves = possibleMoves;

        return true;
    }
}



public class Knight : Piece
{
    public Knight(Player pieceOwner, Board board)
        : base(pieceOwner, board, "knight")
    {
    }

    public override bool CalculateMoves(PiecePosition piecePosition)
    {
        var possibleMoves = new List<PiecePosition>();

        var copy = piecePosition;

        copy.row += 2;

        if (copy.row < _board.RowColLen)
        {
            copy.col += 1;

            if (copy.col < _board.RowColLen)
            {
                possibleMoves.Add(copy);
            }

            copy.col -= 2;

            if (copy.col >= 0)
            {
                possibleMoves.Add(copy);
            }
        }

        //reset
        copy = piecePosition;

        copy.row -= 2;

        if (copy.row >= 0)
        {
            copy.col += 1;

            if (copy.col < _board.RowColLen)
            {
                possibleMoves.Add(copy);
            }

            copy.col -= 2;

            if (copy.col >= 0)
            {
                possibleMoves.Add(copy);
            }
        }

        //reset
        copy = piecePosition;

        copy.col -= 2;

        if (copy.col >= 0)
        {
            copy.row += 1;

            if (copy.row < _board.RowColLen)
            {
                possibleMoves.Add(copy);
            }

            copy.row -= 2;

            if (copy.row >= 0)
            {
                possibleMoves.Add(copy);
            }
        }

        //reset
        copy = piecePosition;

        copy.col += 2;

        if (copy.col < _board.RowColLen)
        {
            copy.row += 1;

            if (copy.row < _board.RowColLen)
            {
                possibleMoves.Add(copy);
            }

            copy.row -= 2;

            if (copy.row >= 0)
            {
                possibleMoves.Add(copy);
            }
        }

        PossibleMoves = possibleMoves;

        return true;
    }
}

public class Bishop : Piece
{
    public Bishop(Player pieceOwner, Board board)
       : base(pieceOwner, board, "Bishop")
    {

    }

    public override bool CalculateMoves(PiecePosition piecePosition)
    {
        var possibleMoves = new List<PiecePosition>();

        var copy = piecePosition;

        while (++copy.row < _board.RowColLen && --copy.col >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.row >= 0 && --copy.col >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.row >= 0 && ++copy.col < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (++copy.row < _board.RowColLen && ++copy.col < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        PossibleMoves = possibleMoves;

        return true;
    }
}

public class King : Piece
{
    public King(Player pieceOwner, Board board)
      : base(pieceOwner, board, "King")
    {
    }

    public override bool CalculateMoves(PiecePosition move)
    {
        var possibleMoves = new List<PiecePosition>();

        if (move.row + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col)] == null || _board[new PiecePosition(move.row + 1, move.col)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row + 1, move.col));

        if (move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row , move.col + 1)] == null || _board[new PiecePosition(move.row, move.col + 1)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row, move.col + 1));

        if (move.col - 1 >= 0 && (_board[new PiecePosition(move.row, move.col - 1)] == null || _board[new PiecePosition(move.row, move.col - 1)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row, move.col - 1));

        if (move.row - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col)] == null || _board[new PiecePosition(move.row - 1, move.col)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row - 1, move.col));

        if (move.row + 1 < _board.RowColLen && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row + 1, move.col + 1)] == null || _board[new PiecePosition(move.row + 1, move.col + 1)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row + 1, move.col + 1));

        if (move.row + 1 < _board.RowColLen && move.col - 1 >= 0 && (_board[new PiecePosition(move.row + 1, move.col - 1)] == null || _board[new PiecePosition(move.row + 1, move.col - 1)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row + 1, move.col - 1));

        if (move.row - 1 >= 0 && move.col + 1 < _board.RowColLen && (_board[new PiecePosition(move.row - 1, move.col + 1)] == null || _board[new PiecePosition(move.row - 1, move.col + 1)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row - 1, move.col + 1));

        if (move.row - 1 >= 0 && move.col - 1 >= 0 && (_board[new PiecePosition(move.row - 1, move.col - 1)] == null || _board[new PiecePosition(move.row - 1, move.col - 1)].PieceOwner.Id != this.PieceOwner.Id))
            possibleMoves.Add(new PiecePosition(move.row - 1, move.col - 1));

        PossibleMoves = possibleMoves;

        return true;
    }
}

public class Queen : Piece
{

    public Queen(Player pieceOwner, Board board)
      : base(pieceOwner, board, "Queen")
    {

    }

    public override bool CalculateMoves(PiecePosition piecePosition)
    {
        var possibleMoves = new List<PiecePosition>();

        var copy = piecePosition;

        while (++copy.row < _board.RowColLen && --copy.col >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.row >= 0 && --copy.col >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.row >= 0 && ++copy.col < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (++copy.row < _board.RowColLen && ++copy.col < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (++copy.col < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.col >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (--copy.row >= 0)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        //reset
        copy = piecePosition;

        while (++copy.row < _board.RowColLen)
        {
            if (_board[copy] != null)
            {
                if (_board[copy].PieceOwner.Id != this.PieceOwner.Id)
                {
                    // player can take this piece, so it is therefore a possible move.
                    possibleMoves.Add(copy);
                }

                break;
            }

            possibleMoves.Add(copy);
        }

        PossibleMoves = possibleMoves;

        return true;
    }
}