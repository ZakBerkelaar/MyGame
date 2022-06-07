using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Utils
{
    public class JobPerformer
    {
        private readonly List<Job> jobs;
        private readonly List<float> accumulator;

        public JobPerformer()
        {
            jobs = new List<Job>();
            accumulator = new List<float>();
        }

        public void Add(Job job)
        {
            jobs.Add(job);
            accumulator.Add(0);
        }

        public bool Remove(Job job)
        {
            int i = jobs.IndexOf(job);
            if(i == -1)
                return false;
            jobs.RemoveAt(i);
            accumulator.RemoveAt(i);
            return true;
        }

        public void Update(float dt)
        {
            for (int i = 0; i < jobs.Count; i++)
            {
                accumulator[i] += dt;
                if(accumulator[i] >= jobs[i].Interval)
                {
                    accumulator[i] = 0;
                    jobs[i].Action();
                }
            }
        }
    }
}
