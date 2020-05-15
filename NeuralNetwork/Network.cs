using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


// 188 строк кода всего
namespace NeuralNetwork
{
    public class Network
    {
        List<Neuron>[] layers;
        public Network(int[][] _layers)
        {
            layers = new List<Neuron>[_layers.Length];
            for (int i = 0; i < _layers.Length; i++)
            {
                layers[i] = new List<Neuron>(_layers[i].Length);
                for (int j = 0; j < _layers[i].Length; j++)
                {
                    if (i == 0)
                    {
                        layers[i].Add(new Neuron(null));
                    }
                    else
                    {
                        layers[i].Add(new Neuron(layers[i - 1]));
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

        public void SetInputData(double[] inputLayerData)
        {
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count; i++)
            {
                layers[inputLayer][i].SetData(inputLayerData[i]);
            }
        }

        public void UpdateData()
        {
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0) continue;
                for (int j = 0; j < layers[i].Count; j++)
                {
                    layers[i][j].UpdateInfo();
                }
            }
        }
    }
}
