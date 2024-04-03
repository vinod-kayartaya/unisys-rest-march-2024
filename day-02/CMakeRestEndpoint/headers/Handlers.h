#pragma once

#include <crow.h>
#include "Service.h";
#include <nlohmann/json.hpp>

class Handler 
{
private:
	CustomerService service;
public:
	Handler();
	~Handler();

	void handle_get_all(crow::request& req, crow::response& resp);
	void handle_get_one(crow::request& req, crow::response& resp, int id);
	void handle_post(crow::request& req, crow::response& resp);
	void handle_put(crow::request& req, crow::response& resp, int id);
	void handle_patch(crow::request& req, crow::response& resp, int id);
	void handle_delete(crow::request& req, crow::response& resp, int id);
};