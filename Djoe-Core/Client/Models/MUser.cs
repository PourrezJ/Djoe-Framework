using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vorpcore_cl.Models
{
    public static class MUser
    {
       public static string firstname { get; set; }
       public static string lastname { get; set; }
       public static string group { get; set; }
       public static int xp { get; set; }
       public static int level { get; set; }
       public static string job { get; set; }
        
        public static void setUser(string characterName, string characterSurname, string usergroup, int characterXp, int characterLevel, string characterJob)
        {
            firstname = characterName;
            lastname = characterSurname;
            group = usergroup;
            xp = characterXp;
            level = characterLevel;
            job = characterJob;
        }
    }

}
