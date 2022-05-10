using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class HumanPlayer : IPlayer
    {
        public (int x, int y) GetNextStep(int board, bool isFirstPlayer)
        {
            int x = -1;
            int y = -1;
            var possibleMoves = Board.GetPosiibleMoves(board);
            bool result = false;
            //Console.WriteLine("============================================");
            //Console.WriteLine(Board.DrawBoard(board));

            while (!result)
            {
                var text = Console.ReadLine();

                var splited = text.Split(' ');

                result = splited.Length == 2 && Int32.TryParse(splited[0], out x) && Int32.TryParse(splited[1], out y) && possibleMoves.Any(m => m.Item1 == x && m.Item2 == y);
            }

            return (x, y);
        }
    }
}
