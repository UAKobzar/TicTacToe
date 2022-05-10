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


        public (int x, int y) GetNextStep(int board, bool isFirstPlayer)
        {
            var inputs = BoardToInput(board, isFirstPlayer);
            var outputs = _network.Predict(inputs);
            return OutputsToMove(outputs, board);
        }

        private double[] BoardToInput(int board, bool isFirstPlayer)
        {
            double[] result = new double[18];

            for (int i = 0; i < 18; i++)
            {
                result[i] = board & 1;
                board >>= 1;
            }

            for (int i = 0; i < 18; i+=2)
            {
                result[i] = isFirstPlayer ? (result[i] == 0 ? 1 : 0) : result[i];
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
