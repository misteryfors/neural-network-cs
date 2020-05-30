using System;
using System.Collections.Generic;
using NerualNetwork_2_0;
using System.Threading;
using System.IO;

namespace NetTest_2_0
{
    class Program
    {
        static void Main(string[] args)
        {

            StreamWriter errorLog = new StreamWriter("log.txt");



            double[][] needOut = new double[8][];

            double[][] input = new double[8][];

            /*-------------TEST-DATA------------*/

            needOut[0] = new double[] { 1, 0 };
            needOut[1] = new double[] { 0, 1 };
            needOut[2] = new double[] { 1, 0 };
            needOut[3] = new double[] { 0, 1 };
            needOut[4] = new double[] { 1, 0 };
            needOut[5] = new double[] { 0, 1 };
            needOut[6] = new double[] { 1, 0 };
            needOut[7] = new double[] { 0, 1 };

            input[0] = new double[] { 10, 0 };
            input[1] = new double[] { 0, 10 };
            input[2] = new double[] { 20, 0 };
            input[3] = new double[] { 0, 20 };
            input[4] = new double[] { 30, 0 };
            input[5] = new double[] { 0, 30 };
            input[6] = new double[] { 40, 0 };
            input[7] = new double[] { 0, 40 };

            /*----------END-TEST-DATA-----------*/

            int[][] layers = new int[3][];

            layers[0] = new int[2];
            layers[1] = new int[5];
            layers[2] = new int[2];

            int epohs = 100000;

            NetConstructor maket = new NetConstructor(layers);

            Network net = new Network(maket.NetTypes, 0.2, 0.1);

            Console.Write("\nКоличество эпох: ");

            epohs = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < epohs; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    net.SetInputData(input[j]);
                    net.UpdateData();
                    net.Learn(needOut[j]);
                    Console.WriteLine($"Ошибка: {net.QuadError()}");
                }
            }
            Console.Write("Нажмите любую кнопку . . . ");
            Console.ReadKey();
        }
    }
}
