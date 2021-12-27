using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab2.Models
{
    public class Paydesk
    {
        public string Id { get; }

        private readonly List<Client> _clients;

        public int Count => _clients.Count;

        public Paydesk(string id)
        {
            Id = id;
            _clients = new List<Client>();
        }

        public void AddClient(Client client)
        {
            _clients.Add(client);
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(_clients.FirstOrDefault(x => x.Id == client.Id));
        }

        public bool Contains(Client client)
        {
            return _clients.Any(x => x.Id == client.Id);
        }


        public bool IsEmpty(Client client)
        {
            return _clients.Count == 0 ? true : false;
        }

        public bool IsMoreComfortable(Paydesk paydesk)
        {
            return paydesk.Count > Count;
        }

        public Paydesk IsMoreComfortable(List<Paydesk> paydesks)
        {
            var better = paydesks[0];
            foreach(var paydesk in paydesks)
            {
                if (!better.IsMoreComfortable(paydesk))
                {
                    better = paydesk;
                }
            }
            return better;
        }

        public Client GetNextClient()
        {
            if (Count <= 0) return null;
            return _clients[Count - 1];
        }

    }
}
