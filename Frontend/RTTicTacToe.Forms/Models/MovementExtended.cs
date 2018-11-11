using System;

namespace RTTicTacToe.Forms.Models
{
    public class MovementExtended : Movement
    {
        public string MovementDetail { get; }

        public MovementExtended(Movement movement, Player player1, Player player2)
        {
            Id = movement.Id;
            PlayerId = movement.PlayerId;
            X = movement.X;
            Y = movement.Y;

            if(PlayerId == player1.Id)
            {
                MovementDetail = $"{player1.Name} - ";
            }
            else if (PlayerId == player2.Id)
            {
                MovementDetail = $"{player2.Name} - ";
            }
            else
            {
                MovementDetail = "Unknown - ";
            }

            MovementDetail += $"({X},{Y})";
        }
    }
}
