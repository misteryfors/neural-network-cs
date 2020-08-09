using System;
using System.Collections.Generic;
using System.IO;

namespace NerualNetowork.Rewrite
{
    public class NerualNetwork
    {
        List<Neuron>[] layers;
        double learnSpeed;
        
        /// <summary>
        /// Конструктор сети по макету.
        /// </summary>
        /// <param name="Maket"> макет полученный из NetworkConstructor</param>
        /// <param name="learnSpeed"> Скорость обучения </param>
        public NerualNetwork(NeruonType[][] Maket, double learnSpeed = 0.1)
        {
            // установка скорости обучения
            this.learnSpeed = learnSpeed;

            // создания сети
            layers = new List<Neuron>[Maket.Length];

            // инициализация сети
            for (int i = 0; i < Maket.Length; i++)
            {
                // создание слоя
                layers[i] = new List<Neuron>(Maket[i].Length);

                // инициализация слоя
                for (int j = 0; j < Maket[i].Length; j++)
                {
                    if (i == 0)
                        layers[i].Add(new Neuron(Maket[i][j], j, null, null));
                    else 
                        layers[i].Add(new Neuron(Maket[i][j], j, Maket[i - 1], layers[i - 1]));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>данные с сети</returns>
        public double[] GetData()
        {
            // берём последний слой
            int lastLayer = layers.Length - 1;

            // создаём массив размером с этот слой
            double[] output = new double[layers[lastLayer].Count];

            // читаем выходные значения каждого нейрона
            for (int i = 0; i < layers[lastLayer].Count; i++)
            {
                output[i] = layers[lastLayer][i].OutputData;
            }
            // возвращаем
            return output;
        }

        /// <summary>
        /// Установка данных в сеть
        /// </summary>
        /// <param name="inputData">массив данных размером с входной слой</param>
        public void SetData(double[] inputData)
        {
            // берём первый слой
            int inputLayer = 0;

            for (int i = 0; i < layers[inputLayer].Count; i++)
            {
                // пропускаем нейрон смещения
                if (layers[inputLayer][i].Type == NeruonType.BIAS) continue;

                // записываем данные в каждый нейрон
                layers[inputLayer][i].DataUpdate(inputData[i]);
            }
        }
        
        /// <summary>
        /// Обновленние данных в сети
        /// </summary>
        public void UpdateData()
        {
            // проходим по каждому слою
            for (int i = 0; i < layers.Length; i++)
            {
                // пропускаем первый слой
                if (i == 0) continue;

                // обновляем каждый нейрон
                for (int j = 0; j < layers[i].Count; j++)
                {
                    layers[i][j].DataUpdate();
                }
            }
        }
        
        /// <summary>
        /// Обучение сети
        /// </summary>
        /// <param name="LearnSet">Массив требуемых выходных значений размером с выходной слой</param>
        public void Learn(double[] LearnSet)
        {
            // поиск ошибки методом обратного распространения ошибки
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    for (int j = 0; j < layers[i].Count; j++)
                    {
                        layers[i][j].FindError(null, LearnSet[j]);
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

            // корректировка весов согласно ошибкам
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
        /// Квадратичная ошибка последнего прохода
        /// </summary>
        /// <returns>Квадратичную ошибку</returns>
        public double GetError()
        {
            double quadError = 0;

            for (int i = 0; i < layers.Length; i++)
            {
                // пропускаем первый слой
                if (i == 0)
                    continue;

                // собираем с каждого нейрона квадратичную ошибку
                for (int j = 0; j < layers[i].Count; j++)
                {
                    quadError += Math.Pow(layers[i][j].Error, 2);
                }
            }
            return quadError;
        }
        
        /// <summary>
        /// Сохранение весов в такстовый документ
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public void SaveData(string path = "LastSave.txt")
        {
            // Объявление потока записи
            StreamWriter writer = new StreamWriter(path, false);

            for (int i = 0; i < layers.Length; i++)
            {
                // пропускаем первый слой, так как у него нет весов
                if (i == 0)
                    continue;

                for (int j = 0; j < layers[i].Count; j++)
                {
                    // если это нейрон смещения то пропускаем
                    if (layers[i][j].Type == NeruonType.BIAS) continue;

                    for (int k = 0; k < layers[i][j].Weigths.Length; k++)
                    {
                        writer.Write(layers[i][j].Weigths[k]);

                        writer.Write(" ; ");
                    }
                }
                writer.WriteLine();
            }

            // закрытие потока
            writer.Close();
        }

        /// <summary>
        /// Загрузка значений весов
        /// </summary>
        /// <param name="path">Путь к файлу с которого идёт загрузка</param>
        public void LoadData(string path = "LastSave.txt")
        {
            // Объявление потока чтения
            StreamReader reader = new StreamReader(path);

            // последняя строка (слой)
            string line;

            for (int i = 0; i < layers.Length; i++)
            {
                // пропускаем первый слой, так как у него нет весов
                if (i == 0)
                    continue;

                // читаем слой
                line = reader.ReadLine();

                // если ничего там нет то выходим
                if (line == null)
                    break;

                for (int j = 0; j < layers[i].Count; j++)
                {
                    // если это нейрон смещения то пропускаем
                    if (layers[i][j].Type == NeruonType.BIAS) continue;

                    // здесь идёт разбиение слоя на веса
                    for (int k = 0; k < layers[i][j].Weigths.Length; k++)
                    {
                        int index = line.IndexOf(";");
                        string value = line.Remove(index);
                        line = line.Substring(index + 1);

                        // запись распарсеного веса
                        layers[i][j].WriteWeight(k, Convert.ToDouble(value));
                    }
                }
            }
            // закрытие потока
            reader.Close();
        }
    }
}
