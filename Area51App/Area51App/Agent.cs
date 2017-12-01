using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Area51App
{
    public class Agent
    {
        public int Id { get; set; }

        public SecurityLevel Level { get; set; }

        public List<Access> FloorAccess
        {
            get
            {
                switch (Level)
                {
                    case SecurityLevel.TopSecret:
                        return new List<Access> { Access.G, Access.S, Access.T1, Access.T2, };
                    case SecurityLevel.Secret:
                        return new List<Access> { Access.G, Access.S };
                    case SecurityLevel.Confidential:
                        return new List<Access> { Access.G };
                    default:
                        return new List<Access> { Access.G };
                }
            }
        }

        public Access CurrentFloor { get; set; }

        public Access TargetFloor { get; set; }

        public int WrongAttempts { get; set; }

        public AutoResetEvent Signal { get; set; } = new AutoResetEvent(false);


        public Agent(int id, SecurityLevel level, Access targetFloor, Access currentFloor = 0)
        {
            Id = id;
            Level = level;
            CurrentFloor = currentFloor;

            if (FloorAccess.Count == 1)
            {
                WrongAttempts = -1; // no permissions
                TargetFloor = Access.G;
                var dumbAgent = Task.Factory.StartNew(() => Util.AgentProducer(this));
                return;
            }

            TargetFloor = targetFloor;

            GetCorrectRandomTargetFloor(this);

            // start a task for every agent
            // var taskAgent = Task.Run(() => Util.AgentProducer());
            var taskAgent = Task.Factory.StartNew(() => Util.AgentProducer(this));
        }


        public static void GetCorrectRandomTargetFloor(Agent agent)
        {
            while (!agent.FloorAccess.Contains(agent.TargetFloor) || agent.TargetFloor.Equals(agent.CurrentFloor))
            {
                agent.WrongAttempts++;
                agent.TargetFloor = Util.RandomEnum<Access>();
            }
        }
    }
}
