﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Copyright (c) 2020 BonMAS14
namespace NerualNetwork_2_0
{
    public class NetConstructor
    {
        /// <summary>
        /// Готовая сеть
        /// </summary>
        public NeuronTypes[][] NetTypes
        {
            get;
            private set;
        }
        /// <summary>
        /// Конструктор макета сети по ступенчатому массиву, автоматически включает в себя нейрон смещения
        /// </summary>
        /// <param name="netMaket">Макет сети, без нейронов смещения (сам добавляет) </param>
        public NetConstructor(int[][] netMaket)
        {
            NetTypes = new NeuronTypes[netMaket.Length][];

            for (int i = 0; i < netMaket.Length; i++)
            {
                if (i != netMaket.Length - 1)
                    NetTypes[i] = new NeuronTypes[netMaket[i].Length + 1];
                else
                    NetTypes[i] = new NeuronTypes[netMaket[i].Length];
            }
            for (int i = 0; i < NetTypes.Length; i++)
            {
                if (i == 0)
                {
                    for (int j = 0; j < NetTypes[i].Length; j++)
                    {
                        if (j == NetTypes[i].Length - 1)
                        {
                            NetTypes[i][j] = NeuronTypes.BIAS;
                            continue;
                        }
                        else
                        {
                            NetTypes[i][j] = NeuronTypes.INPUT;
                        }
                    }
                    continue;
                }
                if (i == NetTypes.Length - 1)
                {
                    for (int j = 0; j < NetTypes[i].Length; j++)
                    {

                        NetTypes[i][j] = NeuronTypes.OUTPUT;
                    
                    }
                    continue;
                }

                for (int j = 0; j < NetTypes[i].Length; j++)
                {
                    if (j == NetTypes[i].Length - 1)
                    {
                        NetTypes[i][j] = NeuronTypes.BIAS;
                        continue;
                    }
                    else
                    {
                        NetTypes[i][j] = NeuronTypes.HIDDEN;
                    }
                }
            }
        }
    }
}
