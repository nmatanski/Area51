using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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


            foreach (var agent in Area51.Agents)
            {
                Console.Write($"{agent.Key}: {agent.Value.Level.ToString()} (Access: ");
                agent.Value.FloorAccess.ForEach(access => Console.Write(access.ToString() + ' '));
                Console.Write($"), is in {agent.Value.CurrentFloor.ToString()} Level ({Convert.ToInt32(agent.Value.CurrentFloor)}) and wants to go to {agent.Value.TargetFloor} Level ({Convert.ToInt32(agent.Value.TargetFloor)}) with daily wrong attempts: {agent.Value.WrongAttempts}");
                Console.WriteLine();
            }

            Console.WriteLine($"\n{Count} agents have been generated.");

            Console.WriteLine("\n\nLoading for a sec...\n");
            Thread.Sleep(1000);


            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();

            //Area51.Agents[0].Level = SecurityLevel.TopSecret;
            //Console.WriteLine(Area51.Agents[0].Level.ToString());
        }
    }
}
