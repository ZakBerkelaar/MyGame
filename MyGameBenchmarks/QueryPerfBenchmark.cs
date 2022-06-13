using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace MyGameBenchmarks
{
    public unsafe class QueryPerfBenchmark
    {
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceCounterSuppress(out long count);
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter"), SuppressUnmanagedCodeSecurity]
        private static extern bool QueryPerformanceCounterSuppressPtr(long* count);
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
        private static extern bool QueryPerformanceCounter(out long count);
        [DllImport("kernel32.dll", EntryPoint = "QueryPerformanceCounter")]
        private static extern bool QueryPerformanceCounterPtr(long* count);

        private const int iter = 10000;

        [Benchmark]
        public long Suppress()
        {
            unchecked
            {
                long sum = 0;
                for (int i = 0; i < iter; i++)
                {
                    QueryPerformanceCounterSuppress(out long temp);
                    sum += temp;
                }
                return sum;
            }
        }

        [Benchmark]
        public long SuppressPtr()
        {
            unchecked
            {
                long sum = 0;
                long tmp;
                for (int i = 0; i < iter; i++)
                {
                    QueryPerformanceCounterSuppressPtr(&tmp);
                    sum += tmp;
                }
                return sum;
            }
        }

        [Benchmark]
        public long NoSuppress()
        {
            unchecked
            {
                long sum = 0;
                for (int i = 0; i < iter; i++)
                {
                    QueryPerformanceCounter(out long temp);
                    sum += temp;
                }
                return sum;
            }
        }

        [Benchmark]
        public long NoSuppressPtr()
        {
            unchecked
            {
                long sum = 0;
                long tmp;
                for (int i = 0; i < iter; i++)
                {
                    QueryPerformanceCounterPtr(&tmp);
                    sum += tmp;
                }
                return sum;
            }
        }
    }
}
