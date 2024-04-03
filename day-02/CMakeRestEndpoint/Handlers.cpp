#include "Handlers.h"
#include "Utils.h"

Handler::Handler() {}
Handler::~Handler() {}

void Handler::handle_get_all(crow::request& req, crow::response& resp)
{
	auto customers = service.get_all_customers();
	auto data = customers_to_json(customers);
	ok(resp, data);
}

void Handler::handle_get_one(crow::request& req, crow::response& resp, int id)
{
	try
	{
		auto customer = service.get_customer_by_id(id);
		auto data = customer_to_json(customer);
		ok(resp, data);
	}
	catch (const exception& e)
	{
		json data = { {"message", e.what()}};
		not_found(resp, data);
	}

}

void Handler::handle_post(crow::request& req, crow::response& resp)
{
	json json_customer;
	try
	{
		json_customer = json::parse(req.body);
	}
	catch (const exception& e)
	{
		bad_request(resp, { {"message", e.what()} });
		return;
	}

	auto customer = json_to_customer(json_customer);
	try
	{
		customer = service.add_new_customer(customer);
		created(resp, customer_to_json(customer));
	}
	catch (const exception& e)
	{
		bad_request(resp, { {"message", e.what()} });
	}
}

void Handler::handle_put(crow::request& req, crow::response& resp, int id)
{

	try
	{
		service.get_customer_by_id(id);
	}
	catch (const exception& e)
	{
		not_found(resp, { {"message", e.what()} });
		return;
	}

	json json_customer;
	try
	{
		json_customer = json::parse(req.body);
	}
	catch (const exception& e)
	{
		bad_request(resp, { {"message", e.what()} });
		return;
	}

	auto customer = json_to_customer(json_customer);
	customer.id = id;

	try
	{
		service.update_customer(customer);
		ok(resp, customer_to_json(customer));
	}
	catch (const exception& e)
	{
		bad_request(resp, { {"message", e.what()} });
	}
}


void Handler::handle_patch(crow::request& req, crow::response& resp, int id)
{
	Customer c;
	try
	{
		c = service.get_customer_by_id(id);
	}
	catch (const exception& e)
	{
		not_found(resp, { {"message", e.what()} });
		return;
	}

	json json_customer;
	try
	{
		json_customer = json::parse(req.body);
	}
	catch (const exception& e)
	{
		bad_request(resp, { {"message", e.what()} });
		return;
	}

	Customer customer;
	customer.id = id;
	customer.name = json_customer.value("name", c.name);
	customer.city = json_customer.value("city", c.city);
	customer.phone = json_customer.value("phone", c.phone);
	customer.email = json_customer.value("email", c.email);

	try
	{
		service.update_customer(customer);
		ok(resp, customer_to_json(customer));
	}
	catch (const exception& e)
	{
		bad_request(resp, { {"message", e.what()} });
	}
}

void Handler::handle_delete(crow::request& req, crow::response& resp, int id)
{
	try
	{
		auto deleted_customer = service.delete_customer(id);
		ok(resp, customer_to_json(deleted_customer));
	}
	catch(const exception & e)
	{
		not_found(resp, { {"message", e.what()} });
	}
}