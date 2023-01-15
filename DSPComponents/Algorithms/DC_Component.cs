using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            float calc_mean = 0;
            for (int y = 0; y < InputSignal.Samples.Count; ++y)
            {
                calc_mean += InputSignal.Samples[y];
            }
            calc_mean /= InputSignal.Samples.Count;

            List<float> res_of_mean_signal = new List<float>();

            for (int r = 0; r < InputSignal.Samples.Count; ++r)
            {
                res_of_mean_signal.Add(InputSignal.Samples[r] - calc_mean);
            }
            OutputSignal = new Signal(res_of_mean_signal, false);
        }
    }
}
