using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork_2_0
{
    public class Network
    {
        List<Neuron>[] layers;
        double learnSpeed;

        /// <summary>
        /// Конструктор сети
        /// </summary>
        /// <param name="Maket"> Макет сети из класса NetConstructor</param>
        /// <param name="_learnSpeed"> Коэфицент/скорость обучения </param>
        /// <param name="stdWeight"> Стандартное значение всех весов </param>
        public Network(NeuronTypes[][] Maket, double _learnSpeed = 0.1)
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
                        layers[i].Add(new Neuron(Maket[i][j], null, null, j));
                    }
                    else
                    {
                        layers[i].Add(new Neuron(Maket[i][j], Maket[i - 1], layers[i - 1], j));
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns> Возвращает массив данных с нейрона </returns>
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

        /// <summary>
        /// Установка значений на входы
        /// </summary>
        /// <param name="inputData"> Массив входных значений для входных нейронов </param>
        public void SetInputData(double[] inputData)
        {
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count; i++)
            {
                if (layers[inputLayer][i].Type == NeuronTypes.BIAS) continue;
                
                layers[inputLayer][i].DataUpdate(inputData[i]);
            }
        }
        /// <summary>
        /// Обновление информации всех нейронов
        /// </summary>
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
        /// <summary>
        /// Функция обучения сети
        /// </summary>
        /// <param name="needOut"> Массив правильных значений для выходного слоя </param>
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
        /// <summary>
        /// Вычисление ошибки
        /// </summary>
        /// <returns>значение ошибки</returns>
        public double QuadError()
        {
            double err = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                    continue;

                for (int j = 0; j < layers[i].Count; j++)
                {
                    err += Math.Pow(layers[i][j].Error, 2);
                }
            }
            return err;
        }


        public void SaveData()
        {

            StreamWriter writer = new StreamWriter("LastSave.txt", false);
            
            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                    continue;

                for (int j = 0; j < layers[i].Count; j++)
                {
                    for (int k = 0; k < layers[i][j].Weigths.Length; k++)
                    {
                        writer.Write(layers[i][j].Weigths[k]);
                        writer.Write(" ; ");
                    }
                }
                writer.WriteLine();
            }

            writer.Close();
        }
        public void LoadData()
        {

            StreamReader reader = new StreamReader("LastSave.txt");

            string line;

            for (int i = 0; i < layers.Length; i++)
            {
                if (i == 0)
                    continue;

                line = reader.ReadLine();
                for (int j = 0; j < layers[i].Count; j++)
                {
                    int index = line.IndexOf(";");
                    string value = line.Remove(index);
                    line = line.Substring(index + 1);

                    for (int k = 0; k < layers[i][j].Weigths.Length; k++)
                    {
                        layers[i][j].WriteWeight(k, Convert.ToDouble(value));
                    }
                }
            }

            reader.Close();
        }
    }
}
