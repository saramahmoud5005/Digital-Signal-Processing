using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();

            List<KeyValuePair<float, float>> harmonic = new List<KeyValuePair<float, float>>();
            List<float> signalA = new List<float>();
            List<float> signalTheta = new List<float>();
            List<float> signalFreq = new List<float>();
            int N = InputTimeDomainSignal.Samples.Count();

            for (int k = 0; k < N; ++k)
            {
                double resX = 0, resY = 0;
                for (int n = 0; n < N; ++n)
                {
                    double powE = (2 * (Math.PI) * n * k) / N; //power in e
                    double x = (Math.Cos(powE) * InputTimeDomainSignal.Samples[n]);
                    double y = (Math.Sin(powE) * InputTimeDomainSignal.Samples[n] * (-1));
                    resX += x; //real
                    resY += y; //imagin
                }
                signalA.Add((float)Math.Sqrt(Math.Pow(resX, 2) + Math.Pow(resY, 2))); //amp
                signalTheta.Add((float)Math.Atan2(resY, resX));                       //theta
            }
            for (int i = 0; i < N; ++i) signalFreq.Add((float)Math.Round((i * (2 * Math.PI) / ((N) * (1 / InputSamplingFrequency))),1));
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = signalA;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = signalTheta;
            OutputFreqDomainSignal.Frequencies = signalFreq;    //indicies of sampels

        }
    }
}

