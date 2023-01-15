using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            if (InputLevel <= 0) InputLevel = (int)Math.Pow(2, (double)InputNumBits);
            else if (InputNumBits <= 0) InputNumBits = (int)Math.Log(InputLevel, 2);
            float max = InputSignal.Samples[0], min = InputSignal.Samples[0];
            for (int i = 0; i < InputSignal.Samples.Count; ++i)
            {
                if (max < InputSignal.Samples[i]) max = InputSignal.Samples[i];
                if (min > InputSignal.Samples[i]) min = InputSignal.Samples[i];
            }
            float res = (max - min) / (float)InputLevel;
            float x = min;
            //limits --> ranges of levels
            List<float> limits = new List<float>();
            List<float> midPoint = new List<float>();
            for (int i = 0; i < InputLevel; ++i)
            {
                if (i == 0) limits.Add(x);
                float a = x;
                x += (float)res;
                limits.Add(x);
                float b = x;
                midPoint.Add(((float)(a + b) / (float)2));
                // for ensuring that the code is good
                if (i == 0) Console.WriteLine("a = " + (float)a + " b = " + (float)b);
                else Console.WriteLine("b = " + (float)b);
                Console.WriteLine("mid = " + ((float)(a + b) / (float)2));
            }
            List<float> quantized = new List<float>();
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            for (int i = 0; i < InputSignal.Samples.Count; ++i)
            {
                for (int j = 0; j < limits.Count; ++j)
                {
                    if (j < limits.Count - 1 && (((float)limits[j] < (float)InputSignal.Samples[i] && (float)limits[j + 1] >= (float)InputSignal.Samples[i]) ||
                        ((float)limits[j] <= (float)InputSignal.Samples[i] && (float)limits[j + 1] > (float)InputSignal.Samples[i])))
                    {
                        float va = midPoint[(j + j + 1) / 2];
                        quantized.Add(va);
                        Console.WriteLine(va);
                        OutputSamplesError.Add((float)((float)va - (float)InputSignal.Samples[i]));
                        Console.WriteLine("error= " + (float)((float)va - (float)InputSignal.Samples[i]));
                        // a --> interval indicies
                        int a = ((j + j + 1) / 2) + 1;
                        OutputIntervalIndices.Add(a);
                        break;
                    }
                    else if (j == limits.Count - 1 && InputSignal.Samples[i] > limits[j])
                    {
                        float va = midPoint[midPoint.Count - 1];
                        quantized.Add(va);
                        Console.WriteLine(va);
                        OutputSamplesError.Add(va - InputSignal.Samples[i]);
                        OutputIntervalIndices.Add((midPoint.Count));
                        break;
                    }
                }
            }
            OutputEncodedSignal = new List<string>();
            for (int i = 0; i < OutputIntervalIndices.Count; ++i)
            {
                string s = "";
                int a = OutputIntervalIndices[i] - 1;
                Console.WriteLine("a = " + a);
                while (a != 0)
                {
                    int r = a % 2;
                    if (r == 1) s += '1';
                    else s += '0';
                    a /= 2;
                }
                string t = "";
                if (s.Length < InputNumBits) for (int j = 0; j < InputNumBits - s.Length; ++j) t += '0';
                if (s.Length > 0)
                    for (int j = s.Length - 1; j >= 0; --j)
                    {
                        t += s[j];
                    }
                Console.WriteLine(t);
                OutputEncodedSignal.Add(t);
            }
            OutputQuantizedSignal = new Signal(quantized, false);
        }

    }
}

