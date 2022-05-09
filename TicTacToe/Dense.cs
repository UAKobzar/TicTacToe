using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Dense : ILayer
    {
        public double[,] Weights { get; set; }

        private Func<double, double> _activationFunction;

        public Dense(int inputs, int length, ActivationFunction activationFunction)
        {
            Weights = new double[length, inputs];

            _activationFunction = GetActivationFunction(activationFunction);
        }

        public Dense(int inputs, int length, Func<double, double> activationFunction)
        {
            Weights = new double[length, inputs];
            _activationFunction = activationFunction;
        }

        private Func<double, double> GetActivationFunction(ActivationFunction activationFunction)
        {
            var tahn = (double x) => { return (Math.Exp(x) - Math.Exp(-x)) / (Math.Exp(x) + Math.Exp(-x)); };

            switch (activationFunction)
            {
                case ActivationFunction.Identity:
                    return (double x) => { return x; };
                case ActivationFunction.BinaryStep:
                    return (double x) => { return x < 0 ? 0 : 1; };
                case ActivationFunction.Logistic:
                    return (double x) => { return 1 / (1 + Math.Exp(-x)); };
                case ActivationFunction.Tanh:
                    return tahn;
                case ActivationFunction.Relu:
                    return (double x) => { return x < 0 ? 0 : x; };
                case ActivationFunction.GELU:
                    throw new NotImplementedException();
                case ActivationFunction.Softplus:
                    return (double x) => { return Math.Log(1 + Math.Exp(x)); };
                case ActivationFunction.ELU:
                    throw new NotImplementedException();
                case ActivationFunction.SELU:
                    throw new NotImplementedException();
                case ActivationFunction.LeackyRelu:
                    return (double x) => { return x < 0 ? 0.01 * x : x; }; ;
                case ActivationFunction.PRELU:
                    throw new NotImplementedException();
                case ActivationFunction.Sigmoid:
                    return (double x) => { return x / (1 + Math.Exp(x)); };
                case ActivationFunction.Mish:
                    return (double x) => { return x * tahn(Math.Log(1 + Math.Exp(x))); };
                case ActivationFunction.Gaussian:
                    return (double x) => { return Math.Exp(-1 * x * x); };
            }

            throw new ArgumentException("Invalid Activation function");
        }

        public double[] GetOutput(double[] input)
        {
            if (input?.Length != Weights.GetLength(1))
                throw new ArgumentException("Wrong input length");

            var result = new double[Weights.GetLength(0)];

            for (int i = 0; i < Weights.GetLength(0); i++)
            {
                for (int j = 0; j < Weights.GetLength(1); j++)
                {
                    result[i] += input[j] * Weights[i, j];
                }

                result[i] = _activationFunction(result[i]);
            }

            return result;
        }
    }

    enum ActivationFunction
    {
        Identity,
        BinaryStep,
        Logistic,
        Tanh,
        Relu,
        GELU,
        Softplus,
        ELU,
        SELU,
        LeackyRelu,
        PRELU,
        Sigmoid,
        Mish,
        Gaussian
    }
}
