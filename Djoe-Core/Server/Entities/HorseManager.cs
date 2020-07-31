using Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Entities
{
    public class HorseManager
    {
        public List<HorseData> HorseDatas = new List<HorseData>();

        public static void Init()
        {
            Logger.Info("Loading all horses");
        }
    }
}
