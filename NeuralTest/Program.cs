using System;
using System.Collections.Generic;
using NeuralNetwork;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace NeuralTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double[][] needOut = new double[4][];

            double[][] input = new double[4][];
            /*-------------TEST-DATA------------*/

            needOut[0] = new double[] { 0 };
            needOut[1] = new double[] { 0 };
            needOut[2] = new double[] { 0 };
            needOut[3] = new double[] { 1 };

            input[0] = new double[] { 0, 0 };
            input[1] = new double[] { 1, 0 };
            input[2] = new double[] { 0, 1 };
            input[3] = new double[] { 1, 1 };

            /*----------END-TEST-DATA-----------*/

            int[][] layers = new int[2][];

            layers[0] = new int[2];
            layers[1] = new int[1];

            Network net = new Network(layers, 0.2, 0.1);

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine($"Эпоха {i + 1}");
                for (int j = 0; j < input.Length; j++)
                {
                    Console.WriteLine($"\tНабор {j + 1}");
                    net.SetInputData(input[j]);
                    net.UpdateData();

                    var data = net.GetOutputData();

                    for (int k = 0; k < data.Length; k++)
                    {
                        Console.WriteLine(data[k]);
                    }

                    net.Learn(needOut[j]);
                }
            }
        }
    }
}
