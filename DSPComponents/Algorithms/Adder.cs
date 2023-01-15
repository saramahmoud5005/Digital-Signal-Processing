using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> samplee_signal_Add = new List<float>();
            
            for (int t = 0; t < InputSignals[0].Samples.Count; ++t)
            {
                //float s = InputSignals[0].Samples[j] + InputSignals[1].Samples[j];
                float s = 0;
                for (int r = 0; r < InputSignals.Count; ++r)
                {
                    s += InputSignals[r].Samples[t];
                }
                samplee_signal_Add.Add(s);
            }
            OutputSignal = new Signal(samplee_signal_Add, false);
        }
    }
}