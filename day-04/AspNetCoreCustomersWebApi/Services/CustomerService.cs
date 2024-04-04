using AspNetCoreCustomersWebApi.Data;
using AspNetCoreCustomersWebApi.Models;

namespace AspNetCoreCustomersWebApi.Services
{
    public class CustomerService
    {
        private readonly IList<Customer> _customers;
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context) { 
            _customers = new List<Customer>();
            _context = context;
        }
   
        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(Guid id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id);
        }

        public void AddCustomer(Customer customer)
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
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
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

        public Customer DeleteCustomer(Guid customerId) {
            var existingCustomerData = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            if (existingCustomerData == null)
            {
                throw new ArgumentException("no customer data found for the given id");
            }

            _context.Customers.Remove(existingCustomerData);
            _context.SaveChanges();
            return existingCustomerData;
        }
    }
}
