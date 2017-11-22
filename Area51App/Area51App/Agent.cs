using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51App
{
    public class Agent
    {
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


        public Agent(SecurityLevel level, Access targetFloor, Access currentFloor = 0)
        {
            Level = level;
            CurrentFloor = currentFloor;

            if (FloorAccess.Count==1)
            {
                WrongAttempts++;
                TargetFloor = Access.G;
                return;
            }

            TargetFloor = targetFloor;

            while (!FloorAccess.Contains(TargetFloor) || TargetFloor.Equals(CurrentFloor))
            {
                WrongAttempts++;
                TargetFloor = Util.RandomEnum<Access>();
            }

            // start a task for every agent with Worker
        }


        public void Worker()
        {

        }



    }
}
