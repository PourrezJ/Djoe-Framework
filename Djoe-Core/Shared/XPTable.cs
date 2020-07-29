namespace Shared
{
    public class XP
    {
        public string Name;
        public double CurrentXP;

        public XP(string name, double currentXP)
        {
            Name = name;
            CurrentXP = currentXP;
        }
    }

    public class XPTable
    {
        public XP Agriculture = new XP("Agriculture", 0.0);
        public XP Artisanat = new XP("Artisanat", 0.0);
        public XP Mineur = new XP("Mineur", 0.0);
        public XP Lumberman = new XP("Lumberman", 0.0);
    }
}
