//Aluno Alison Civiero 451219
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CallCenterDemo
{
    internal class Program
    {
        // Thread-local Random to avoid contention and repeated seeds
        private static readonly ThreadLocal<Random> _rng =
            new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

        static void Main(string[] args)
        {
            var center = new CallCenter();

            Parallel.Invoke(
                () => CallersAction(center),
                () => ConsultantAction(center, "Marcin", ConsoleColor.Red),
                () => ConsultantAction(center, "James", ConsoleColor.Yellow),
                () => ConsultantAction(center, "Olivia", ConsoleColor.Green)
            );
        }

        private static void CallersAction(CallCenter center)
        {
            var random = _rng.Value!;
            while (true)
            {
                int clientId = random.Next(1, 10000);
                int waitingCount = center.Call(clientId);
                Log($"Incoming call from {clientId}, waiting in the queue: {waitingCount}");
                Thread.Sleep(random.Next(1000, 5000)); // 1..5s between calls
            }
        }

        private static void ConsultantAction(CallCenter center, string name, ConsoleColor color)
        {
            var random = _rng.Value!;
            while (true)
            {
                var call = center.Answer(name);
                if (call != null)
                {
                    var oldColor = Console.ForegroundColor;
                    Console.ForegroundColor = color;
                    Log($"Call #{call.Id} from {call.ClientId} is answered by {call.Consultant}.");
                    Console.ForegroundColor = oldColor;

                    Thread.Sleep(random.Next(1000, 10000)); // 1..10s handling the call
                    center.End(call);

                    Console.ForegroundColor = color;
                    Log($"Call #{call.Id} from {call.ClientId} is ended by {call.Consultant}.");
                    Console.ForegroundColor = oldColor;

                    Thread.Sleep(random.Next(500, 1000)); // 0.5..1s between calls
                }
                else
                {
                    Thread.Sleep(100); // brief wait if no calls
                }
            }
        }

        private static void Log(string text)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {text}");
        }
    }
}

