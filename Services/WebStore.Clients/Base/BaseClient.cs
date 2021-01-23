using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient
    {
        protected string Address { get; }

        protected HttpClient Http { get; }

        protected BaseClient (HttpClient Client, string ServiceAddress)
        {
            Http = Client;
            Address = ServiceAddress;
        }
    }
}
