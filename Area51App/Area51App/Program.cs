using System;
using System.Threading;

namespace Area51App
{
    class Program
    {
        public static int Count { get; set; }


        static void Main(string[] args)
        {

            Console.Write("How many agents do you want to generate? ");
            if (!int.TryParse(Console.ReadLine(), out int temp)) temp = 0;
            Count = temp;

            Console.WriteLine("Generating agents...");

            foreach (var agent in Area51.Agents) // starts a task for every Agent through the static class
            {
                Console.Write($"{agent.Key}: {agent.Value.Level.ToString()} (Access: ");
                agent.Value.FloorAccess.ForEach(access => Console.Write(access.ToString() + ' '));
                Console.Write($"), is in {agent.Value.CurrentFloor.ToString()} Level ({Convert.ToInt32(agent.Value.CurrentFloor)}) and wants to go to {agent.Value.TargetFloor} Level ({Convert.ToInt32(agent.Value.TargetFloor)}) with daily wrong attempts: {agent.Value.WrongAttempts}");
                Console.WriteLine();
            }

            Console.WriteLine($"\n{Count} agents have been generated.");

            Elevator.Run(); // starts the elevator's task 

            Console.WriteLine("\n\nLoading...\n");
            Thread.Sleep(1000);



            bool hasAgentsWithPermissions = false; // All agents have a security level Confidential
            foreach (var agent in Area51.Agents)
            {
                if (agent.Value.Level != SecurityLevel.Confidential)
                    hasAgentsWithPermissions = true;
            }
            while (hasAgentsWithPermissions) ;
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }
    }
}
