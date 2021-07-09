

namespace CountriesAPI.Services
{
    using Models;
    using System.Net;

    public class NetworkService
    {
        public Response CheckConnection()
        {
            var client = new WebClient();

            try
            {
                using (client.OpenRead("http://client3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSuccess = true
                    };
                }
            }
            catch 
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Internet connection not available"
                };
            }
        }
    }
}
