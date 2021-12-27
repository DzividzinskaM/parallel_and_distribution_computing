using Akka.Actor;
using lab2.Messages;
using lab2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace lab2.Actors
{
    class ClientActor : ReceiveActor
    {
        public Client Client { get; }
        private IActorRef _paydeskSystem;

        public ClientActor(string clientId, IActorRef paydesk)
        {
            Client = new Client(clientId);
            Random rand = new Random();
            _paydeskSystem = paydesk;

            Receive<MakeShopping>(h =>
            {
                int time = (Int32)rand.NextDouble() * 5000;
                Console.WriteLine($"Client ${h.Client.Id} make order on paydesk N {h.Paydesk.Id} for {time} milliseconds");
                Thread.Sleep(time);
                h.Paydesk.RemoveClient(h.Client);
                if(h.Paydesk.Count > 0)
                {
                    Sender.Tell(new MakeShopping(h.Paydesk.GetNextClient(), h.Paydesk));
                }
            });

            Receive<TakeQueue>(h =>
            {
                Console.WriteLine($"Client {h.Client.Id} is take queue ${h.Paydesk.Id}");
                h.Paydesk.AddClient(h.Client);
                if (h.Paydesk.Count == 1)
                {
                    Sender.Tell(new MakeShopping(h.Client, h.Paydesk));
                }
                else
                {
                    Sender.Tell(new IsMoreComfortableQueue(h.Client, h.Paydesk));
                }
            });

            GoToStop(clientId);
        }

        private void GoToStop(string clientId)
        {
            _paydeskSystem.Tell(new EnterClient(new Client(clientId)));
        }
    }
}
