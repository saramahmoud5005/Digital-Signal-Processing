using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<string> window_name = new List<string>();
            List<float> window_StopBandAtten = new List<float>(); //stopBandAttenuation
            List<float> window_res = new List<float>();
            List<float> filter_res = new List<float>();
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            OutputYn = new Signal(new List<float>(), new List<int>(), false);

            window_name.Add("Rectangular");
            window_StopBandAtten.Add(21);
            window_name.Add("Hanning");
            window_StopBandAtten.Add(44);
            window_name.Add("Hamming");
            window_StopBandAtten.Add(53);
            window_name.Add("Blackman");
            window_StopBandAtten.Add(74);

            string windowName="", filterName="";
            for(int i=0; i<4; ++i)
            {
                if (InputStopBandAttenuation <= window_StopBandAtten[i])
                {
                    windowName = window_name[i];
                    break;
                }
                
            }

            float deltaF=0;
            int startBoundry=0, endBoundry=0;
            
            if (windowName == "Rectangular")
            {

                deltaF = (float)(0.9 / (InputTransitionBand / InputFS));

                int N = (int)Math.Ceiling(deltaF);
                if (N % 2 == 0) N += 1;

                startBoundry = (-1)*(N/2);
                endBoundry = N/2;
                for(int n=startBoundry; n<=endBoundry; ++n)
                {
                    window_res.Add((float)(1));
                }
                
            }
            else if (windowName == "Hanning")
            {

                deltaF = (float)(3.1 / (InputTransitionBand / InputFS));

                int N = (int)Math.Ceiling(deltaF);
                if (N % 2 == 0) N += 1;

                startBoundry = (-1) * (N / 2);
                endBoundry = N / 2;
                for(int n=startBoundry ; n<=endBoundry; ++n)
                {
                    window_res.Add((float)(0.5 + 0.5 * Math.Cos((2 * Math.PI * Math.Abs(n)) / N)));
                }
            }
            else if (windowName == "Hamming")
            {

                deltaF = (float)(3.3 / (InputTransitionBand / InputFS));

                int N = (int)Math.Ceiling(deltaF);
                if (N % 2 == 0) N += 1;

                startBoundry = (-1) * (N / 2);
                endBoundry = N / 2;
                for (int n = startBoundry; n <= endBoundry; ++n)
                {
                    window_res.Add((float)(0.54 + 0.46 * Math.Cos((2 * Math.PI * Math.Abs(n)) / N)));
                }
               
            }
            else if (windowName == "Blackman")
            {

                deltaF = (float)(5.5 / (InputTransitionBand / InputFS));

                int N = (int)Math.Ceiling(deltaF);
                if (N % 2 == 0) N += 1;

                startBoundry = (-1) * (N / 2);
                endBoundry = N / 2;
                for (int n = startBoundry; n <= endBoundry ; ++n)
                {
                    window_res.Add((float)(0.42 + 0.5 * Math.Cos((2 * Math.PI * Math.Abs(n)) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * Math.Abs(n)) / (N - 1))));
                }
            }


            float resultCOFFilter=0;

            if(FILTER_TYPES.LOW==InputFilterType)
            {
                resultCOFFilter = ((float)(InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS);
                for (int n = startBoundry; n <= endBoundry; ++n)
                {
                    if (n != 0)
                    {
                        filter_res.Add((float)(2 * resultCOFFilter * (Math.Sin(Math.Abs(n) * 2 * Math.PI * resultCOFFilter)) / (Math.Abs(n) * 2 * Math.PI * resultCOFFilter)));
                    }
                    else
                    {
                        filter_res.Add(2 * resultCOFFilter);
                    }
                }
                
            }
            else if (FILTER_TYPES.HIGH == InputFilterType)
            {
                resultCOFFilter = ((float)(InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS);
                for (int n = startBoundry; n <= endBoundry ; ++n)
                {
                    if (n != 0)
                    {
                        filter_res.Add((float)(-1 * 2 * resultCOFFilter * (Math.Sin(Math.Abs(n) * 2 * Math.PI * resultCOFFilter)) / (Math.Abs(n) * 2 * Math.PI * resultCOFFilter)));
                    }
                    else
                    {
                        filter_res.Add(1 - (2 * resultCOFFilter));
                    }
                }

            }
            else if (FILTER_TYPES.BAND_PASS == InputFilterType)
            {
                float resultCOFFilter1 = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                float resultCOFFilter2= (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                for (int n = startBoundry; n <= endBoundry; ++n)
                {
                    if (n != 0)
                    {
                        float res1 = (float)(2 * resultCOFFilter1 * ((Math.Sin(Math.Abs(n) * 2 * Math.PI * resultCOFFilter1)) / (Math.Abs(n) * 2 * Math.PI * resultCOFFilter1)));
                        float res2 = (float)(2 * resultCOFFilter2 * ((Math.Sin(Math.Abs(n) * 2 * Math.PI * resultCOFFilter2)) / (Math.Abs(n) * 2 * Math.PI * resultCOFFilter2)));
                        filter_res.Add(res2 - res1);
                    }
                    else
                    {
                        filter_res.Add(2 * (resultCOFFilter2 - resultCOFFilter1));
                    }
                }

            }
            else if (FILTER_TYPES.BAND_STOP == InputFilterType)
            {
                float resultCOFFilter1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                float resultCOFFilter2 = (float)((InputF2 - (InputTransitionBand / 2)) / InputFS);
                for (int n = startBoundry; n <= endBoundry; ++n)
                {
                    if (n != 0)
                    {
                        float res1 = (float)(2 * resultCOFFilter1 * ((Math.Sin(Math.Abs(n) * 2 * Math.PI * resultCOFFilter1)) / (Math.Abs(n) * 2 * Math.PI * resultCOFFilter1)));
                        float res2 = (float)(2 * resultCOFFilter2 * ((Math.Sin(Math.Abs(n) * 2 * Math.PI * resultCOFFilter2)) / (Math.Abs(n) * 2 * Math.PI * resultCOFFilter2)));
                        filter_res.Add(res1 - res2);
                    }
                    else
                    {
                        filter_res.Add(1 - (2 * (resultCOFFilter2 - resultCOFFilter1)));
                    }
                }

            }
            
            int indx_arr = 0;
            for (int i = startBoundry; i <= endBoundry; ++i)
            {
                OutputHn.SamplesIndices.Add(i);
                float hn_res = (window_res[indx_arr] * filter_res[indx_arr]);
                OutputHn.Samples.Add(hn_res);
                indx_arr++;
            }
                
            DirectConvolution directConv_Obj = new DirectConvolution();
            directConv_Obj.InputSignal1 = InputTimeDomainSignal;
            directConv_Obj.InputSignal2 = OutputHn;
            directConv_Obj.Run();

            OutputYn = new Signal(directConv_Obj.OutputConvolvedSignal.Samples, directConv_Obj.OutputConvolvedSignal.SamplesIndices, false);

        }
    }
}
