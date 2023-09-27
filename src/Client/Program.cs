// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using IdentityModel.Client;

Console.WriteLine("Hello, World!");

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
if (disco.IsError)
{
    Console.WriteLine(disco.Error);
}
Console.WriteLine(disco.TokenEndpoint);


var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = disco.TokenEndpoint,
    ClientId = "client",
    ClientSecret = "secret",
    Scope = "api1",
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine(tokenResponse.AccessToken);


//call api 
client.SetBearerToken(tokenResponse.AccessToken);

var res = await client.GetAsync("https://localhost:6001/identity");

if (!res.IsSuccessStatusCode)
{
    Console.WriteLine(res.StatusCode);
}
else
{
    var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions
    {
        WriteIndented = true
    }));
}
