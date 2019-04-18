using System;
using System.Collections.Generic;
using System.Linq;
using chess.pieces;
using Xunit;

namespace Test.Pieces
{
    public class PawnTests
    {
        [Fact]
        public void CanMoveTwoSpacesOnFirstMove()
        {
            //arrange
            var board = new Board();

            var player = new Player(Guid.NewGuid().ToString(), "top");

            var pawn = new Pawn(player, board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();
            
            //assert
            Assert.Equal(2, pawn.PossibleMoves.Count);
        }

        [Fact]
        public void CanOnlyMoveOneSpaceAfterFirstMove()
        {
            //arrange
            var board = new Board();

            var player = new Player(Guid.NewGuid().ToString(), "top");

            var pawn = new Pawn(player, board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            var move = new PiecePosition(2,0);

            pawn.Move(move);

            board.CalculatePieceMoves();            
            
            //assert
            Assert.Equal(1, pawn.PossibleMoves.Count);
        }

        [Fact]
        public void CanOnlyMoveForward()
        {
            //arrange
            var board = new Board();

            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var pawn1 = new Pawn(player1, board, new PiecePosition(3,0));
            var pawn2 = new Pawn(player2, board, new PiecePosition(3,7));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            var pawn1HasMoveThatIsntForward = false;

            foreach(var move in pawn1.PossibleMoves)
            {
                if (move.col != pawn1.CurrentPosition.col || move.row < pawn1.CurrentPosition.row)
                {
                    pawn1HasMoveThatIsntForward = true;

                    break;
                }
            }

            var pawn2HasMoveThatIsntForward = false;

            foreach(var move in pawn2.PossibleMoves)
            {
                if (move.col != pawn2.CurrentPosition.col || move.row > pawn2.CurrentPosition.row)
                {
                    pawn2HasMoveThatIsntForward = true;

                    break;
                }
            }
            
            //assert
            Assert.False(pawn1HasMoveThatIsntForward);
            Assert.False(pawn2HasMoveThatIsntForward);
        }
    
        [Fact]
        public void CanMoveDiagonallyForwardOneSquareWhenTaking()
        {
            var board = new Board();

            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var pawn1 = new Pawn(player1, board, new PiecePosition(3,0));
            var pawn2 = new Pawn(player2, board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();
            
            //assert
            Assert.Contains(pawn2, board[pawn1.CurrentPosition].ThreateningPieces);
            Assert.Contains(pawn1, board[pawn2.CurrentPosition].ThreateningPieces);
        }

        [Fact]
        public void EnPassantFlagEnabledOnDoubleMove()
        {
             //arrange
            var board = new Board();

            var player = new Player(Guid.NewGuid().ToString(), "top");

            var pawn = new Pawn(player, board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            var move = new PiecePosition(2,0);

            pawn.Move(move);            
            
            //assert
            Assert.True(pawn.EnPassant);
        }

        [Fact]
        public void EnPassantFlagDisabledOnSingleMove()
        {
             //arrange
            var board = new Board();

            var player = new Player(Guid.NewGuid().ToString(), "top");

            var pawn = new Pawn(player, board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            var move = new PiecePosition(1,0);

            pawn.Move(move);            
            
            //assert
            Assert.False(pawn.EnPassant);
        }

        [Fact]
        public void CanMoveEnPassant()
        {
            //arrange
            var board = new Board();

            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var pawn1 = new Pawn(player1, board, new PiecePosition(2,0));
            var pawn2 = new Pawn(player2, board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            var move = new PiecePosition(4,0);

            pawn1.Move(move);

            board.CalculatePieceMoves();

            //assert
            var enPassantPos = new PiecePosition(3,0);
            var canTakeEnPassant = pawn2.PossibleMoves.Any(m => m == enPassantPos);

            Assert.True(canTakeEnPassant);
        }

        [Fact]
        public void CanTakeEnPassant()
        {
            //arrange
            var board = new Board();

            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var pawn1 = new Pawn(player1, board, new PiecePosition(2,0));
            var pawn2 = new Pawn(player2, board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            var move = new PiecePosition(4,0);

            pawn1.Move(move);

            board.CalculatePieceMoves();

            var enPassantPos = new PiecePosition(3,0);

            pawn2.Move(enPassantPos);

            //assert
            Assert.Null(board[pawn1.CurrentPosition].OccupyingPiece);
        }

        [Fact]
        public void CanOnlyMoveForwardWhenVerticallyPinned()
        {
            //arrange
            var board = new Board();

            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var p1Pawn1 = new Pawn(player1, board, new PiecePosition(3,3));
            var p1King1 = new King(player1, board, new PiecePosition(0,3));
            var p2Rook1 = new Rook(player2, board, new PiecePosition(6,3));

            // add takeable piece to add possibility of moving out of current col.
            var p2Pawn1 = new Pawn(player2, board, new PiecePosition(4,4));


            var p1Pawn2 = new Pawn(player1, board, new PiecePosition(3,5));
            var p1King2 = new King(player1, board, new PiecePosition(1,5));
            var p2Rook2 = new Rook(player2, board, new PiecePosition(7,5));

            // add takeable piece to add possibility of moving out of current col.
            var p2Pawn2 = new Pawn(player2, board, new PiecePosition(4,6));
            
            var pieces = new List<Piece>(){ p1Pawn1, p1King1, p2Rook1, p2Pawn1, p1Pawn2, p1King2, p2Rook2, p2Pawn2};

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            //assert
            Assert.NotEmpty(p1Pawn1.PossibleMoves);

            var pinnedPawn1CanOnlyMoveForward = !p1Pawn1.PossibleMoves.Any(m => m.col != p1Pawn1.CurrentPosition.col);

            var pinnedPawn2CanOnlyMoveForward = !p1Pawn2.PossibleMoves.Any(m => m.col != p1Pawn2.CurrentPosition.col);

            Assert.True(pinnedPawn1CanOnlyMoveForward);

            Assert.True(pinnedPawn2CanOnlyMoveForward)
;        }

        [Fact]
        public void CantMoveWhenHorizontallyPinned()
        {
            //arrange
            var board = new Board();

            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            // create pin from right to left.
            var p1Pawn1 = new Pawn(player1, board, new PiecePosition(3,3));
            var p1King1 = new King(player1, board, new PiecePosition(3,0));
            var p2Rook1 = new Rook(player2, board, new PiecePosition(3,6));

            // create pin from left to right.
            var p1Pawn2 = new Pawn(player1, board, new PiecePosition(4,4));
            var p1King2 = new King(player1, board, new PiecePosition(4,7));
            var p2Rook2 = new Rook(player2, board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ p1Pawn1, p1King1, p2Rook1, p1Pawn2, p1King2, p2Rook2};

            board.Setup(pieces);

            //act
            board.CalculatePieceMoves();

            //assert
            Assert.Empty(p1Pawn1.PossibleMoves);

            Assert.Empty(p1Pawn2.PossibleMoves);
        }
    }
}