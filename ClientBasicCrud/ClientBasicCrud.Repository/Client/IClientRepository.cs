using System.Collections.Generic;
using ClientModel = ClientBasicCrud.Model.Client;

namespace ClientBasicCrud.Repository.Client
{
    public interface IClientRepository
    {
        ClientModel Create(ClientModel client);
        ClientModel Delete(int id);
        ClientModel Get(int id);
        bool IsClientExists(int id);
        IEnumerable<ClientModel> ListClients();
        void Update(ClientModel client);
    }
}