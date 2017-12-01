using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Area51App
{
    public static class Elevator
    {
        public static Queue<Agent> Agents { get; set; }

        public static Agent Passenger { get; set; }

        public static AutoResetEvent Signal { get; set; } = new AutoResetEvent(false);  // receive new agent/s


        public static void Run()
        {
            Agents = new Queue<Agent>();
            var taskElevator = Task.Factory.StartNew(() => Util.ElevatorConsumer());
        }
    }
}
