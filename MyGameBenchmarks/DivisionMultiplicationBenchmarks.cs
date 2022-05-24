using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGameBenchmarks
{
    public class DivisionMultiplicationBenchmarks
    {
        private Random random;

        private static int width;

        public DivisionMultiplicationBenchmarks()
        {
            random = new Random();
            width = 800;
        }

        [Benchmark]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public float ScreenToNormalDiv()
        {
            return (random.Next(width) / (width / 2)) - 1;
        }

        [Benchmark]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public float ScreenToNormalMul()
        {
            return ((2*random.Next(width)) * width) - 1;
        }

        [Benchmark]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public float ScreenToNormalMulShift()
        {
            return ((random.Next(width) << 1) * width) - 1;
        }
    }
}
