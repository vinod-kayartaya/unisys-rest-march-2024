#pragma once

#include <crow.h>



struct CORS_Middleware
{
	struct context{};

	// called automaticall by crow before the control is passed to the handlers
	void before_handle(crow::request& req, crow::response& resp, context& ctx)
	{
		resp.set_header("Access-Control-Allow-Origin", "*");
		resp.set_header("Access-Control-Allow-Methods", "GET, POST, PUT, PATCH, DELETE, OPTIONS");
		resp.set_header("Access-Control-Allow-Headers", "*");
		// optional, end the response with these headers to any OPTIONS request
		if (req.method == crow::HTTPMethod::Options)
		{
			resp.end();
		}
	}

	void after_handle(crow::request& req, crow::response& resp, context& ctx) { }
};