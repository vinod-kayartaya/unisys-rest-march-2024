using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Xml.Serialization;

using System.Text.Encodings;
using System.Net.Http.Headers;

namespace DotNetRestClientApp
{
    internal class Program
    {

        static readonly string BASE_URL = "http://54.206.76.39:8080/api/contacts/";

        static async Task GetContactById(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync($"{BASE_URL}{id}");
                string responseBody = await resp.Content.ReadAsStringAsync();
                if (resp.IsSuccessStatusCode)
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

        static async Task GetContactRepresentation(string id, string mimeType)
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
                if (resp.IsSuccessStatusCode)
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
                        Contact contact = (Contact)serializer.Deserialize(reader);
                        Console.WriteLine($"Name = {contact?.Firstname} {contact?.Lastname}");
                        Console.WriteLine($"City = {contact?.City}");
                    }
                }
            }
        }


        static async Task AddNewContactFromStringJson()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                MediaTypeHeaderValue headerValue = new MediaTypeHeaderValue("application/json");
                string jsonContact = @"{
""firstname"": ""Kishore"",
""email"": ""kishore@xmpl.com"",
""phone"": ""9874561230""
}";
                var payload = new StringContent(jsonContact, headerValue);
                HttpResponseMessage resp = await client.PostAsync(BASE_URL, payload);

                Console.WriteLine("Response code is " + ((int)resp.StatusCode));
                JObject body = JObject.Parse(await resp.Content.ReadAsStringAsync());
                Console.WriteLine(body);
            }
        }
        static async Task AddNewContactFromStringXml()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/xml");
                MediaTypeHeaderValue headerValue = new MediaTypeHeaderValue("application/xml");
                string jsonContact = @"<?xml version=""1.0""?>
<contact>
    <firstname>James Miller</firstname>
    <email>jamesmiller@xmpl.com</email>
    <phone>5675673345</phone>
