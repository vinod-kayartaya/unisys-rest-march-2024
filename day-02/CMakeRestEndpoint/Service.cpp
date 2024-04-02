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
	throw invalid_argument("no customer found for id " + id);
}