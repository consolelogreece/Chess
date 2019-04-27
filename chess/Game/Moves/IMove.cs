using System.Collections.Generic;

namespace Chess.Moves
{
    public interface IMove
    {
        void MakeMove();

        BoardTile GetMovePos();

        List<string> GetMoveMeta();

        void UndoMove();
    }
}