</contact>";
                var payload = new StringContent(jsonContact, headerValue);
                HttpResponseMessage resp = await client.PostAsync(BASE_URL, payload);

                Console.WriteLine("Response code is " + ((int)resp.StatusCode));
                var responseBody = await resp.Content.ReadAsStringAsync();
                // JObject body = JObject.Parse(responseBody);
                Console.WriteLine(responseBody);
            }
        }

        static async Task AddNewContact()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                MediaTypeHeaderValue headerValue = new("application/json");
                Contact contact = new()
                {
                    Firstname = "Vinod",
                    Lastname = "Kumar",
                    Email = "vinod@vinod.co",
                    Phone = "9731424784"
                };

                var jsonContact = JsonConvert.SerializeObject(contact);
                var payload = new StringContent(jsonContact, headerValue);
                HttpResponseMessage resp = await client.PostAsync(BASE_URL, payload);

                Console.WriteLine("Response code is " + ((int)resp.StatusCode));
                JObject body = JObject.Parse(await resp.Content.ReadAsStringAsync());
                Console.WriteLine(body);
            }
        }


        static async Task UpdateContactData()
        {
            Console.Write("enter contact id to search: ");
            string? id = Console.ReadLine();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage resp = await client.GetAsync(BASE_URL + id);
                if (resp.IsSuccessStatusCode)
                {
                    string responseBody = await resp.Content.ReadAsStringAsync();
                    Contact? c = JsonConvert.DeserializeObject<Contact>(responseBody);
                    string input;

                    // display the current value, accept new value
                    Console.Write($"Firstname : ({c?.Firstname}) ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        c.Firstname = input;
                    }

                    Console.Write($"Lastname : ({c?.Lastname}) ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        c.Lastname = input;
                    }

                    Console.Write($"City : ({c?.City}) ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        c.City = input;
                    }

                    Console.Write($"State : ({c?.State}) ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        c.State = input;
                    }

                    Console.Write($"Country : ({c?.Country}) ");
                    input = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(input))
                    {
                        c.Country = input;
                    }

                    MediaTypeHeaderValue headerValue = new("application/json");
                    var jsonContact = JsonConvert.SerializeObject(c);
                    var payload = new StringContent(jsonContact, headerValue);
                    resp = await client.PutAsync(BASE_URL+id, payload);

                    if (resp.IsSuccessStatusCode)
                    {
                        await Console.Out.WriteLineAsync("Customer data updated successfully");
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync(await resp.Content.ReadAsStringAsync());
                    }

                }
                else
                {
                    Console.WriteLine("No data found (may be invalid id)");
                }
            }

            return;
        }

        static async Task UpdateContactEmail(string id, string email)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                MediaTypeHeaderValue headerValue = new("application/json");
                string jsonData = "{\"email\": \"" + email + "\"}";
                var payload = new StringContent(jsonData, headerValue);
                HttpResponseMessage resp = await client.PatchAsync(BASE_URL+id, payload);

                Console.WriteLine("Response code is " + ((int)resp.StatusCode));
                JObject body = JObject.Parse(await resp.Content.ReadAsStringAsync());
                Console.WriteLine(body);
            }
        }

        static async Task DeleteContact(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                HttpResponseMessage resp = await client.DeleteAsync(BASE_URL + id);

                Console.WriteLine("Response code is " + ((int)resp.StatusCode));
                JObject body = JObject.Parse(await resp.Content.ReadAsStringAsync());
                Console.WriteLine(body);
            }
        }

        static async Task GetOrdersByCustomer()
        {
            // this is making a request to a different endpoint than all other examples.
            string baseUrl = "http://54.206.76.39:8080/";
            Console.Write("enter your email: ");
            string email = Console.ReadLine();
            Console.Write("enter your password: ");
            string password = Console.ReadLine();

            Dictionary<string, string> user = new (){
                { "email", email},
                { "password", password}
            };

            var userJson = JsonConvert.SerializeObject(user);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                MediaTypeHeaderValue headerValue = new("application/json");
                var payload = new StringContent(userJson, headerValue);
                HttpResponseMessage resp = await client.PostAsync(baseUrl+"login", payload);

                if(resp.IsSuccessStatusCode)
                {

                    JObject responseBody = JObject.Parse(await resp.Content.ReadAsStringAsync());
                    Console.WriteLine($"fetching orders for {responseBody["name"]}...");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseBody["token"].ToString());

                    resp = await client.GetAsync(baseUrl + "orders");
                    var strOrders = await resp.Content.ReadAsStringAsync();

                    var orders = JArray.Parse(strOrders);
                    if (resp.IsSuccessStatusCode)
                    {
                        foreach (var order in orders)
                        {
                            Console.WriteLine("ID = " + order["id"]);
                            Console.WriteLine("Date = " + order["orderDate"]);
                            Console.WriteLine("Status = " + order["orderStatus"]);
                            Console.WriteLine("-----------------------------");
                        }
                    }
                    else
                    {
                        Console.WriteLine(responseBody);
                    }
                }
                else
                {
                    Console.WriteLine(await resp.Content.ReadAsStringAsync());
                }
            }
        }



        static async Task Main(string[] args)
        {
            // await GetContactById("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");
            // await GetContactById("95c41e9e-9b7e-429d-ba4e-5e2e1a440440");
            // await GetAllContacts();
            // await GetContactDetails("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");

            // await GetContactRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "application/json");
            // await GetContactRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "application/xml");
            // await GetContactRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "text/plain");
            // await GetContactRepresentation("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b", "text/csv");

            // await GetContactsFromState("Texas");

            // await GetContactAsModelObjectFromJson("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");
            // await GetContactsByCityAsListFromJson("El Paso");

            // await GetContactAsModelObjectFromXml("95c41e9e-9b7e-429d-ba4e-5e2e1a44044b");

            // await AddNewContactFromStringJson();
            // await AddNewContactFromStringXml();
            // await AddNewContact();
            //  await UpdateContactData();
            // await UpdateContactEmail("7423e648-e7ff-4c83-9bc0-665d83b95bf5", "vinod@vinod.co");
            // await DeleteContact("7423e648-e7ff-4c83-9bc0-665d83b95bf5");

            await GetOrdersByCustomer();
        }

        
    }
}
