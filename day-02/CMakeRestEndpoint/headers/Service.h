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
	int next_id = 4;
public:
	CustomerService();
	~CustomerService();

	vector<Customer> get_all_customers();
	Customer get_customer_by_id(int id);
	Customer add_new_customer(Customer customer);
	void update_customer(Customer customer);
	Customer delete_customer(int id);
};