using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();
            List<float> inverseSamples_Folder = new List<float>();
            List<int> inverseSamplesIndexes_Folder = new List<int>();
            for (int y = InputSignal.Samples.Count - 1; y >= 0; --y)
            {
                inverseSamplesIndexes_Folder.Add(-1 * InputSignal.SamplesIndices[y]);
                inverseSamples_Folder.Add(InputSignal.Samples[y]);
            }
            OutputFoldedSignal = new Signal(inverseSamples_Folder, inverseSamplesIndexes_Folder, !InputSignal.Periodic);

        }
    }
}