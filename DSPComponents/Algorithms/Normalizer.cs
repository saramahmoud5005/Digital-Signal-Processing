using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> samplee_normm = new List<float>();
            float max_num_signal_sample = InputSignal.Samples[0], min_num_signal_sample = InputSignal.Samples[0];
            for(int y=0; y<InputSignal.Samples.Count; ++y)
            {
                if (max_num_signal_sample < InputSignal.Samples[y]) max_num_signal_sample = InputSignal.Samples[y];
                if (min_num_signal_sample > InputSignal.Samples[y]) min_num_signal_sample = InputSignal.Samples[y];
            }
            for (int t = 0; t < InputSignal.Samples.Count; ++t)
            {
                float s = ((InputSignal.Samples[t] - min_num_signal_sample) / (max_num_signal_sample - min_num_signal_sample)) * (InputMaxRange - InputMinRange) + InputMinRange;
                samplee_normm.Add(s);
            }
            OutputNormalizedSignal = new Signal(samplee_normm, false);
        }
    }
}
