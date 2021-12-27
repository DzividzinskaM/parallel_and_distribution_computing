using lab2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Messages
{
    class LeaveQueue
    {
        public Client Client { get; }
        public Paydesk Paydesk { get; }

        public LeaveQueue(Client client, Paydesk paydesk)
        {
            Client = client;
            Paydesk = paydesk;
        }
    }
}
