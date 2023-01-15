using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            bool auto_corr = false;
            if(InputSignal2==null)
            {
                auto_corr = true;
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);

            }
            float norm = 0,corr=0;
            int N=0;
            float res1 = 0, res2 = 0;
            N = InputSignal1.Samples.Count; ;
            for (int i = 0; i < InputSignal1.Samples.Count; ++i)
            {
                res1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
            }
            for (int i = 0; i < InputSignal2.Samples.Count; ++i)
            {
                res2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }
            norm = res1 * res2;
            norm = (float)Math.Sqrt(norm);
            norm = norm / N;
            
            for (int j = 0; j < InputSignal1.Samples.Count; ++j)
            {
                corr = 0;
                for (int i = 0; i < InputSignal2.Samples.Count; ++i)
                {
                    corr += (InputSignal1.Samples[i] * InputSignal2.Samples[i]);
                }
                corr = corr / N;
                OutputNonNormalizedCorrelation.Add(corr);
                OutputNormalizedCorrelation.Add(corr / norm);
                // shift left
                float Index0 = InputSignal2.Samples[0];
                for (int i = 0; i < InputSignal2.Samples.Count - 1; ++i)
                {
                    InputSignal2.Samples[i] = InputSignal2.Samples[i + 1];
                }
                if (InputSignal2.Periodic)
                    InputSignal2.Samples[InputSignal2.Samples.Count - 1] = Index0;
                else
                    InputSignal2.Samples[InputSignal2.Samples.Count - 1] = 0;
            }
        }
    }
}