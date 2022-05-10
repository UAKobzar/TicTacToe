using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    interface IPlayer
    {
        (int x, int y) GetNextStep(int board, bool isFirstPlayer);
    }
}
