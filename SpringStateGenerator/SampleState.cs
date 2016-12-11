using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpringStateGenerator
{
    class SampleState
    {
        public int ElixirCount { get; private set; }
        public int AngerLevel { get; private set; }

        public SampleState()
        {
            ElixirCount = 5;
            AngerLevel = 0;
        }

        public SampleState(SampleState template)
        {
            ElixirCount = template.ElixirCount;
            AngerLevel = template.AngerLevel;
        }

        public SampleState SetElixirCount(int value)
        {
            var copy = new SampleState(this);
            copy.ElixirCount = value;
            return copy;
        }

        public SampleState SetAngerLevel(int value)
        {
            var copy = new SampleState(this);
            copy.AngerLevel = value;
            return copy;
        }

        public List<string> CompareTo(SampleState other)
        {
            var result = new List<string>();
            if (ElixirCount.Equals(other.ElixirCount))
                result.Add(String.Format("ElixirCount:\t{0}\t>>>\t{1}",ElixirCount,other.ElixirCount));
            return result;
        }
    }
}
