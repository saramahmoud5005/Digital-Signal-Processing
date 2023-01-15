using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            int N = InputSignal1.Samples.Count;
            bool auto = false;
            if (InputSignal2 == null)
            {
                auto = true;
                InputSignal2 = new Signal(new List<float>(), false);
                for (int i = 0; i < InputSignal1.Samples.Count; ++i)
                {
                    InputSignal2.Samples.Add(InputSignal1.Samples[i]);
                }
            }
            if (InputSignal1.Samples.Count != InputSignal2.Samples.Count)
            {
                N = InputSignal1.Samples.Count() + InputSignal2.Samples.Count() - 1;
                while (InputSignal1.Samples.Count() != N)
                {
                    InputSignal1.Samples.Add(0);
                }
                while (InputSignal2.Samples.Count() != N)
                {
                    InputSignal2.Samples.Add(0);
                }
            }
            float s1 = 0, s2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                s1 += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                s2 += InputSignal2.Samples[i] * InputSignal2.Samples[i];
            }
            float normSum = (1 / (float)N * (float)Math.Sqrt(s1 * s2));
            
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            //dft.InputTimeDomainSignal=new Signal(in)
            dft.InputTimeDomainSignal = InputSignal1;
            dft.Run();
            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = InputSignal2;
            dft2.Run();
            List<Complex> compo1 = new List<Complex>();
            List<Complex> compo2 = new List<Complex>();
            for (int i = 0; i < dft.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                float real = (float)((float)dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos((float)dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                float img = (float)((float)dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin((float)dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                Complex c = new Complex(real, (-1) * img); ;

                compo1.Add(c);
            }
            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                float real = (float)(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos((float)dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                float img = (float)(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin((float)dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                Complex c = new Complex(real, img);

                compo2.Add(c);
            }
            List<Complex> compo_multiply = new List<Complex>();

            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex c = compo1[i] * compo2[i];
                compo_multiply.Add(c);


            }

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            List<float> amp = new List<float>();
            List<float> ph = new List<float>();
            for (int i = 0; i < compo_multiply.Count; ++i)
            {
                amp.Add((float)compo_multiply[i].Magnitude);
                ph.Add((float)compo_multiply[i].Phase);
            }
            idft.InputFreqDomainSignal = new Signal(new List<float>(), false);
            idft.InputFreqDomainSignal.FrequenciesAmplitudes = amp;
            idft.InputFreqDomainSignal.FrequenciesPhaseShifts = ph;
            idft.Run();
            OutputNonNormalizedCorrelation = new List<float>();
            //OutputNonNormalizedCorrelation = idft.OutputTimeDomainSignal.Samples;
            Signal resSignal = idft.OutputTimeDomainSignal;
            OutputNormalizedCorrelation = new List<float>();

            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; ++i)
            {
                //    //OutputNormalizedCorrelation.Add((float)((float)idft.OutputTimeDomainSignal.Samples[i] / (double)N));
                //    OutputNormalizedCorrelation.Add(idft.OutputTimeDomainSignal.Samples[i] / normSum);

                //    Console.WriteLine(OutputNonNormalizedCorrelation[i]);
                //    Console.WriteLine(OutputNormalizedCorrelation[i]);
                resSignal.Samples[i] /= N;
                OutputNonNormalizedCorrelation.Add(resSignal.Samples[i]);
                OutputNormalizedCorrelation.Add(resSignal.Samples[i] / normSum);
            }






        }
    }
}