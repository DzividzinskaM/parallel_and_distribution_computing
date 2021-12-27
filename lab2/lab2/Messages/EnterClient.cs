using lab2.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace lab2.Messages
{
    class EnterClient
    {
        public Client Client { get; }

        public EnterClient(Client client)
        {
            Client = client;

        }

    }
}
