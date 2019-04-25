using Chess.Moves;

namespace Chess.Helpers.Moves
{
    public static class AIHelpers
    {
        public static IMove Minimax(Game game, int depth)
        {
            var newGameMoves = game.GetAllMoves(game.NextMovePlayer);
            var bestMove =  float.MinValue;
            IMove bestMoveFound = null;

            var alpha = float.MinValue;
            var beta = float.MaxValue;

            for(var i = 0; i < newGameMoves.Count; i++) {
                var newGameMove = newGameMoves[i];
                game.Move(newGameMove);
                var value = Minimax(game, depth - 1, alpha, beta, true);
                game.Undo(false);
                if(value >= bestMove) {
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
                return -game.Board.EvaluateBoard();
            }

            var newGameMoves = game.GetAllMoves(game.NextMovePlayer);

            if (isMaximisingPlayer) {
                var maxEval = float.MinValue; 
                for (var i = 0; i < newGameMoves.Count; i++) {
                    game.Move(newGameMoves[i], false);
                    var eval = Minimax(game, depth - 1, alpha, beta, false);
                    game.Undo(false);
                    maxEval = maxEval < eval ? eval : maxEval;
                    alpha = alpha < eval ? eval : alpha;
                    if (beta <= alpha) return maxEval;
                }

                return maxEval;
            } else {
                var minEval = float.MaxValue;
                for (var i = 0; i < newGameMoves.Count; i++) {
                    game.Move(newGameMoves[i], false);
                    var eval = Minimax(game, depth - 1,alpha, beta, true);
                    game.Undo(false);
                    minEval = minEval > eval ? eval : minEval;
                    beta = beta > eval ? eval : beta;
                    if (beta <= alpha) {
                        return minEval;
                    }
                }
                return minEval;
            }
        }
    }
}