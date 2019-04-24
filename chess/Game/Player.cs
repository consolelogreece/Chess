namespace Chess
{
    public struct Player
    {
        // todo: store list of pieces by type so we don't have to loop board to find specific pieces. use hash map.
        public readonly string Id;

        public readonly string Side;

        public Player(string Id, string side)
        {
            this.Id = Id;

            this.Side = side;
        }

        public static bool operator ==(Player player1, Player player2)
        {
            return player1.Id == player2.Id;
        }

        public static bool operator !=(Player player1, Player player2)
        {
            return (player1.Id == player2.Id) == false;
        }
    }
}
