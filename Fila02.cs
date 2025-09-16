//Aluno Alison Civiero 451219
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CallCenterDemo
{
    public class CallCenter
    {
        private int _counter = 0;

        public ConcurrentQueue<IncomingCall> Calls { get; }

        public CallCenter()
        {
            Calls = new ConcurrentQueue<IncomingCall>();
        }

        // Enqueue a new call and return the number of waiting calls
        public int Call(int clientId)
        {
            var call = new IncomingCall
            {
                Id = Interlocked.Increment(ref _counter),
                ClientId = clientId,
                CallTime = DateTime.Now
            };

            Calls.Enqueue(call);
            return Calls.Count;
        }

        // Try to dequeue a waiting call for the given consultant
        public IncomingCall? Answer(string consultant)
        {
            if (Calls.TryDequeue(out var call))
            {
                call.Consultant = consultant;
                call.StartTime = DateTime.Now;
                return call;
            }
            return null;
        }

        public void End(IncomingCall call)
        {
            call.EndTime = DateTime.Now;
        }

        public bool AreWaitingCalls() => !Calls.IsEmpty;
    }
}

