using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerualNetwork_2._0
{
    public class NetConstructor
    {
        public NeuronTypes[][] NetTypes
        {
            get;
            private set;
        }

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
