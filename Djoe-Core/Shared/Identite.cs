namespace Shared
{
    public class Identite
    {
        public string LastName;
        public string FirstName;
        public int Age;
        public string Nationality;

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }
    }
}
