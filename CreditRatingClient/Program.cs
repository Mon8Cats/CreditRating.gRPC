using System;
using System.Threading.Tasks;
using CreditRatingClient;
using Grpc.Net.Client;
using System.Runtime.InteropServices;

namespace CreditRatingClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serverAddress = "https://localhost:5001";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                // The following statement allows you to call insecure services. To be used only in development environments.
                AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
                serverAddress = "http://localhost:5000";
            }

            var channel = GrpcChannel.ForAddress(serverAddress);
            var client =  new CreditRatingCheck.CreditRatingCheckClient(channel);
            //var creditRequest = new CreditRequest { CustomerId = "id0201", Credit = 700000};
            //var reply = await client.CheckCreditRequestAsync(creditRequest);

            /*
             {"id0201", 10000},
            {"id0417", 5000},
            {"id0306", 15000}

            */

            var creditReuests = new List<CreditRequest>
            {
                new CreditRequest { CustomerId = "id0201", Credit = 7000},
                new CreditRequest { CustomerId = "id0417", Credit = 7000}
            };

            foreach (var cr in creditReuests)
            {
                var reply = await client.CheckCreditRequestAsync(cr);
                Console.WriteLine($"Credit for customer {cr.CustomerId} {(reply.IsAccepted ? "approved" : "rejected")}!");
            }

            //Console.WriteLine($"Credit for customer {creditRequest.CustomerId} {(reply.IsAccepted ? "approved" : "rejected")}!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}