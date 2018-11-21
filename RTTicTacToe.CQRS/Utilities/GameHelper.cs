using System;
using RTTicTacToe.CQRS.ReadModel.Dtos;
using RTTicTacToe.CQRS.ReadModel.Infrastructure;

namespace RTTicTacToe.CQRS.Utilities
{
    public static class GameHelper
    {
        public static bool CheckGameFinished(GameDto game)
        {
            int[,] gameBoard = new int[3, 3];

            foreach (var mov in game.Movements)
            {
                gameBoard[mov.X, mov.Y] = (mov.PlayerId == game.Player1.Id ? 1 : 2);
            }

            return CheckAllRows(gameBoard) || CheckAllDiagonals(gameBoard);
        }

        public static bool CheckGameFinished(int[,] gameBoard)
        {
            return CheckAllRows(gameBoard) || CheckAllDiagonals(gameBoard);
        }

        private static bool CheckAllRows(int[,] gameBoard)
        {
            int countforP1 = 0;
            int countforP2 = 0;

            for (int i = 0; i < 3; i++)
            {
                countforP1 = 0;
                countforP2 = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (gameBoard[i, j] == 1)
                    {
                        countforP1++;
                    }
                    if (gameBoard[i, j] == 2)
                    {
                        countforP2++;
                    }

                    if (countforP1 == 3)
                    {
                        //Player 1 Wins
                        return true;
                    }
                    if (countforP2 == 3)
                    {
                        //Player 2 Wins
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool CheckAllDiagonals(int[,] gameBoard)
        {
            int countforP1 = 0;
            int countforP2 = 0;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == j && gameBoard[j, i] == 1)
                    {
                        countforP1++;
                    }

                    if (i == j && gameBoard[j, i] == 2)
                    {
                        countforP2++;
                    }

                    if (countforP1 == 3)
                    {
                        //Player 1 Wins
                        return true;
                    }
                    if (countforP2 == 3)
                    {
                        //Player 2 Wins
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
