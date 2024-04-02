#pragma once

#include <nlohmann/json.hpp>
#include "Service.h"
#include <crow.h>

using json = nlohmann::json;

json customer_to_json(Customer customer);
json customers_to_json(vector<Customer> customers);

void ok(crow::response& resp, json data);
void not_found(crow::response& resp, json& data);