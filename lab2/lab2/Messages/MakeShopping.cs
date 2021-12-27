using lab2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Messages
{
    public class MakeShopping
    {
        public Client Client { get; }
        public Paydesk Paydesk { get; }

        public MakeShopping(Client client, Paydesk paydesk)
        {
            Client = client;
            Paydesk = paydesk;

        }
    }
}
