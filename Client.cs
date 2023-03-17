using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice_bike
{

        public class Client
        {
            public string Nom { get; set; }
            public string Adresse { get; set; }
            public string TVA { get; set; }
        }
        public static class ClientManager
        {
            private static List<Client> clients = new List<Client>();

            public static void AjouterClient(Client client)
            {
                clients.Add(client);
            }

            public static List<Client> GetClients()
            {
                return clients;
            }
        }
}
