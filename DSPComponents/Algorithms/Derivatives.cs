using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> f1_derv = new List<float>();
            List<float> s2_derv = new List<float>();
            for (int e = 1; e < InputSignal.Samples.Count; e++)
            {
                f1_derv.Add(InputSignal.Samples[e] - InputSignal.Samples[e - 1]);
            }
            for (int o = 1; o < InputSignal.Samples.Count-1; o++)
            {
                s2_derv.Add(InputSignal.Samples[o + 1] - 2 * InputSignal.Samples[o] + InputSignal.Samples[o - 1]);
            }

            FirstDerivative = new Signal(f1_derv, false);
            SecondDerivative = new Signal(s2_derv, false);
            // throw new NotImplementedException();
        }
    }
}
