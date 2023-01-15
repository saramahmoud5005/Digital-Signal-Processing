using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> samples_res = new List<float>();

            for (int y = 0; y < InputSignal.Samples.Count; ++y)
            {
                float sample_res = InputSignal.Samples[y] *InputConstant;
                samples_res.Add(sample_res);
            }
            OutputMultipliedSignal = new Signal(samples_res, false);
        }
    }
}
