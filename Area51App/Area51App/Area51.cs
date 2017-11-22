using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51App
{
    public static class Area51
    {


        public static ConcurrentDictionary<int, Agent> Agents { get; set; } = new ConcurrentDictionary<int, Agent>();

        static Area51()
        {
            PopulateAgents();
        }


        private static void PopulateAgents()
        {
            for (int i = 1; i <= Program.Count; i++)
            {
                Agents.TryAdd(i, new Agent(Util.RandomEnum<SecurityLevel>(), Util.RandomEnum<Access>()));
            }

        }


    }
}
