#pragma once

#include <nlohmann/json.hpp>
#include "Service.h"
#include <crow.h>

using json = nlohmann::json;

json customer_to_json(Customer customer);
json customers_to_json(vector<Customer> customers);
Customer json_to_customer(json data);

void ok(crow::response& resp, json data);
void not_found(crow::response& resp, json data);
void created(crow::response& resp, json data);
void bad_request(crow::response& resp, json data);