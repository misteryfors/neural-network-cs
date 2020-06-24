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

            // объявления макета сети

            int[][] netMaket = new int[3][];

            netMaket[0] = new int[2];
            netMaket[1] = new int[2];
            netMaket[2] = new int[2];

            // объявления потока для записи ошибок в эпохе
            StreamWriter errorLog = new StreamWriter("log1.txt");


            // Должно быть на выходе
            double[][] needOut = new double[8][];

            // Данные на вход 
            double[][] input = new double[8][];

            #region dataInit

            needOut[0] = new double[] { 0, 1 };
            needOut[1] = new double[] { 1, 0 };
            needOut[2] = new double[] { 0, 1 };
            needOut[3] = new double[] { 1, 0 };
            needOut[4] = new double[] { 1, 0 };
            needOut[5] = new double[] { 0, 1 };
            needOut[6] = new double[] { 1, 0 };
            needOut[7] = new double[] { 1, 0 };

            input[0] = new double[] { 1, 0 };
            input[1] = new double[] { 6, 20 };
            input[2] = new double[] { 2, 0 };
            input[3] = new double[] { 4, 44 };
            input[4] = new double[] { 6, 44 };
            input[5] = new double[] { 8, 1 };
            input[6] = new double[] { 10, 43 };
            input[7] = new double[] { 11, 24 };

            #endregion

            int epohs;

            // Создание конструктора сети
            NetConstructor maket = new NetConstructor(netMaket);

            // Объявление сети
            Network net = new Network(maket.NetTypes, 0.2);

            net.LoadData();

            // Ввод кол-ва эпох
            Console.Write("\nКоличество эпох (Больше 2000 нет смысла): ");

            epohs = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < epohs; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    net.SetInputData(input[j]);
                    net.UpdateData();

                    net.Learn(needOut[j]);
                    
                }

                errorLog.WriteLine(net.QuadError());
            }

            net.SaveData();

            errorLog.Close();
            Console.Write("Нажмите любую кнопку . . . ");
            Console.ReadKey();
        }
    }
}
