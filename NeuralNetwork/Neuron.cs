using System;
using System.Collections.Generic;

namespace NeuralNetwork
{
    class Neuron
    {
        public List<Neuron> InputNeurons;

        public double[] Weigts
        {
            get;
            private set;
        }

        public bool IsInput
        {
            get;
            private set;
        }

        public double OutputData
        {
            get;
            private set;
        }

        public double Error
        {
            get;
            private set;
        }


        /// <summary>
        /// Конструктор нейрона
        /// </summary>
        /// <param name="Input"> Предшесвующий слой. Если нету то нужен null </param>
        /// <param name="stdWeight"> Стандартный вес у нейрона </param>
        public Neuron(List<Neuron> Input, double stdWeight = 1)
        {

            if (Input == null)
            {
                IsInput = true;
            }
            else
            {
                InputNeurons = Input;
                Weigts = new double[InputNeurons.Count];

                for (int i = 0; i < Weigts.Length; i++)
                {
                    SetWegth(stdWeight, i);
                }
            }
        }
        /// <summary>
        /// Функция активации
        /// </summary>
        /// <param name="input"> Входное значение </param>
        /// <returns> Нормализованное значение</returns>
        double FActivation(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }

        /// <summary>
        /// Обновление информации для промежуточных и выходных нейронов
        /// </summary>
        /// <returns></returns>
        public bool UpdateInfo()
        {
            if (!IsInput)
            {
                double[] neuron = new double[InputNeurons.Count];
                for (int i = 0; i < InputNeurons.Count; i++)
                {
                    neuron[i] = InputNeurons[i].OutputData * Weigts[i];
                }

                double data = 0;
                for (int i = 0; i < neuron.Length; i++)
                {
                    data += neuron[i];
                }

                OutputData = FActivation(data);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Обновление данных для входных нейронов
        /// </summary>
        /// <param name="input">Входные данные без функции активации</param>
        /// <returns></returns>
        public bool SetData(double input)
        {
            if(IsInput)
            {
                OutputData = FActivation(input);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Установка весов нейрона
        /// </summary>
        /// <param name="weight"> Устанавливаемый вес </param>
        /// <param name="ind"> Индекс веса в массиве </param>
        public void SetWegth(double weight, int ind)
        {
            Weigts[ind] = weight;
        }

        public void Correct(double learnSpeed)
        {
            if (IsInput)
                return;
            
            double deltaError = Error * (OutputData * (1 - OutputData));

            for (int i = 0; i < InputNeurons.Count; i++)
            {
                double correct = learnSpeed * deltaError * InputNeurons[i].OutputData ;
                Weigts[i] += correct;
            }
        }

        public void FindError(bool isOutputLayer, List<Neuron> prewLayer, int indInLayer, double needOut = 0)
        {
            Error = 0;
            if (isOutputLayer)
            {
                Error = needOut - OutputData;
                return;
            }
            else
            {
                for (int i = 0; i < prewLayer.Count; i++)
                {
                    Error += prewLayer[i].Weigts[indInLayer] * prewLayer[i].Error;
                }
            }
        }
    }
}
