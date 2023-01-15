using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            //int arrat_size=(InputSignal.Samples.Count-InputWindowSize)+1;
            List<float> samples_signal_avg = new List<float>();
            for (int y = 0; y <= InputSignal.Samples.Count - InputWindowSize; ++y)
            {
                float sum_for_avg = 0;
                for (int j = y; j < y + InputWindowSize; ++j)
                    sum_for_avg += InputSignal.Samples[j];
                sum_for_avg /= InputWindowSize;
                samples_signal_avg.Add(sum_for_avg);
                //Console.WriteLine("sum = " + sum);
            }
            OutputAverageSignal = new Signal(samples_signal_avg, false);
        }
    }
}