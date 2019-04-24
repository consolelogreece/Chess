using System.Collections.Generic;

namespace Chess.Moves
{
    public interface IMove
    {
        void MakeMove();

        // return int is the value of the move. i.e. if move takes a knight, it'd be 3 etc..
        float MoveVal();

        BoardTile GetMovePos();

        List<string> GetMoveMeta();

        void UndoMove();
    }
}
