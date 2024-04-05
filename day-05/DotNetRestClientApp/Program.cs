using System.Formats.Asn1;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Serialization;
using System.Xml;


namespace DotNetRestClientApp
{
    internal class Program
    {

        static readonly string BASE_URL = "http://54.206.76.39:8080/api/contacts/";

        static async Task GetContactById(string id)
        {
            using(HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp =  await client.GetAsync($"{BASE_URL}{id}");
                    string responseBody = await resp.Content.ReadAsStringAsync();
                if(resp.IsSuccessStatusCode)
                {
                    Console.WriteLine("Successfully retrieved the data");
                }
                else
                {
                    Console.WriteLine(resp.StatusCode);
                }
                Console.WriteLine(responseBody);
            }
        }

        static async Task GetAllContacts()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(BASE_URL);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
            }
        }

        static async Task GetContactDetails(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(BASE_URL + id);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    JObject contact = JObject.Parse(responseBody);
                    string name = contact["firstname"] + " " + contact["lastname"];
                    Console.WriteLine(name);
                    // Console.WriteLine(contact["firstname"]);
                }
            }
        }

        static async Task GetCustomerRepresentation(string id, string mimeType)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", mimeType);
                HttpResponseMessage resp = await client.GetAsync(BASE_URL + id);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine("Request MIME type: " + mimeType);
                    Console.WriteLine(responseBody);
                    Console.WriteLine("--------------------------------------------------");
                }
                else
                {
                    Console.WriteLine("It was not successful for MIME type: " + mimeType);
                    Console.WriteLine("Status code: " + (int)resp.StatusCode);
                    Console.WriteLine("Status text: " + resp.StatusCode);
                }
            }
        }

        static async Task GetContactsFromState(string state)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var query = $"?state={state}";

                HttpResponseMessage resp = await client.GetAsync(BASE_URL + query);
                if(resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    var data = JObject.Parse(responseBody);
                    var contacts = data["contacts"];
                    if (contacts != null)
                    {
                        foreach (var c in contacts)
                        {
                            var firstname = c["firstname"];
                            var lastname = c["lastname"];
                            var city = c["city"];
                            var region = c["state"];

                            Console.WriteLine($"{firstname} {lastname} is from {city}/{region}");
                        }
                    }
                    
                }
            }
        }

        static async Task GetContactAsModelObjectFromJson(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage resp = await client.GetAsync(BASE_URL + id);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    Contact? contact = JsonConvert.DeserializeObject<Contact>(responseBody);

                    Console.WriteLine($"Name = {contact?.Firstname} {contact?.Lastname}");
                    Console.WriteLine($"City = {contact?.City}");
                }
            }
        }

        static async Task GetContactsByCityAsListFromJson(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                var query = $"?city={city}";
                HttpResponseMessage resp = await client.GetAsync(BASE_URL + query);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    ContactList body = JsonConvert.DeserializeObject<ContactList>(responseBody);

                    foreach (var contact in body.Contacts)
                    {
                        Console.WriteLine($"Name = {contact?.Firstname} {contact?.Lastname}");
                    }
                }
            }
        }

        // some issue with xml namespace; see if you can fix this.
        static async Task GetContactAsModelObjectFromXml(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/xml");
                HttpResponseMessage resp = await client.GetAsync(BASE_URL + id);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    XmlSerializer serializer = new XmlSerializer(typeof(Contact));
                    using (TextReader reader = new StringReader(responseBody))
                    {
                        Contact contact = (Contact) serializer.Deserialize(reader);
                        Console.WriteLine($"Name = {contact?.Firstname} {contact?.Lastname}");
                        Console.WriteLine($"City = {contact?.City}");
                    }
                }
            }
        }

        static async Task Main(string[] args)
        {
            // await GetContactById("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");
            // await GetContactById("95c41e9e-9b7e-429d-ba4e-5e2e1a440440");
            // await GetAllContacts();
            // await GetContactDetails("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");

            // await GetCustomerRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "application/json");
            // await GetCustomerRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "application/xml");
            // await GetCustomerRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "text/plain");
            // await GetCustomerRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "text/csv");

            // await GetContactsFromState("Texas");

            // await GetContactAsModelObjectFromJson("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");
            // await GetContactsByCityAsListFromJson("El Paso");
            
            // await GetContactAsModelObjectFromXml("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");
        }
    }
}
