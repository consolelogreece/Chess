using System;
using System.Collections.Generic;
using System.Linq;
using Chess;
using Chess.Pieces;
using Xunit;

namespace Test.Pieces
{
    public class PawnTests
    {
        [Fact]
        public void CanMoveTwoSpacesOnFirstMove()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn = new Pawn(player1, game.Board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            //act
            game.Setup(pieces);
            
            //assert
            Assert.Equal(2, pawn.PossibleMoves.Count);
        }

        [Fact]
        public void CanOnlyMoveOneSpaceAfterFirstMove()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn = new Pawn(player1, game.Board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            //act
            game.Setup(pieces);

            var move = new PiecePosition(2,0);

            game.Move(pawn.CurrentPosition, move, false);
            
            //assert
            Assert.Equal(1, pawn.PossibleMoves.Count);
        }

        [Fact]
        public void CanOnlyMoveForward()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn1 = new Pawn(player1, game.Board, new PiecePosition(3,0));
            var pawn2 = new Pawn(player2, game.Board, new PiecePosition(3,7));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            //act
            game.Setup(pieces);

            var pawn1HasMoveThatIsntForward = false;

            foreach(var move in pawn1.PossibleMoves)
            {
                if (move.GetMovePos().Position.col != pawn1.CurrentPosition.col || move.GetMovePos().Position.row < pawn1.CurrentPosition.row)
                {
                    pawn1HasMoveThatIsntForward = true;

                    break;
                }
            }

            var pawn2HasMoveThatIsntForward = false;

            foreach(var move in pawn2.PossibleMoves)
            {
                if (move.GetMovePos().Position.col != pawn2.CurrentPosition.col || move.GetMovePos().Position.row > pawn2.CurrentPosition.row)
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
           //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn1 = new Pawn(player1, game.Board, new PiecePosition(3,0));
            var pawn2 = new Pawn(player2, game.Board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            //act
            game.Setup(pieces);
            
            //assert
            Assert.Contains(pawn2, game.Board[pawn1.CurrentPosition].ThreateningPieces);
            Assert.Contains(pawn1, game.Board[pawn2.CurrentPosition].ThreateningPieces);
        }

        [Fact]
        public void EnPassantFlagEnabledOnDoubleMove()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn = new Pawn(player1, game.Board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            //act
            game.Setup(pieces);

            var move = new PiecePosition(2,0);

            game.Move(pawn.CurrentPosition, move, false);            
            
            //assert
            Assert.True(pawn.EnPassant);
        }

        [Fact]
        public void EnPassantFlagDisabledOnSingleMove()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn = new Pawn(player1, game.Board, new PiecePosition(0,0));
            
            var pieces = new List<Piece>(){ pawn };

            //act
            game.Setup(pieces);

            var move = new PiecePosition(1,0);

            game.Move(pawn.CurrentPosition, move, false);            
            
            //assert
            Assert.False(pawn.EnPassant);
        }

        [Fact]
        public void CanMoveEnPassant()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn1 = new Pawn(player1, game.Board, new PiecePosition(2,0));
            var pawn2 = new Pawn(player2, game.Board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            //act
            game.Setup(pieces);

            var move = new PiecePosition(4,0);

            game.Move(pawn1.CurrentPosition, move, false);

            //assert
            var enPassantPos = new PiecePosition(3,0);
            var canTakeEnPassant = pawn2.PossibleMoves.Any(m => m.GetMovePos().Position == enPassantPos);

            Assert.True(canTakeEnPassant);
        }

        [Fact]
        public void CanTakeEnPassant()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var pawn1 = new Pawn(player1, game.Board, new PiecePosition(2,0));
            var pawn2 = new Pawn(player2, game.Board, new PiecePosition(4,1));
            
            var pieces = new List<Piece>(){ pawn1, pawn2 };

            //act
            game.Setup(pieces);

            var move = new PiecePosition(4,0);

            game.Move(pawn1.CurrentPosition, move, false);

            var enPassantPos = new PiecePosition(3,0);

            game.Move(pawn2.CurrentPosition, enPassantPos, false);

            //assert
            Assert.Null(game.Board[pawn1.CurrentPosition].OccupyingPiece);
        }

        [Fact]
        public void CanOnlyMoveForwardWhenVerticallyPinned()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            var p1Pawn1 = new Pawn(player1, game.Board, new PiecePosition(3,3));
            var p1King1 = new King(player1, game.Board, new PiecePosition(0,3));
            var p2Rook1 = new Rook(player2, game.Board, new PiecePosition(6,3));

            // add takeable piece to add possibility of moving out of current col.
            var p2Pawn1 = new Pawn(player2, game.Board, new PiecePosition(4,4));


            var p1Pawn2 = new Pawn(player1, game.Board, new PiecePosition(3,5));
            var p1King2 = new King(player1, game.Board, new PiecePosition(1,5));
            var p2Rook2 = new Rook(player2, game.Board, new PiecePosition(7,5));

            // add takeable piece to add possibility of moving out of current col.
            var p2Pawn2 = new Pawn(player2, game.Board, new PiecePosition(4,6));
            
            var pieces = new List<Piece>(){ p1Pawn1, p1King1, p2Rook1, p2Pawn1, p1Pawn2, p1King2, p2Rook2, p2Pawn2};    

            //act
            game.Setup(pieces);

            //assert
            Assert.NotEmpty(p1Pawn1.PossibleMoves);

            var pinnedPawn1CanOnlyMoveForward = !p1Pawn1.PossibleMoves.Any(m => m.GetMovePos().Position.col != p1Pawn1.CurrentPosition.col);

            var pinnedPawn2CanOnlyMoveForward = !p1Pawn2.PossibleMoves.Any(m => m.GetMovePos().Position.col != p1Pawn2.CurrentPosition.col);

            Assert.True(pinnedPawn1CanOnlyMoveForward);

            Assert.True(pinnedPawn2CanOnlyMoveForward)
;        }

        [Fact]
        public void CantMoveWhenHorizontallyPinned()
        {
            //arrange
            var player1 = new Player(Guid.NewGuid().ToString(), "top");
            var player2 = new Player(Guid.NewGuid().ToString(), "bottom");

            var game = new Game(new List<Player>(){ player1, player2 });

            // create pin from right to left.
            var p1Pawn1 = new Pawn(player1, game.Board, new PiecePosition(3,3));
            var p1King1 = new King(player1, game.Board, new PiecePosition(3,0));
            var p2Rook1 = new Rook(player2, game.Board, new PiecePosition(3,6));

            // create pin from left to right.
            var p1Pawn2 = new Pawn(player1, game.Board, new PiecePosition(4,4));
            var p1King2 = new King(player1, game.Board, new PiecePosition(4,7));
            var p2Rook2 = new Rook(player2, game.Board, new PiecePosition(4,1));
         
            var pieces = new List<Piece>(){ p1Pawn1, p1King1, p2Rook1, p1Pawn2, p1King2, p2Rook2};

            //act
            game.Setup(pieces);

            //assert
            Assert.Empty(p1Pawn1.PossibleMoves);

            Assert.Empty(p1Pawn2.PossibleMoves);
        }
    }
}