using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }
        
        public override void Run()
        {
            //throw new NotImplementedException();
            //
            List<Complex> compo;
            List<float> samples;
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            compo = new List<Complex>(); // sample in frequency domain --> compo[real and imagin num]
            samples = new List<float>(); // contain final output
            
            
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++) 
            {
                compo.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[i], InputFreqDomainSignal.FrequenciesPhaseShifts[i]));

            }

            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++) // i --> n
            {
                Complex complex = 0; // accumlate var

                for (int j = 0; j < InputFreqDomainSignal.FrequenciesAmplitudes.Count; j++) // j --> k
                {

                    float theta = (float)(2 * Math.PI * i * j) / (float)InputFreqDomainSignal.FrequenciesAmplitudes.Count;  
                    Complex imaginNum = Complex.ImaginaryOne *(float) Math.Sin((float)theta);
                    float realNum = (float)Math.Cos(theta);
                    complex += compo[j] * realNum + compo[j] * imaginNum;
                }
                complex /=(float) InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                samples.Add((float)complex.Real); // final answer in time domain 
            }
            OutputTimeDomainSignal.Samples = samples;
            //throw new NotImplementedException();
            /*List<float> harmonic = new List<float>();
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count();
            for (int k = 0; k < N; ++k)
            {
                double res=0;
                for (int n = 0; n < N; ++n)
                {
                    float real = (float)InputFreqDomainSignal.FrequenciesAmplitudes[n] * (float)Math.Cos((float)InputFreqDomainSignal.FrequenciesPhaseShifts[n]);
                    float img = (float)InputFreqDomainSignal.FrequenciesAmplitudes[n] * (float)Math.Sin((float)InputFreqDomainSignal.FrequenciesPhaseShifts[n]);
                    float powE = (float)(2 * (Math.PI) * (float)n * (float)k) / (float)N;
                    float x = ((float)Math.Cos((float)powE));
                    float y = ((float)Math.Sin((float)powE));
                    res += (x * real);
                    res += (x * img);//
                    res += (y * real);//
                    res += (y * img*-1);//-1
                }
                harmonic.Add((float)res / (float)N); 
                Console.WriteLine("res= " + res/N);
            }
            OutputTimeDomainSignal = new Signal(harmonic, false);*/
        
        }
    }
}
