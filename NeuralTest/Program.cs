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
            int[][] layers = new int[3][];

            layers[0] = new int[2];
            layers[1] = new int[4];
            layers[2] = new int[2];

            Network net = new Network(layers);

            net.UpdateData();

            var data = net.GetOutputData();

            for (int i = 0; i < data.Length; i++)
            {
                Console.WriteLine(data[i]);
            }
        }
    }
}
