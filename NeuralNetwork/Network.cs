using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


namespace NeuralNetwork
{
    

    public class Network
    {
        List<Neuron>[] layers;
        double learnSpeed;
        public Network(int[][] _layers, double _learnSpeed = 0.1, double stdWeight = 0.1)
        {
            learnSpeed = _learnSpeed;
            layers = new List<Neuron>[_layers.Length];
            for (int i = 0; i < _layers.Length; i++)
            {
                layers[i] = new List<Neuron>(_layers[i].Length);
                for (int j = 0; j < _layers[i].Length; j++)
                {

                    /*
                     Косяк полный
                     надо снести полность биас и сделать заново
                     
                     */
                    if (i == 0)
                    {
                        //if (j != _layers[i].Length - 1)
                            layers[i].Add(new Neuron(null, stdWeight));
                        //else
                        //    layers[i].Add(new Neuron(null, true));
                    }
                    else
                    {
                        //if (j != _layers[i].Length - 1)
                            layers[i].Add(new Neuron(layers[i - 1], stdWeight));
                        //else
                        //{
                        //    if(i != _layers.Length)
                        //    {
                        //        layers[i].Add(new Neuron(null, true));
                        //    }
                        //    else
                        //    {
                        //        layers[i].Add(new Neuron(layers[i - 1], false, stdWeight));
                        //    }
                        //}
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

        public void Learn(double[] needOut)
        {
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].FindError(true, null, 0, needOut[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].FindError(false, layers[i + 1], j);
                    }
                }
            }

            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].Correct(learnSpeed);
                    }
                }
                else
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].Correct(learnSpeed);
                    }
                }
            }
        }

        public double ErrorArif()
        {
            double err = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                    continue;

                for (int j = 0; j < layers[i].Count; j++)
                {
                    err += layers[i][j].Error;
                }
            }

            int count = 0;

            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                    continue;

                for (int j = 0; j < layers[i].Count; j++)
                {
                    count++;
                }
            }
            return err /= count;
        }
    }
}
