namespace Identity.API
{
    using IdentityModel.Client;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class TokenFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _token;

        public TokenFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_token))
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = "https://localhost:5003/connect/token",
                    ClientId = "identity",
                    ClientSecret = "secret",
                    Scope = "user.write"
                });

                if (response.IsError)
                    throw new InvalidOperationException("Could not get token");

                _token = response.AccessToken;
            }

            return _token;
        }
    }
}
