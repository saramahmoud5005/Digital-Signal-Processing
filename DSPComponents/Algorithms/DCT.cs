using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> samples = new List<float>();
            OutputSignal = new Signal(new List<float>(), new List<int>(), InputSignal.Periodic);
            int N=InputSignal.Samples.Count;
            for(int r=0; r<InputSignal.Samples.Count; ++r)
            {
                float res = 0;
                for(int j=0; j<InputSignal.Samples.Count; ++j)
                {
                    res+=(float)(InputSignal.Samples[j]*Math.Cos((Math.PI/(4*N))*(2*j-1)*(2*r-1)));
                }
                res=res*(float)(Math.Sqrt((double)2/N));
                OutputSignal.Samples.Add(res);
                OutputSignal.SamplesIndices.Add(r);
            }
        }
    }
}
