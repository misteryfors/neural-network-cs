using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetowork.Rewrite
{
    public class NetworkConstructor
    {
        public NeruonType[][] Maket
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор макета сети по массиву, автоматически включает в себя нейрон смещения
        /// </summary>
        /// <param name="netMaket">Макет сети, без нейронов смещения (сеть добавляет их сама) </param>
        public NetworkConstructor(int[] netMaket)
        {
            CreateMaket(netMaket);

            InitializeMaket();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="learnSpeed">скорость обучения</param>
        /// <returns>Новая сеть созданная по параметрам</returns>
        public NerualNetwork GetNet(double learnSpeed)
        {
            return new NerualNetwork(Maket, learnSpeed);
        }

        private void CreateMaket(int[] netMaket)
        {
            Maket = new NeruonType[netMaket.Length][];
            
            for (int i = 0; i < netMaket.Length; i++)
            {
                if (i != netMaket.Length - 1)
                    Maket[i] = new NeruonType[netMaket[i] + 1];
                else
                    Maket[i] = new NeruonType[netMaket[i]];
            }
        }

        private void InitializeMaket()
        {

            // полная иницаиализация сети
            for (int i = 0; i < Maket.Length; i++)
            {
                // первый слой
                if (i == 0)
                {
                    // инициализация нейронов
                    for (int j = 0; j < Maket[i].Length; j++)
                    {
                        // если последний то это нейрон смещения
                        // иначе это вход
                        if (j == Maket[i].Length - 1)
                        {
                            Maket[i][j] = NeruonType.BIAS;
                        }
                        else
                        {
                            Maket[i][j] = NeruonType.INPUT;
                        }
                    }
                    continue;
                }

                // последний слой
                if (i == Maket.Length - 1)
                {
                    // все нейроны тут выходные
                    for (int j = 0; j < Maket[i].Length; j++)
                    {
                        Maket[i][j] = NeruonType.OUTPUT;
                    }
                    continue;
                }

                // все остальные слои
                for (int j = 0; j < Maket[i].Length; j++)
                {
                    // если последний то это нейрон смещения
                    // иначе это внутренний нейрон
                    if (j == Maket[i].Length - 1)
                    {
                        Maket[i][j] = NeruonType.BIAS;
                        continue;
                    }
                    else
                    {
                        Maket[i][j] = NeruonType.HIDDEN;
                    }
                }
            }
        }
    }
}
