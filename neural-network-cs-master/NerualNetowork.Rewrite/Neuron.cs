using System;
using System.Collections.Generic;

namespace NerualNetowork.Rewrite
{
    public enum NeruonType
    {
        INPUT,
        HIDDEN,
        BIAS,
        OUTPUT
    }

    class Neuron
    {
        public List<Neuron> prewLayer;

        public double[] Weigths
        {
            get;
            private set;
        }

        public NeruonType Type
        {
            get;
            private set;
        }

        public double OutputData
        {
            get;
            private set;
        }

        public double Error
        {
            get;
            private set;
        }

        int indexInLayer;

        public Neuron(NeruonType type, int _indexInLayer, NeruonType[] Neurons, List<Neuron> _prewLayer)
        {
            indexInLayer = _indexInLayer;
            Type = type;
            prewLayer = _prewLayer;

            if (type == NeruonType.INPUT || type == NeruonType.BIAS)
            {
                if (type == NeruonType.BIAS)
                {
                    OutputData = 1;
                }
                return;
            }

            Weigths = new double[Neurons.Length];

            Random rand = new Random();

            for (int i = 0; i < Weigths.Length; i++)
            {
                Weigths[i] = rand.NextDouble() - 0.5;
            }
        }

        double ActivationFunc(double data)
        {
            return 1 / (1 + Math.Pow(Math.E, -data));
        }

        double DeltaFunc(double y)
        {
            return y * (1 - y);
        }


        public bool DataUpdate()
        {
            if (Type == NeruonType.INPUT || Type == NeruonType.BIAS)
                return false;

            double[] neuron = new double[prewLayer.Count];

            for (int i = 0; i < prewLayer.Count; i++)
            {
                neuron[i] = prewLayer[i].OutputData * Weigths[i];
            }

            double data = 0;

            for (int i = 0; i < neuron.Length; i++)
            {
                data += neuron[i];
            }

            OutputData = ActivationFunc(data);
            return true;
        }
        public bool DataUpdate(double data)
        {
            if (Type != NeruonType.INPUT)
                return false;

            OutputData = ActivationFunc(data);
            return true;
        }
        public void CorrectWeigts(double learnSpeed)
        {
            if (Type == NeruonType.INPUT || Type == NeruonType.BIAS)
                return;

            double deltaError = Error * DeltaFunc(OutputData);

            for (int i = 0; i < prewLayer.Count; i++)
            {
                double correct = learnSpeed * deltaError * prewLayer[i].OutputData;
                Weigths[i] += correct;
            }
        }

        public void FindError(List<Neuron> NextLayer, double needOut = 0)
        {
            Error = 0;

            if (Type == NeruonType.INPUT || Type == NeruonType.BIAS)
                return;

            if (Type == NeruonType.OUTPUT)
            {
                Error = needOut - OutputData;
                return;
            }
            else
            {
                for (int i = 0; i < NextLayer.Count; i++)
                {
                    if (NextLayer[i].Type == NeruonType.BIAS)
                        continue;
                    Error += NextLayer[i].Weigths[indexInLayer] * NextLayer[i].Error;
                }
            }
        }

        public bool WriteWeight(int index, double value)
        {
            if (Type == NeruonType.INPUT || Type == NeruonType.BIAS)
                return false;

            Weigths[index] = value;

            return true;
        }
    }
}
