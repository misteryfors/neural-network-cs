using System;
using System.Collections.Generic;
using NeuralNetwork;
using System.Threading;
using System.IO;

namespace NeuralTest
{
    class Program
    {
        static void Main(string[] args)
        {

            StreamWriter errorLog = new StreamWriter("log.txt");

            

            double[][] needOut = new double[8][];

            double[][] input = new double[8][];

            double[] Error = new double[needOut.Length];

            /*-------------TEST-DATA------------*/

            needOut[0] = new double[] { 0 };
            needOut[1] = new double[] { 1 };
            needOut[2] = new double[] { 0 };
            needOut[3] = new double[] { 1 };
            needOut[4] = new double[] { 1 };
            needOut[5] = new double[] { 0 };
            needOut[6] = new double[] { 1 };
            needOut[7] = new double[] { 1 };

            input[0] = new double[] { 1, 0 };
            input[1] = new double[] { 6, 20 };
            input[2] = new double[] { 2, 0 };
            input[3] = new double[] { 4, 44};
            input[4] = new double[] { 6, 44 };
            input[5] = new double[] { 8, 1 };
            input[6] = new double[] { 10, 43 };
            input[7] = new double[] { 11, 24 };

            /*----------END-TEST-DATA-----------*/

            int[][] layers = new int[2][];

            layers[0] = new int[2];
            layers[1] = new int[1];

            bool Log = false;
            bool LogFile = false;
            bool stdSettings = false;
            int epohs = 100000;

            Network net = new Network(layers, 0.2, 0.1);

            Console.WriteLine("Обратите внимание! \nЕсли вы хотите сделать много эпох, \nто тогда, советуем не включать логирование в экран");

            Console.WriteLine();

            Console.WriteLine("Пользовательские настройки? нажмите клавишу (y - да; n - нет)");
            ConsoleKeyInfo settings = Console.ReadKey();

            if (settings.Key != ConsoleKey.Y)
            {
                Log = true;
                LogFile = true;
                stdSettings = true;
            }
                
            Console.WriteLine();

            if (!stdSettings)
            {
                Console.Write("\nКоличество эпох: ");

                epohs = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Логировать на экран? нажмите клавишу (y - да; n - нет)");
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.Y)
                    Log = true;

                Console.WriteLine();

                Console.WriteLine("Логировать в файл? нажмите клавишу (y - да; n - нет)");
                ConsoleKeyInfo file = Console.ReadKey();

                if (file.Key == ConsoleKey.Y)
                    LogFile = true;

                Console.WriteLine();
            }

            for (int i = 0; i < epohs; i++)
            {
                //Console.WriteLine($"\n\nЭпоха {i + 1}");
                for (int j = 0; j < input.Length; j++)
                {
                    //Console.WriteLine($"\n\tНабор {j + 1}");
                    net.SetInputData(input[j]);
                    net.UpdateData();

                    //var data = net.GetOutputData();

                    //for (int k = 0; k < data.Length; k++)
                    //{
                    //    Console.WriteLine(data[k]);
                    //}

                    net.Learn(needOut[j]);
                    
                    Error[j] = net.ErrorArif();
                }

                double arifErr = 0;
                for (int j = 0; j < needOut.Length; j++)
                {
                    arifErr += Error[j];
                }
                arifErr /= Error.Length;

                if(LogFile)
                    errorLog.WriteLine(arifErr);

                if (Log)
                    Console.WriteLine($"Ошибка в эпохе {i + 1} = {arifErr}");

                //Thread.Sleep(10);
            }

            Console.Write("Нажмите любую кнопку . . . ");
            Console.ReadKey();
        }
    }
}
