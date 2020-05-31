using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork_2_0
{
    public enum NeuronTypes
    {
        INPUT,
        HIDDEN,
        BIAS,
        OUTPUT,
        ELMAN
    }

    class Neuron
    {
        public List<Neuron> prewLayer;

        Neuron elmanNeuron;

        public double[] Weigths
        {
            get;
            private set;
        }

        public NeuronTypes Type
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

        bool isElmanNet;

        int indexInLayer;

        public Neuron(NeuronTypes type, NeuronTypes[] prewLayerTypes, List<Neuron> _prewLayer, int _indexInLayer, bool _isElmanNet = false)
        {
            indexInLayer = _indexInLayer;

            Type = type;

            prewLayer = _prewLayer;

            isElmanNet = _isElmanNet;

            if (type == NeuronTypes.INPUT || type == NeuronTypes.BIAS)
            {
                if (type == NeuronTypes.BIAS)
                {
                    OutputData = 1;
                }
                return;
            }

            if (type == NeuronTypes.HIDDEN && isElmanNet)
            {
                List<Neuron> thisNeuron = new List<Neuron>();
                thisNeuron.Add(this);
                elmanNeuron = new Neuron(NeuronTypes.ELMAN, null, thisNeuron, 0, true);
                Weigths = new double[prewLayerTypes.Length + 1];
            }
            else
            {
                Weigths = new double[prewLayerTypes.Length];
            }

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

        public bool DataUpdate()
        {
            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
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

            if (Type == NeuronTypes.HIDDEN && isElmanNet)
            {
                elmanNeuron.DataUpdate();
            }

            return true;
        }
        public bool DataUpdate(double data)
        {
            if (Type != NeuronTypes.INPUT)
                return false;

            OutputData = ActivationFunc(data);
            return true;
        }
        public void CorrectWeigts(double learnSpeed)
        {
            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
                return;

            double deltaError = Error * (OutputData * (1 - OutputData));

            int countInLayer = prewLayer.Count;
            
            if (isElmanNet && Type == NeuronTypes.HIDDEN)
            {
                countInLayer++;
            }

            for (int i = 0; i < countInLayer; i++)
            {
                double correct;
                if (isElmanNet && Type == NeuronTypes.HIDDEN && i == countInLayer - 1)
                {
                    correct = learnSpeed * deltaError * elmanNeuron.OutputData;
                }
                else
                {
                    correct = learnSpeed * deltaError * prewLayer[i].OutputData;
                }

                Weigths[i] += correct;
            }
        }

        public void FindError(List<Neuron> NextLayer, double needOut = 0)
        {
            Error = 0;

            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
                return;

            if (Type == NeuronTypes.OUTPUT)
            {
                Error = needOut - OutputData;
                return;
            }
            else
            {
                for (int i = 0; i < NextLayer.Count; i++)
                {
                     Error += NextLayer[i].Weigths[indexInLayer] * NextLayer[i].Error;
                }
            }
        }

        public void ElmanDataSet()
        {
            if (Type != NeuronTypes.ELMAN)
                return;

            OutputData = ActivationFunc(0);
        }
    }
}
