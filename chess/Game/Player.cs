namespace Chess
{
    public struct Player
    {
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