using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameBenchmarks
{
    public class SpanTesting
    {
        private Random random;
        public SpanTesting()
        {
            random = new Random();
        }

        [Benchmark]
        public void Array()
        {
            float[] arr = new float[30];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (float)random.NextDouble();
        }

        [Benchmark]
        public void Span()
        {
            Span<float> arr = stackalloc float[30];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (float)random.NextDouble();
        }
    }
}
