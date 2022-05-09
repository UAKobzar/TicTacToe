using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class NNPlayer : IPlayer
    {
        private readonly Network _network;

        public  NNPlayer(Network network)
        {
            _network = network;
        }


        public (int x, int y) GetNextStep(int board)
        {
            var inputs = BoardToInput(board);
            var outputs = _network.Predict(inputs);
            return OutputsToMove(outputs, board);
        }

        private double[] BoardToInput(int board)
        {
            double[] result = new double[18];

            for (int i = 0; i < 18; i++)
            {
                result[i] = board & 1;
                board >>= 1;
            }

            return result;
        }

        private (int x, int y) OutputsToMove(double[] outputs, int board)
        {
            var possibleMoves = Board.GetPosiibleMoves(board);

            var move = possibleMoves.Select(m => new { move = m, prop = outputs[m.Item1 * 3 + m.Item2] }).OrderByDescending(m=>m.prop).First().move;

            return (move.Item1, move.Item2);
        }
    }
}
