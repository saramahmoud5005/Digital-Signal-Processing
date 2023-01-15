using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> shiftingList_signal = new List<float>();
            List<int> shiftingListIndexes_signal = new List<int>();
            
            if (InputSignal.Periodic == true)
                for (int i = 0; i < InputSignal.Samples.Count; ++i)
                {
                    shiftingList_signal.Add(InputSignal.Samples[i]);
                    shiftingListIndexes_signal.Add(InputSignal.SamplesIndices[i] + ShiftingValue);

                }
            else
            {
                for (int i = 0; i < InputSignal.Samples.Count; ++i)
                {
                    shiftingList_signal.Add(InputSignal.Samples[i]);
                    shiftingListIndexes_signal.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                }
            }
            OutputShiftedSignal = new Signal(new List<float>(), new List<int>(), true);
            OutputShiftedSignal.Samples = shiftingList_signal;
            OutputShiftedSignal.SamplesIndices = shiftingListIndexes_signal;
        }
    }
}