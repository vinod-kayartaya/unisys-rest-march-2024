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
		json data = { {"message", "no customer found for id " + id} };
		not_found(resp, data);
	}

}