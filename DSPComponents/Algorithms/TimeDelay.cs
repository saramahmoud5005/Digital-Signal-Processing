using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            DirectCorrelation dc = new DirectCorrelation();
            dc.InputSignal1 = new Signal(InputSignal1.Samples, InputSignal1.Periodic);
            dc.InputSignal2 = new Signal(InputSignal2.Samples, InputSignal2.Periodic);
            dc.Run();
            int indx_value = 0;
            float max_value = dc.OutputNormalizedCorrelation[0];
            for(int y=0; y<dc.OutputNormalizedCorrelation.Count; ++y)
            {
                if (max_value < dc.OutputNormalizedCorrelation[y])
                {
                    max_value = dc.OutputNormalizedCorrelation[y];
                    indx_value = y;
                }
            }
            OutputTimeDelay = InputSamplingPeriod * indx_value;
        }
    }
}
