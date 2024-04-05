using AspNetCoreCustomersWebApi.Models;
using System.Xml.Serialization;

namespace AspNetCoreCustomersWebApi.DTO
{
    public class CustomerDTO
    {
        [XmlAttribute]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }

        
        public static CustomerDTO FromCustomer(Customer customer)
        {
            return new CustomerDTO
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                City= customer.City
            };
        }

        public static Customer FromCustomerDTO(CustomerDTO customerDTO)
        {
            return new Customer
            {
                Id = customerDTO.Id,
                Name = customerDTO.Name,
                Email = customerDTO.Email,
                Phone = customerDTO.Phone,
                City= customerDTO.City
            };
        }
    }
}
