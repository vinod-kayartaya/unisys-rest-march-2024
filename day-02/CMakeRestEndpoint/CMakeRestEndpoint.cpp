﻿#include <iostream>
#include <crow.h>
#include "Handlers.h"
#include "CORS.h"
#include "JsonOnly.h"

using namespace std;

int main()
{

	// crow::SimpleApp app;
	crow::App<JsonOnlyMiddleWare, CORS_Middleware> app;
	app.loglevel(crow::LogLevel::Info);


	// http://localhost:18080/
	CROW_ROUTE(app, "/")([]() {
		return "Hello, wordl!";
	});

	Handler handler;

	// http://localhost:18080/api/customers
	CROW_ROUTE(app, "/api/customers").methods(crow::HTTPMethod::Get)
		([&handler](crow::request& req, crow::response& resp) {
			handler.handle_get_all(req, resp);
		});

	// http://localhost:18080/api/customers/3
	CROW_ROUTE(app, "/api/customers/<int>").methods(crow::HTTPMethod::Get)
		([&handler](crow::request& req, crow::response& resp, int id) {
		cout << "going to the handler method `handle_get_one`...";
			handler.handle_get_one(req, resp, id);
		});

	// http://localhost:18080/api/customers
	CROW_ROUTE(app, "/api/customers").methods("POST"_method) // .methods(crow::HTTPMethod::Post)
		([&handler](crow::request& req, crow::response& resp) {
			handler.handle_post(req, resp);
		});

	// http://localhost:18080/api/customers/3
	CROW_ROUTE(app, "/api/customers/<int>").methods(crow::HTTPMethod::Put)
		([&handler](crow::request& req, crow::response& resp, int id) {
			handler.handle_put(req, resp, id);
		});

	// http://localhost:18080/api/customers/3
	CROW_ROUTE(app, "/api/customers/<int>").methods(crow::HTTPMethod::Patch)
		([&handler](crow::request& req, crow::response& resp, int id) {
			handler.handle_patch(req, resp, id);
		});

	// http://localhost:18080/api/customers/3
	CROW_ROUTE(app, "/api/customers/<int>").methods(crow::HTTPMethod::Delete)
		([&handler](crow::request& req, crow::response& resp, int id) {
			handler.handle_delete(req, resp, id);
		});

	app.port(18080).multithreaded().run();

	cout << endl << "Hit RETURN to exit" << endl;
	cin.get();
	return 0;
}
