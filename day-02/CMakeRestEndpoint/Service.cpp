#include "Service.h"


CustomerService::CustomerService() 
{
	customers = {
		Customer{1, "Vinod", "Bangalore", "vinod@vinod.co", "9731424784"},
		Customer{2, "Shyam", "Bangalore", "shyam@xmpl.com", "9731424000"},
		Customer{3, "Murali", "Bangalore", "murali@xmpl.com", "9731424111"}
	};
}

CustomerService::~CustomerService() {}

vector<Customer> CustomerService::get_all_customers()
{
	return customers;
}

Customer CustomerService::get_customer_by_id(int id)
{
	auto& it = find_if(customers.begin(), customers.end(), [id](const auto& c) { return c.id == id; });
	if (it != customers.end())
	{
		return *it;
	}

	throw invalid_argument("no customer found for the given id");
}

Customer CustomerService::add_new_customer(Customer customer)
{
	if (customer.name.empty() || customer.email.empty() || customer.phone.empty())
	{
		throw invalid_argument("missing one or more of required fields - name/email/phone");
	}

	auto it = find_if(customers.begin(), customers.end(), [customer](Customer& c) {return c.email == customer.email; });
	if (it != customers.end())
	{
		throw invalid_argument("another customer with this email is already present");
	}

	it = find_if(customers.begin(), customers.end(), [customer](Customer& c) {return c.phone == customer.phone; });
	if (it != customers.end())
	{
		throw invalid_argument("another customer with this phone is already present");
	}

	customer.id = next_id++;
	customers.push_back(customer);
	return customer;
}


void CustomerService::update_customer(Customer customer)
{
	auto it = find_if(customers.begin(), customers.end(), [customer](Customer& c) { return c.id == customer.id; });

	if (it == customers.end()) {
		throw invalid_argument("customer with the given id was not found");
	}

	if (customer.name.empty() || customer.email.empty() || customer.phone.empty())
	{
		throw invalid_argument("missing one or more of required fields - name/email/phone");
	}

	auto email_duplicate = find_if(customers.begin(), customers.end(), [customer](Customer& c) { 
		return c.id != customer.id && c.email == customer.email;
		});

	if (email_duplicate != customers.end()) {
		throw invalid_argument("there is a different customer with the given email");
	}

	auto phone_duplicate = find_if(customers.begin(), customers.end(), [customer](Customer& c) {
		return c.id != customer.id && c.phone == customer.phone;
		});

	if (phone_duplicate != customers.end()) {
		throw invalid_argument("there is a different customer with the given phone");
	}

	*it = customer;
}