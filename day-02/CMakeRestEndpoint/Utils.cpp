#include "Utils.h"

json customer_to_json(Customer customer)
{
	return {
		{"id", customer.id},
		{"name", customer.name},
		{"city", customer.city},
		{"email", customer.email},
		{"phone", customer.phone},
	};
}

json customers_to_json(vector<Customer> customers)
{
	json customers_json;
	for (const auto& c : customers)
	{
		customers_json.push_back(customer_to_json(c));
	}
	return customers_json;
}

void ok(crow::response& resp, json data)
{
	resp.code = 200;
	resp.set_header("Content-Type", "application/json");
	resp.write(data.dump());
	resp.end();
}

void not_found(crow::response& resp, json& data)
{
	resp.code = 404;
	resp.set_header("Content-Type", "application/json");
	resp.write(data.dump());
	resp.end();
}