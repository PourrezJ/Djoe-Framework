using System.Collections.Generic;

namespace Shared
{
    public class StableHorse
    {
        public string Name;
        public Dictionary<string, string> Variation;
        public double Price;
        public double InvWeight;
    }

    public class CompomentHorse
    {
        public string Name;
        public double Price;
        public Dictionary<string, string[]> Variations;
    }
}
