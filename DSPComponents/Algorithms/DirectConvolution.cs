using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            //throw new NotImplementedException();
            // sum=x(k)*h(n-k)
            List<float> samplesDirctConv = new List<float>();
            List<int> samplesDirctConvIndexes = new List<int>();
            int startValue = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
            int endValue = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();
            for (int numIterations = startValue; numIterations <= endValue; ++numIterations)
            {
                float resultHarmonic = 0;
                for (int k = InputSignal1.SamplesIndices.Min(); k <= InputSignal1.SamplesIndices.Max(); ++k)
                {
                    //if (numIterations - k >= InputSignal2.Samples.Count) continue;
                    if (numIterations - k < InputSignal2.SamplesIndices.Min() || numIterations - k > InputSignal2.SamplesIndices.Max())
                        continue;
                    if (k < InputSignal1.SamplesIndices.Min() || k > InputSignal1.SamplesIndices.Max())
                        continue;
                    int x_indx = InputSignal1.SamplesIndices.IndexOf(k);
                    int h_indx = InputSignal2.SamplesIndices.IndexOf(numIterations - k);
                    resultHarmonic += InputSignal1.Samples[x_indx] * InputSignal2.Samples[h_indx];
                }
                //if (resultHarmonic == 0 && numIterations == endValue)
                  //  continue;
                samplesDirctConv.Add(resultHarmonic);
                samplesDirctConvIndexes.Add(numIterations);
            }
            List<float> samplesConv2 = new List<float>();
            List<int> samplesConvIndexes2 = new List<int>();
            int f = 0;
            for (int i = samplesDirctConv.Count - 1; i >= 0; --i)
            {
                if (samplesDirctConv[i] == 0) continue;
                else
                {
                    f = i;
                    break;
                }
            }
            for (int i = 0; i <= f; ++i)
            {
                samplesConv2.Add(samplesDirctConv[i]);
                samplesConvIndexes2.Add(samplesDirctConvIndexes[i]);
            }
            OutputConvolvedSignal = new Signal(samplesConv2, samplesConvIndexes2, false);
        }
    }
}