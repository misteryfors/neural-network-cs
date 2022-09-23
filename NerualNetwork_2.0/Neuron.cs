﻿using System;
using System.Collections.Generic;
//Copyright (c) 2020 BonMAS14
namespace NerualNetwork_2_0
{
    //
    public enum NeuronTypes
    {
        INPUT,
        HIDDEN,
        BIAS,
        OUTPUT
    }

    class Neuron
    {
        public List<Neuron> prewLayer;

        //Веса связей
        public double[] Weigths
        {
            get;
            private set;
        }
        //Тип нейрона
        public NeuronTypes Type
        {
            get;
            private set;
        }
        // выходные данные
        public double OutputData
        {
            get;
            private set;
        }
        //ошибка
        public double Error
        {
            get;
            private set;
        }

        int indexInLayer;
        //инициализация нейрона
        public Neuron(NeuronTypes type, NeuronTypes[] prewLayerTypes, List<Neuron> _prewLayer, int _indexInLayer)
        {
            indexInLayer = _indexInLayer;

            Type = type;

            prewLayer = _prewLayer;
            
            if (type == NeuronTypes.INPUT || type == NeuronTypes.BIAS)
            {
                if (type == NeuronTypes.BIAS)
                {
                    OutputData = 1;
                }
                return;
            }

            Weigths = new double[prewLayerTypes.Length];

            Random rand = new Random();

            for (int i = 0; i < Weigths.Length; i++)
            {
                Weigths[i] = rand.NextDouble() - 0.5;
            }
        }

        //функция активации 1/(1+e^(-x)
        //Выдаёт результат в пределах от 0 до 1
        double ActivationFunc(double data)
        {
            return 1 / (1 + Math.Pow(Math.E, -data));
        }

        //функция коректировки значения 
        double DeltaFunc(double y)
        {
            return y * (1 - y);
        }

        //функция передачи заряда между нейронами
        public bool DataUpdate()
        {
            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
                return false;

            double[] neuron = new double[prewLayer.Count];
            for (int i = 0; i < prewLayer.Count; i++)
            {
                neuron[i] = prewLayer[i].OutputData * Weigths[i];
            }

            double data = 0;
            for (int i = 0; i < neuron.Length; i++)
            {
                data += neuron[i];
            }

            OutputData = ActivationFunc(data);
            return true;
        }
        //функция прохождения заряда через нейрон 
        //Значения могут принимать только входные нейроны
        public bool DataUpdate(double data)
        {
            if (Type != NeuronTypes.INPUT)
                return false;

            OutputData = ActivationFunc(data);
            return true;
        }
        //Функция корректировки вессов в соответствии с полученной ошибкой
        public void CorrectWeigts(double learnSpeed)
        {
            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
                return;

            double deltaError = Error * DeltaFunc(OutputData);

            for (int i = 0; i < prewLayer.Count; i++)
            {
                double correct = learnSpeed * deltaError * prewLayer[i].OutputData;
                Weigths[i] += correct;
            }
        }

        //Функия получения значения ошибки
        public void FindError(List<Neuron> NextLayer, double needOut = 0)
        {
            Error = 0;

            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
                return;

            if (Type == NeuronTypes.OUTPUT)
            {
                Error = needOut - OutputData;
                return;
            }
            else
            {
                for (int i = 0; i < NextLayer.Count; i++)
                {
                    if (NextLayer[i].Type == NeuronTypes.BIAS)
                        continue;
                    Error += NextLayer[i].Weigths[indexInLayer] * NextLayer[i].Error;
                }
            }
        }
        //Функция записи весов
        public bool WriteWeight(int index, double value)
        {
            if (Type == NeuronTypes.INPUT || Type == NeuronTypes.BIAS)
                return false;

            Weigths[index] = value;

            return true;
        }
    }
}
