using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ClientModel = ClientBasicCrud.Model.Client;

namespace ClientBasicCrud.Repository.Client
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext context;
        public ClientRepository(AppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ClientModel> ListClients()
        {
            return this.context.Clients;
        }

        public ClientModel Get(int id)
        {
            return this.context.Clients.Find(id);
        }

        public void Update(ClientModel client)
        {
            this.context.Entry(client).State = EntityState.Modified;
            this.context.SaveChanges();
        }

        public ClientModel Create(ClientModel client)
        {
            var newClient = this.context.Clients.Add(
               new ClientModel
               {
                   Name = client.Name,
                   Birthdate = client.Birthdate,
                   RegistrationDate = client.RegistrationDate
               }
            ).Entity;

            this.context.SaveChanges();

            return newClient;
        }

        public ClientModel Delete(int id)
        {
            var client = this.context.Clients.Find(id);
            if (client == null)
            {
                return new ClientModel { };
            }

            this.context.Clients.Remove(client);
            context.SaveChanges();

            return client;
        }

        public bool IsClientExists(int id)
        {
            return this.context.Clients.Any(e => e.Id == id);
        }
    }
}
