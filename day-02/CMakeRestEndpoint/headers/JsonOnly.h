#pragma once

#include <crow.h>
#include "Utils.h"

struct JsonOnlyMiddleWare
{
	struct context {};

	void before_handle(crow::request& req, crow::response& resp, context& ctx) 
	{
		const string value = req.get_header_value("Accept");
		if (value != "application/json")
		{
			not_acceptable(resp);
			return;
		}
	}

	void after_handle(crow::request& req, crow::response& resp, context& ctx) 
	{
		cout << "going back to the client with response code " << resp.code << endl;
	}
};