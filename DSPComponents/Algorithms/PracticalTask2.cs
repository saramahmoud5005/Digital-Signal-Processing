using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);

            // FIR Obj
            FIR FIR_Signal_Obj = new FIR();
            FIR_Signal_Obj.InputF1 = miniF;
            FIR_Signal_Obj.InputF2 = maxF;
            FIR_Signal_Obj.InputFS = Fs;
            FIR_Signal_Obj.InputStopBandAttenuation = 50;
            FIR_Signal_Obj.InputTransitionBand = 500;         
            FIR_Signal_Obj.InputTimeDomainSignal = InputSignal;
            FIR_Signal_Obj.InputFilterType = FILTER_TYPES.BAND_PASS;
            FIR_Signal_Obj.Run();
            Signal FIR_Signal_Output = FIR_Signal_Obj.OutputYn;

            writeFileTimeDomain("D:/el kolia/4th Year/DSPToolbox/Task2Signals/FIR.ds", FIR_Signal_Output);

            bool Freq_sampling = false;
            Signal Sampling_Signal_Output=null;
            // Sampling Obj
            if (newFs >= 2 * maxF)
            {
                Freq_sampling = true;
                Sampling sampling = new Sampling();
                sampling.L = L;
                sampling.M = M;
                sampling.InputSignal = FIR_Signal_Output;
                sampling.Run();
                Sampling_Signal_Output = sampling.OutputSignal;
                //FIR_Signal_Output = sampling.OutputSignal;
                writeFileTimeDomain("D:/el kolia/4th Year/DSPToolbox/Task2Signals/Sampling.ds", Sampling_Signal_Output);
            }
            else
            {
                //throw message ((newFs is not valid))
                Console.WriteLine("newFs is not valid");
            }

            // DC Obj
            DC_Component DC_Signal_Obj = new DC_Component();
            if (Freq_sampling == true) DC_Signal_Obj.InputSignal = Sampling_Signal_Output;
            else DC_Signal_Obj.InputSignal = FIR_Signal_Output;
            DC_Signal_Obj.Run();
            Signal DC_Signal_Output = DC_Signal_Obj.OutputSignal;

            writeFileTimeDomain("D:/el kolia/4th Year/DSPToolbox/Task2Signals/Removing DC.ds", DC_Signal_Output);

            // Normalization Obj
            Normalizer Norm_Signal_Obj = new Normalizer();
            Norm_Signal_Obj.InputSignal = DC_Signal_Output;
            Norm_Signal_Obj.InputMaxRange = 1;
            Norm_Signal_Obj.InputMinRange = -1;
            Norm_Signal_Obj.Run();
            Signal Norm_Signal_Output = Norm_Signal_Obj.OutputNormalizedSignal;
            
            writeFileTimeDomain("D:/el kolia/4th Year/DSPToolbox/Task2Signals/Normalization.ds", Norm_Signal_Output);

            // DFT Obj
            DiscreteFourierTransform DFT_Signal_Obj = new DiscreteFourierTransform();
            DFT_Signal_Obj.InputTimeDomainSignal = Norm_Signal_Output;
            DFT_Signal_Obj.InputSamplingFrequency = Fs;
            DFT_Signal_Obj.Run(); // should display the result
            Signal lastbutnotleast = DFT_Signal_Obj.OutputFreqDomainSignal;
            OutputFreqDomainSignal = lastbutnotleast;  // (save DFT signal in file)
            
            writeFileFreqDomain("D:/el kolia/4th Year/DSPToolbox/Task2Signals/DFT.ds", OutputFreqDomainSignal);

        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
        public void writeFileTimeDomain(String fullPath,Signal signal_in_time_domain)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine(0); // time

                if (signal_in_time_domain.Periodic == true)
                    writer.WriteLine(1); // periodic

                else writer.WriteLine(0); // non periodic

                writer.WriteLine(signal_in_time_domain.Samples.Count);

                for (int i = 0; i < signal_in_time_domain.Samples.Count; ++i)
                {
                    writer.Write(signal_in_time_domain.SamplesIndices[i]);

                    writer.Write(" ");

                    if (i < signal_in_time_domain.Samples.Count - 1) writer.WriteLine(signal_in_time_domain.Samples[i]);

                    else if (i == signal_in_time_domain.Samples.Count - 1) writer.Write(signal_in_time_domain.Samples[i]);
                }
            }
        }
        public void writeFileFreqDomain(String fullPath, Signal signal_in_freq_domain)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine(1); // freq

                if (signal_in_freq_domain.Periodic == true)
                writer.WriteLine(1); // periodic

                else writer.WriteLine(0); // non periodic

                writer.WriteLine(signal_in_freq_domain.Frequencies.Count);

                for (int i = 0; i < signal_in_freq_domain.Frequencies.Count; ++i)
                {
                    writer.Write(signal_in_freq_domain.Frequencies[i]);

                    writer.Write(" ");

                    writer.Write(signal_in_freq_domain.FrequenciesAmplitudes[i]);

                    writer.Write(" ");

                    if (i < signal_in_freq_domain.Frequencies.Count - 1) writer.WriteLine(signal_in_freq_domain.FrequenciesPhaseShifts[i]);

                    else if (i == signal_in_freq_domain.Frequencies.Count - 1) writer.Write(signal_in_freq_domain.FrequenciesPhaseShifts[i]);
                }
            }
        }
    }
}
