using AspNetCoreCustomersWebApi.Models;

namespace AspNetCoreCustomersWebApi.Services
{
    public class CustomerService
    {
        private readonly IList<Customer> _customers;

        public CustomerService() { 
            _customers = new List<Customer>();
            _customers.Add(new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Vinod",
                City = "Bangalore",
                Email = "vinod@vinod.co",
                Phone = "9731424784"
            });

            _customers.Add(new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Johh Doe",
                City = "Dallas",
                Email = "johndoe@xmpl.com",
                Phone = "5011424001"
            });

            _customers.Add(new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                City = "Washington",
                Email = "janedoe@xmpl.co",
                Phone = "5091421234"
            });
        }
   
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customers;
        }

        public Customer GetCustomerById(Guid id)
        {
            return _customers.FirstOrDefault(c => c.Id == id);
        }

        public void AddCustomer(Customer customer)
        {
            if(string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) ||
                string.IsNullOrWhiteSpace(customer.Phone))
            {
                throw new ArgumentException("missing one ore more of mandatory fields: name/email/phone");
            }

            if(_customers.Any(c => c.Email== customer.Email)) {
                throw new ArgumentException("another customer with this email is already present");
            }

            if (_customers.Any(c => c.Phone == customer.Phone))
            {
                throw new ArgumentException("another customer with this phone is already present");
            }

            // if all is well
            customer.Id = Guid.NewGuid();   
            _customers.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) ||
                string.IsNullOrWhiteSpace(customer.Phone))
            {
                throw new ArgumentException("missing one ore more of mandatory fields: name/email/phone");
            }

            var existingCustomerData = _customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomerData == null)
            {
                throw new ArgumentException("no customer found for the given id");
            }
            if (_customers.Any(c => c.Id != customer.Id && c.Email == customer.Email))
            {
                throw new ArgumentException("this email is already registered by another customer");
            }
            if (_customers.Any(c => c.Id != customer.Id && c.Phone == customer.Phone))
            {
                throw new ArgumentException("this phone is already registered by another customer");
            }
            existingCustomerData.Name = customer.Name;
            existingCustomerData.Email = customer.Email;
            existingCustomerData.Phone = customer.Phone;
            existingCustomerData.City = customer.City;
        }

        public Customer DeleteCustomer(Guid customerId) {
            var existingCustomerData = _customers.FirstOrDefault(c => c.Id == customerId);
            if (existingCustomerData == null)
            {
                throw new ArgumentException("no customer data found for the given id");
            }

            _customers.Remove(existingCustomerData);
            return existingCustomerData;
        }
    }
}
