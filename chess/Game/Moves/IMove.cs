using System.Collections.Generic;

namespace Chess.Moves
{
    public interface IMove
    {
        // return int is the value of the move. i.e. if move takes a knight, it'd be 3 etc..
        void MakeMove();

        float MoveVal();

        BoardTile GetMovePos();

        List<string> GetMoveMeta();

        void UndoMove();
    }
}       
        
