using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Network
    {
        private List<ILayer> _layers;

        public Network()
        {
            _layers = new List<ILayer>();
        }

        public void AddLayer(ILayer layer)
        {
            _layers.Add(layer);
        }

        public List<double[,]> Save()
        {
            return _layers.Select(l => l.Weights).ToList();
        }

        public void Load(IEnumerable<double[,]> weights)
        {
            var weightsList = weights.ToList();

            if (weightsList.Count != _layers.Count)
                throw new ArgumentException("weights not equal to layers count");

            for (int i = 0; i < weightsList.Count; i++)
            {
                _layers[i].Weights = weightsList[i];
            }
        }

        public double[] Predict(double[] inputs)
        {
            var output = inputs;

            foreach (var layer in _layers)
            {
                output = layer.GetOutput(output);
            }

            return output;
        }
    }
}
