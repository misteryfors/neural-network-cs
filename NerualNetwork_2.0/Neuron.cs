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
        OUTPUT
    }
    class Neuron
    {
        public List<Neuron> prewLayer;

        public double[] Weigts
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

        int indexInLayer;

        public Neuron(NeuronTypes type, NeuronTypes[] prewLayerTypes, List<Neuron> _prewLayer, int _indexInLayer, double stdWeight = 0.1)
        {
            indexInLayer = _indexInLayer;

            Type = type;

            prewLayer = _prewLayer;
            
            if (type == NeuronTypes.INPUT || type == NeuronTypes.BIAS)
            {
                if (type == NeuronTypes.BIAS)
                {
                    OutputData = 1;
                }
                return;
            }

            Weigts = new double[prewLayerTypes.Length];
            for (int i = 0; i < Weigts.Length; i++)
            {
                Weigts[i] = stdWeight;
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
                neuron[i] = prewLayer[i].OutputData * Weigts[i];
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

            for (int i = 0; i < prewLayer.Count; i++)
            {
                double correct = learnSpeed * deltaError * prewLayer[i].OutputData;
                Weigts[i] += correct;
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
                    Error += NextLayer[i].Weigts[indexInLayer] * NextLayer[i].Error;
                }
            }
        }
    }
}
