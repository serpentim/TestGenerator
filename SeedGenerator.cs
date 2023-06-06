using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGenerator
{
    public class SeedGenerator
    {
        private Random random;

        public SeedGenerator()
        {
            random = new Random();
        }

        public SeedGenerator(int? seed)
        {
            if (seed.HasValue)
                random = new Random(seed.Value);
            else
                random = new Random();
        }

        public int GetRandomSeed()
        {
            return random.Next();
        }
    }
}
