﻿using BenchmarkDotNet.Running;
namespace MyGameBenchmarks
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<SpanTesting>();
        }
    }
}