using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> cum_samples_Signal = new List<float>();
            cum_samples_Signal.Add(InputSignal.Samples[0]);
            for (int r = 1; r < InputSignal.Samples.Count; ++r)
            {
                cum_samples_Signal.Add(cum_samples_Signal[r - 1] + InputSignal.Samples[r]);
            }
            OutputSignal = new Signal(cum_samples_Signal, false);
        }
    }
}