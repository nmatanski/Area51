using System;
using System.Threading;

namespace Area51App
{
    public static class Util
    {
        private static Random _rng = new Random(DateTime.Now.Millisecond);

        public static object QueueLock { get; } = new object();


        public static void AgentProducer(Agent agent)
        {
            int count = 0;
            while (true)
            {
                Thread.Sleep(3000);
                Console.WriteLine($"The agent {agent.Id} has finished its work. The agent's going to the elevator now...");

                Thread.Sleep(50);
                lock (QueueLock)
                {
                    if (agent.Level == SecurityLevel.Confidential)
                    {
                        Area51.Agents.TryRemove(agent.Id, out Agent temp);
                        Console.WriteLine($"Agent {agent.Id} with Confidential Security Level has tried to access the Elevator, but he has no such permissions. This agent can't be enqueued again unless his SecurityLevel is granted with more permissions.");
                        Elevator.Signal.Set();
                        //agent.Signal.WaitOne(); // ?
                        var tempCheck = Elevator.Passenger == null ? "null" : Elevator.Passenger.Id.ToString();
                        Console.WriteLine($"Agent {agent.Id} is feeling sad. He's going to work now... (Elevator's passenger check: {tempCheck})");
                        Thread.Sleep(200);
                        return;
                    }
                    Elevator.Agents.Enqueue(agent);
                    Console.WriteLine($"The agent {agent.Id} has been enqueued to the elevator's queue");
                }
                Elevator.Signal.Set();

                // the agent is in the queue (waiting)
                agent.Signal.WaitOne(); // waiting for Set()

                count++;
                Console.WriteLine($"This agent {agent.Id} has been in the elevator {count} times and now he's rdy to go to work and to go to the elevator again...");
                Agent.GetCorrectRandomTargetFloor(agent);
                Thread.Sleep(2000);
            }
        }

        public static void ElevatorConsumer()
        {
            Agent previousAgent = null;
            int currentFloor;
            while (true)
            {
                if (previousAgent == null)
                    currentFloor = 0;
                else
                    currentFloor = (int)previousAgent.CurrentFloor;
                bool hasAgent;
                lock (QueueLock) // the elevator stops working here with 5-10+ agents, why?
                {
                    hasAgent = Elevator.Agents.Count > 0;
                }
                if (hasAgent)
                {
                    Agent passenger;
                    lock (QueueLock)
                    {
                        passenger = Elevator.Agents.Dequeue();
                    }
                    // the elevator removes an agent from the queue, then it goes to the agent's floor (time to reach the new floor = ?)
                    var tempTime = 1000 * Math.Abs(currentFloor - (int)passenger.CurrentFloor);
                    Console.WriteLine($"The elevator is on the {(Access)currentFloor} Floor and it's going to Agent {passenger.Id} (on floor {passenger.CurrentFloor}), because he's the next in the queue.\nThe elevator will be there in {tempTime / 1000} seconds");
                    Thread.Sleep(tempTime == 0 ? 40 : tempTime);
                    // the elevator goes to the target floor with the agent inside
                    currentFloor = (int)passenger.CurrentFloor;
                    tempTime = 1000 * Math.Abs(currentFloor - (int)passenger.TargetFloor);
                    Console.WriteLine($"An agent {passenger.Id} on Level {passenger.CurrentFloor.ToString()} entered the Elevator and now he's going to Level {passenger.TargetFloor.ToString()}. The elevator will be there in {tempTime / 1000} seconds");
                    Thread.Sleep(tempTime == 0 ? 40 : tempTime);
                    passenger.CurrentFloor = passenger.TargetFloor; // change the current floor
                    passenger.Signal.Set(); // unblock the agent's task
                    Console.WriteLine("Some agent's just exited the elevator, now the agent's working smth for 5 seconds...");
                    previousAgent = passenger;
                    // the agent's working smth before he join the queue again
                }
                else
                {
                    Elevator.Signal.WaitOne();
                }
            }
        }


        public static T RandomEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            lock (_rng)
            {
                return (T)Convert.ChangeType(values.GetValue(_rng.Next(values.Length)), typeof(T));
            }
        }

    }
}
