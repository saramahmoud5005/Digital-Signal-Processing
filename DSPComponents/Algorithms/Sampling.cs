﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            // throw new NotImplementedException();

            List<float> samples_signal = new List<float>();
            List<int> samplesIdex = new List<int>();
            FIR FIR_Signal_Obj = new FIR();
            FIR_Signal_Obj.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            FIR_Signal_Obj.InputFS = 8000;
            FIR_Signal_Obj.InputStopBandAttenuation = 50;
            FIR_Signal_Obj.InputCutOffFrequency = 1500;
            FIR_Signal_Obj.InputTransitionBand = 500;

            int first_Indx = InputSignal.SamplesIndices.Min();
            if (L == 0 && M != 0) //down sampling
            {

                FIR_Signal_Obj.InputTimeDomainSignal = new Signal(InputSignal.Samples, InputSignal.SamplesIndices, false);

                FIR_Signal_Obj.Run();

                for (int y = 0; y < FIR_Signal_Obj.OutputYn.Samples.Count; y += M)
                {
                    samples_signal.Add(FIR_Signal_Obj.OutputYn.Samples[y]);
                }

            }
            else if (M == 0 && L != 0) //up sampling
            {
                for (int i = 0; i < InputSignal.Samples.Count; ++i)
                {
                    samples_signal.Add(InputSignal.Samples[i]);
                    for (int j = 1; j < L; ++j) samples_signal.Add(0);
                }

                first_Indx = InputSignal.SamplesIndices.Min();

                for (int o = 0; o < samples_signal.Count; ++o)
                {
                    samplesIdex.Add(first_Indx);
                    first_Indx++;
                }

                FIR_Signal_Obj.InputTimeDomainSignal = new Signal(samples_signal, samplesIdex, false);

                FIR_Signal_Obj.Run();

                samples_signal.Clear();

                for (int i = 0; i < FIR_Signal_Obj.OutputYn.Samples.Count; ++i)
                {
                    samples_signal.Add((float)FIR_Signal_Obj.OutputYn.Samples[i]);
                }
            }
             
            else //up sampling & Low pass filter & down sampling
            {
                
                for (int i = 0; i < InputSignal.Samples.Count; ++i)
                {
                    samples_signal.Add(InputSignal.Samples[i]);
                    for (int j = 1; j < L; ++j) samples_signal.Add(0);
                }

                first_Indx = InputSignal.SamplesIndices.Min();

                for (int i = 0; i < samples_signal.Count; ++i)
                {
                    samplesIdex.Add(first_Indx);
                    first_Indx++;
                }

                FIR_Signal_Obj.InputTimeDomainSignal = new Signal(samples_signal, samplesIdex, false);

                FIR_Signal_Obj.Run();

                samples_signal.Clear();

                for (int i = 0; i < FIR_Signal_Obj.OutputYn.Samples.Count; i += M)
                {
                    samples_signal.Add((float)FIR_Signal_Obj.OutputYn.Samples[i]);
                }
            }

            samplesIdex.Clear();

            first_Indx = FIR_Signal_Obj.OutputYn.SamplesIndices.Min();

            for (int r = 0; r < samples_signal.Count; ++r)
            {
                samplesIdex.Add(first_Indx);
                first_Indx++;
            }

            OutputSignal = new Signal(samples_signal, samplesIdex, false);

        }
    }

}