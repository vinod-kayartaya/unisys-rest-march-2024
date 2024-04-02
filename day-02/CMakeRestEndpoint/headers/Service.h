#pragma once

#include <iostream>
#include <vector>

using namespace std;

struct Customer
{
	int id;
	string name;
	string city;
	string email;
	string phone;
};

class CustomerService
{
private:
	vector<Customer> customers;
public:
	CustomerService();
	~CustomerService();

	vector<Customer> get_all_customers();
	Customer get_customer_by_id(int id);
};