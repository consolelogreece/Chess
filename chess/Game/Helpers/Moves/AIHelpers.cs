using Chess.Moves;

namespace Chess.Helpers.Moves
{
    public static class AIHelpers
    {
        public static IMove Minimax(Game game, int depth)
        {
            var newGameMoves = game.GetAllMoves(game.NextMovePlayer);
            var bestMove = float.MinValue;
            IMove bestMoveFound = null;

            var alpha = float.MinValue;
            var beta = float.MaxValue;

            for (var i = 0; i < newGameMoves.Count; i++)
            {
                var newGameMove = newGameMoves[i];
                game.Move(newGameMove, false);
                var value = Minimax(game, depth - 1, alpha, beta, false);
                game.Undo();
                if (value >= bestMove)
                {
                    bestMove = value;
                    bestMoveFound = newGameMove;
                }
            }
            return bestMoveFound;
        }

        private static float Minimax(Game game, int depth, float alpha, float beta, bool isMaximisingPlayer)
        {
            if (depth == 0)
            {
                var val = game.Board.EvaluateBoard(game.NextMovePlayer, game.GetNextPlayer(game.NextMovePlayer));

                return val;
            }

            var newGameMoves = game.GetAllMoves(game.NextMovePlayer);

            if (isMaximisingPlayer)
            {
                var maxEval = float.MinValue;
                for (var i = 0; i < newGameMoves.Count; i++)
                {
                    game.Move(newGameMoves[i], false);
                    var eval = Minimax(game, depth - 1, alpha, beta, false);
                    game.Undo();
                    maxEval = maxEval < eval ? eval : maxEval;
                    alpha = alpha < eval ? eval : alpha;
                    if (beta <= alpha)return maxEval;
                }

                return maxEval;
            }
            else
            {
                var minEval = float.MaxValue;
                for (var i = 0; i < newGameMoves.Count; i++)
                {
                    game.Move(newGameMoves[i], false);
                    var eval = Minimax(game, depth - 1, alpha, beta, true);
                    game.Undo();
                    minEval = minEval > eval ? eval : minEval;
                    beta = beta > eval ? eval : beta;
                    if (beta <= alpha)
                    {
                        return minEval;
                    }
                }
                return minEval;
            }
        }
    }
}
