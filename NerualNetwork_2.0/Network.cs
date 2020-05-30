using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork_2_0
{
    public class Network
    {
        List<Neuron>[] layers;
        double learnSpeed;
        public Network(NeuronTypes[][] Maket, double _learnSpeed = 0.1, double stdWeight = 0.1)
        {

            learnSpeed = _learnSpeed;

            layers = new List<Neuron>[Maket.Length];

            for (int i = 0; i < Maket.Length; i++)
            {
                layers[i] = new List<Neuron>(Maket[i].Length);
                for (int j = 0; j < Maket[i].Length; j++)
                {
                    if (i == 0)
                    {
                        layers[i].Add(new Neuron(Maket[i][j], null, null, j, stdWeight));
                    }
                    else
                    {
                        layers[i].Add(new Neuron(Maket[i][j], Maket[i - 1], layers[i - 1], j, stdWeight));
                    }
                }
            }
        }

        public double[] GetOutputData()
        {
            int lastLayer = layers.Length - 1;
            double[] output = new double[layers[lastLayer].Count];

            for (int i = 0; i < layers[lastLayer].Count; i++)
            {
                output[i] = layers[lastLayer][i].OutputData;
            }
            return output;
        }

        public void SetInputData(double[] inputData)
        {
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count; i++)
            {
                if (layers[inputLayer][i].Type == NeuronTypes.BIAS) continue;
                
                layers[inputLayer][i].DataUpdate(inputData[i]);
            }
        }

        public void UpdateData()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0) continue;
                for (int j = 0; j < layers[i].Count; j++)
                {
                    layers[i][j].DataUpdate();
                }
            }
        }

        public void Learn(double[] needOut)
        {
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].FindError(null, needOut[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].FindError(layers[i + 1]);
                    }
                }
            }

            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].CorrectWeigts(learnSpeed);
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].CorrectWeigts(learnSpeed);
                    }
                }
            }
        }
    }
}
