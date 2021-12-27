using Akka.Actor;
using lab2.Messages;
using lab2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab2.Actors
{
    class PaydesksActor : ReceiveActor
    {
        private readonly List<Paydesk> _paydeskModels;
        Random rand = new Random();


        public PaydesksActor()
        {
            _paydeskModels = new List<Paydesk>()
            {
                new Paydesk("1"),
                new Paydesk("2"),
                new Paydesk("3"),
            };
            Receive<EnterClient>(h =>
            {
                var lessQueue = _paydeskModels[0].IsMoreComfortable(_paydeskModels);
                Console.WriteLine($"Client {h.Client.Id} enter");
                Sender.Tell(new TakeQueue(h.Client, lessQueue));
            });
            
            Receive<LeaveQueue>(h =>
            {
                Console.WriteLine($"Client {h.Client.Id} is leave queue {h.Paydesk.Id}");
                h.Paydesk.RemoveClient(h.Client);

            });

            Receive<IsMoreComfortableQueue>(h =>
            {
                Console.WriteLine($"Client {h.Client.Id} is looking for more smaller queue");
                var better = h.Paydesk.IsMoreComfortable(_paydeskModels);
                if (better.Id == h.Paydesk.Id)
                {
                    Console.WriteLine($"Client {h.Client.Id} stay in current queue, which is better");
                    if (h.Paydesk.Count == 1)
                    {
                        Sender.Tell(new MakeShopping(h.Client, h.Paydesk));
                    }
                } else
                {
                    Console.WriteLine($"Client {h.Client.Id} change queue on {better.Id}");
                    h.Paydesk.RemoveClient(h.Client);
                    Sender.Tell(new TakeQueue(h.Client, better));
                }

            });

            Receive<MakeShopping>(h =>
            {
                Console.WriteLine($"Client ${h.Client.Id} make order on paydesk N {h.Paydesk.Id}");
                Thread.Sleep((Int32)rand.NextDouble() * 5000);
                h.Paydesk.RemoveClient(h.Client);
                if (h.Paydesk.Count > 0)
                {
                    Sender.Tell(new MakeShopping(h.Paydesk.GetNextClient(), h.Paydesk));
                }
            });
        }

    }
}
