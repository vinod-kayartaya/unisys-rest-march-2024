using System.Xml.Serialization;

namespace AspNetCoreCustomersWebApi.Models
{
    public class Customer
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
    }
}
