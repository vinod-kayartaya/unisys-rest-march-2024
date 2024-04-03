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

void json_resp(crow::response& resp, json data)
{
	resp.set_header("Content-Type", "application/json");
	resp.write(data.dump());
	resp.end();
}

void ok(crow::response& resp, json data)
{
	resp.code = 200;
	json_resp(resp, data);
}

void not_found(crow::response& resp, json data)
{
	resp.code = 404;
	json_resp(resp, data);
}

void created(crow::response& resp, json data)
{
	resp.code = 201;
	json_resp(resp, data);
}

void bad_request(crow::response& resp, json data)
{
	resp.code = 400;
	json_resp(resp, data);
}

void not_acceptable(crow::response& resp)
{
	resp.code = 406;
	json_resp(resp, { {"message", "we work only with application/json"} });
}

Customer json_to_customer(json data)
{
	Customer c;
	c.id = data.value("id", 0);
	c.name = data.value("name", "");
	c.city = data.value("city", "");
	c.email = data.value("email", "");
	c.phone = data.value("phone", "");

	return c;
}