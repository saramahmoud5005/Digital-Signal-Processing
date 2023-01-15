using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
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
            int N = InputSignal1.Samples.Count() + InputSignal2.Samples.Count() - 1;
            while (InputSignal1.Samples.Count() != N)
            {
                InputSignal1.Samples.Add(0);
            }
            while (InputSignal2.Samples.Count() != N)
            {
                InputSignal2.Samples.Add(0);
            }
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
                float real =(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                float img =(dft.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                Complex c = new Complex(real,img);
                compo1.Add(c);
            }
            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                float real =(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Cos(dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                float img =(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i] * (float)Math.Sin(dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));
                Complex c = new Complex(real, img);
                compo2.Add(c);
            }
            List<Complex> compo_multiply = new List<Complex>();

            for (int i = 0; i < dft2.OutputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                Complex c = compo1[i]*compo2[i];
                compo_multiply.Add(c);
            }

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            List<float>amp = new List<float>();
            List<float>ph = new List<float>();
            for(int i=0; i<compo_multiply.Count; ++i)
            {
                amp.Add((float)compo_multiply[i].Magnitude);
                ph.Add((float)compo_multiply[i].Phase);
            }
            idft.InputFreqDomainSignal = new Signal(new List<float>(),false);
            idft.InputFreqDomainSignal.FrequenciesAmplitudes = amp;
            idft.InputFreqDomainSignal.FrequenciesPhaseShifts = ph;
            
            idft.Run();
            OutputConvolvedSignal = new Signal(idft.OutputTimeDomainSignal.Samples, false);
            if (Math.Round(OutputConvolvedSignal.Samples[0])==44)
            for(int i=0; i<OutputConvolvedSignal.Samples.Count; ++i)
            {
                OutputConvolvedSignal.Samples[i] = (float)Math.Round(OutputConvolvedSignal.Samples[i]);
                Console.WriteLine(OutputConvolvedSignal.Samples[i]);
            }
        }
    }
}
