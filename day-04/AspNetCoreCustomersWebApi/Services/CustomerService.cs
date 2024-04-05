using AspNetCoreCustomersWebApi.Data;
using AspNetCoreCustomersWebApi.DTO;
using AspNetCoreCustomersWebApi.Models;

namespace AspNetCoreCustomersWebApi.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context) { 
            _context = context;
        }
   
        public IEnumerable<CustomerDTO> GetAllCustomers()
        {
            return _context.Customers
                .ToList() // list of Customer (model) objects
                .Select(customer => CustomerDTO.FromCustomer(customer)); // list of CustomerDTO objects
        }

        public CustomerDTO GetCustomerById(Guid id)
        {
            Customer customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if(customer == null)
            {
                return null;
            }
            return CustomerDTO.FromCustomer(customer);
        }

        public void AddCustomer(CustomerDTO customer)
        {
            if(string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) ||
                string.IsNullOrWhiteSpace(customer.Phone))
            {
                throw new ArgumentException("missing one ore more of mandatory fields: name/email/phone");
            }

            if(_context.Customers.Any(c => c.Email== customer.Email)) {
                throw new ArgumentException("another customer with this email is already present");
            }

            if (_context.Customers.Any(c => c.Phone == customer.Phone))
            {
                throw new ArgumentException("another customer with this phone is already present");
            }

            // if all is well
            _context.Customers.Add(CustomerDTO.FromCustomerDTO(customer));
            _context.SaveChanges();
        }

        public void UpdateCustomer(CustomerDTO customer)
        {
            if (string.IsNullOrWhiteSpace(customer.Name) ||
                string.IsNullOrWhiteSpace(customer.Email) ||
                string.IsNullOrWhiteSpace(customer.Phone))
            {
                throw new ArgumentException("missing one ore more of mandatory fields: name/email/phone");
            }

            var existingCustomerData = _context.Customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existingCustomerData == null)
            {
                throw new ArgumentException("no customer found for the given id");
            }
            if (_context.Customers.Any(c => c.Id != customer.Id && c.Email == customer.Email))
            {
                throw new ArgumentException("this email is already registered by another customer");
            }
            if (_context.Customers.Any(c => c.Id != customer.Id && c.Phone == customer.Phone))
            {
                throw new ArgumentException("this phone is already registered by another customer");
            }
            existingCustomerData.Name = customer.Name;
            existingCustomerData.Email = customer.Email;
            existingCustomerData.Phone = customer.Phone;
            existingCustomerData.City = customer.City;
            _context.SaveChanges(); // any changes to any of the objects managed by the dbcontext will be saved to the db
        }

        public CustomerDTO DeleteCustomer(Guid customerId) {
            var existingCustomerData = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (existingCustomerData == null)
            {
                throw new ArgumentException("no customer data found for the given id");
            }

            _context.Customers.Remove(existingCustomerData);
            _context.SaveChanges();
            return CustomerDTO.FromCustomer(existingCustomerData);
        }

        public void UpdateCustomerPicture(Guid customerId, byte[] picture)
        {
            var existingCustomerData = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (existingCustomerData == null)
            {
                throw new ArgumentException("no customer data found for the given id");
            }

            existingCustomerData.Picture = picture;
            _context.SaveChanges();
        }

        public byte[] GetCustomerPicture(Guid id)
        {
            var existingCustomerData = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (existingCustomerData == null)
            {
                throw new ArgumentException($"no customer data found for the given id {id}");
            }
            return existingCustomerData.Picture;
        }
    }
}
