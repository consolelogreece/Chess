namespace Chess.Moves
{
    public interface IMove
    {
        // return int is the value of the move. i.e. if move takes a knight, it'd be 3 etc..
        int MakeMove();

        BoardTile GetMovePos();

        //todo: make makemove void, and make getvalue method.
    }
}       
        
