using lab2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Messages
{
    public class TakeQueue
    {
        public Client Client { get; }
        public Paydesk Paydesk { get; }

        public TakeQueue(Client client, Paydesk paydesk)
        {
            Client = client;
            Paydesk = paydesk;

        }

    }
}
