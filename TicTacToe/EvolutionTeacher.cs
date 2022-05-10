using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class EvolutionTeacher
    {

        private readonly int _populatonSize;
        private readonly double _sigma;
        private readonly double _alpha;

        private Network _network;
        public Network BestNetwork
        {
            get
            {
                if (_network == null)
                {
                    _network = CreateRandomPlayerNetwork();
                }

                return _network;
            }
            private set { _network = value; }
        }

        public EvolutionTeacher(int populatonSize, double sigma, double alpha)
        {
            _populatonSize = populatonSize;
            _sigma = sigma;
            _alpha = alpha;
        }

        public EvolutionTeacher() : this(50, 0.1, 0.001)
        {

        }

        public void Teach(int iteratons = 300)
        {
            var guess = BestNetwork.Save();

            for (int iteraton = 0; iteraton < iteratons; iteraton++)
            {
                var noises = CreateNoises(guess, _populatonSize);

                var results = new double[_populatonSize];

                var populaton = CreatePopulation(guess, noises);

                var tasks = populaton.Select(p1 => populaton.Select(p2 => Play(p1, p2)).ToArray()).ToList();

                tasks.ForEach(t => Task.WaitAll(t));

                var scores = tasks.Select(l => l.Select(t => t.Result).ToList()).ToList();

                for (int i = 0; i < _populatonSize; i++)
                {
                    var score = scores[i].Select(s => (s + 1) / 2.0d).Sum();

                    score += scores.Select(s => s[i]).Select(s => (s * -1 + 1) / 2.0d).Sum();

                    results[i] = score;
                }

                var normalizedResult = Matrix.Divide(Matrix.Minus(results, Matrix.Mean(results)), Matrix.Std(results));



                var koef = _alpha / (_populatonSize * _sigma);
                for (int i = 0; i < guess.Count; i++)
                {
                    var w = guess[i];
                    for (int j = 0; j < w.GetLength(0); j++)
                    {
                        for (int k = 0; k < w.GetLength(1); k++)
                        {
                            guess[i][j, k] = guess[i][j, k] + koef * noises.Select(n => n[i][j, k]).Zip(results).Select(z => z.First * z.Second).Sum();
                        }
                    }
                }

                Console.WriteLine($"{iteraton + 1} / {iteratons}");
            }

            BestNetwork.Load(guess);
        }

        private List<List<double[,]>> CreateNoises(List<double[,]> weights, int populatonSize)
        {
            return Enumerable.Range(0, populatonSize).Select(i => weights.Select(w => Matrix.Randn(w.GetLength(0), w.GetLength(1))).ToList()).ToList();
        }

        private List<Network> CreatePopulation(List<double[,]> weights, List<List<double[,]>> noises)
        {
            var result = new List<Network>();

            foreach (var noise in noises)
            {
                var newWeights = Enumerable.Zip(weights, noise).Select(z => Matrix.Plus(z.First, Matrix.Dot(z.Second, _sigma))).ToList();

                result.Add(CreatePlayerNetwork(newWeights));
            }

            return result;
        }


        private Network CreateRandomPlayerNetwork()
        {
            return CreatePlayerNetwork(new List<double[,]>() { Matrix.Randn(9, 19) });
        }

        private Network CreatePlayerNetwork(List<double[,]> weights)
        {
            Network network = new Network();

            network.AddLayer(new Dense(18, 9, ActivationFunction.Relu));

            network.Load(weights);

            return network;
        }

        private static Task<int> Play(Network network1, Network network2)
        {
            return Task.Run(() =>
            {
                var player1 = new NNPlayer(network1);
                var player2 = new NNPlayer(network2);

                var agent = new TicTacToeAgent(player1, player2);
                var result = agent.Play();

                return (int)result;
            });
        }
    }
}
