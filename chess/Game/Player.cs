using System;
using System.Collections.Generic;
using Chess.Pieces;

namespace Chess
{
    public struct Player
    {
        // todo: store list of pieces by type so we don't have to loop board to find specific pieces. use hash map.
        public readonly string Id;

        public readonly string Side;

        public readonly bool _isAI;

        public Player(string Id, string side, bool isAI = false)
        {
            this.Id = Id;

            this.Side = side;

            _isAI = isAI;
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
