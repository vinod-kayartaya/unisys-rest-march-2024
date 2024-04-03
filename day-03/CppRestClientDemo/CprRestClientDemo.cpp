#include <iostream>
#include <cpr/cpr.h>
#include <nlohmann/json.hpp>


using namespace std;
using json = nlohmann::json;

struct Contact
{
	string id;
	string firstname;
	string lastname;
	string gender;
	string email;
	string phone;
	string address;
	string city;
	string state;
	string pincode;
	string country;
	string picture;

	void print()
	{
		cout << "Contact details: " << endl;
		cout << "id: " << id << endl;
		cout << "firstname: " << firstname << endl;
		cout << "lastname: " << lastname << endl;
		cout << "gender: " << gender << endl;
		cout << "email: " << email << endl;
		cout << "phone: " << phone << endl;
		cout << "address: " << address << endl;
		cout << "city: " << city << endl;
		cout << "state: " << state << endl;
		cout << "pincode: " << pincode << endl;
		cout << "country: " << country << endl;
		cout << "picture: " << picture << endl;
	}
};


struct Contacts
{
	vector<Contact> contacts;
};


void from_json(const json& j, Contact& c)
{
	j.at("id").get_to(c.id);
	j.at("firstname").get_to(c.firstname);
	j.at("lastname").get_to(c.lastname);
	j.at("gender").get_to(c.gender);
	j.at("email").get_to(c.email);
	j.at("phone").get_to(c.phone);
	j.at("address").get_to(c.address);
	j.at("city").get_to(c.city);
	j.at("state").get_to(c.state);
	j.at("pincode").get_to(c.pincode);
	j.at("country").get_to(c.country);
	j.at("picture").get_to(c.picture);
}

void from_json(const json& j, Contacts& data)
{
	j.at("contacts").get_to(data.contacts);
}


const string base_url = "http://3.106.228.45:8080/api/contacts/";


void get_all_contacts()
{
	cpr::Response resp = cpr::Get(cpr::Url{ base_url });
	if (resp.status_code == 200)
	{
		cout << "Got the response successfully" << endl << endl;
		// cout << resp.text << endl;

		json json_body = json::parse(resp.text);
		json contacts = json_body["contacts"];

		for (const auto& c : contacts)
		{
			cout << (c.value("gender", "") == "Male" ? "Mr." : "Ms.")
				<< c.value("firstname", "") 
				<< " " 
				<< c.value("lastname", "") 
				<< endl;
		}
	}
	else
	{
		cout << "Something went wrong!" << endl;
		cout << resp.reason << endl;
	}
}

void get_contact_by_id(string id, string mime_type)
{
	cpr::Header headers = {
		{"Accept", mime_type}
	};

	cpr::Response resp = cpr::Get(cpr::Url{ base_url + id }, headers);
	if (resp.status_code == 404)
	{
		cout << "No customer data found for id: " << id << endl;
	}
	else if (resp.status_code == 406) 
	{
		cout << "Requested representation is not available" << endl;
	}
	else if(resp.status_code==200)
	{ 
		auto content_type = resp.header["Content-Type"];
		if (content_type == "application/json")
		{
			json json_body = json::parse(resp.text);
			cout << json_body.dump(4) << endl;
			cout << "----------------------------------" << endl;
			Contact contact = json_body.get<Contact>(); // json_body and a new Contact object will be passed to the from_json(..) function
			contact.print();
		}
		else
		{
			cout << resp.text << endl;
		}
	}
	else
	{
		cout << "response code is " << resp.status_code << endl;
	}

}


void get_contacts_from_state(string state)
{
	cpr::Parameters params = {
		{"state", state}
	};
	
	cpr::Header headers = {
		{"Accept", "application/json"}
	};

	cpr::Response resp = cpr::Get(cpr::Url{ base_url }, params, headers);
	json json_body = json::parse(resp.text);
	auto data = json_body.get<Contacts>();

	cout << "got " << data.contacts.size() << " contacts from the state of " << state << endl;
	for (auto c : data.contacts)
	{
		cout << c.firstname << " " << c.lastname << " (" << c.city << ")" << endl;
	}
}

void add_new_customer_as_json()
{
	const json contact = {
		{"firstname", "Vinod"},
		{"lastname", "Kumar"},
		{"email", "vinod@vinod.co"},
		{"phone", "9731424784"}
	};
	cpr::Header headers = {
		{"Content-Type", "application/json"}
	};

	cpr::Response resp = cpr::Post(cpr::Url{ base_url }, cpr::Body{ contact.dump() }, headers);

	json json_body = json::parse(resp.text);

	if (resp.status_code == 201)
	{
		cout << "new customer created" << endl;
	}
	else
	{
		cout << "Something wrong" << endl;
		cout << "status code is " << resp.status_code << endl;
	}

	cout << json_body.dump(4) << endl;
}

void add_new_customer_as_xml()
{
	string contact = "<?xml version=\"1.0\"?>";

	contact += "<contact>";
	contact += "<firstname>Rajesh</firstname>";
	contact += "<email>rajesh@xmpl.com</email>";
	contact += "<phone>7896547651</phone>";
	contact += "</contact>";

	cpr::Header headers = {
		{"Content-Type", "application/xml"},
		{"Accept", "application/json"}
	};

	cpr::Response resp = cpr::Post(cpr::Url{ base_url }, cpr::Body{ contact }, headers);

	json json_body = json::parse(resp.text);

	if (resp.status_code == 201)
	{
		cout << "new customer created" << endl;
	}
	else
	{
		cout << "Something wrong" << endl;
		cout << "status code is " << resp.status_code << endl;
	}

	cout << json_body.dump(4) << endl;
}

void update_contact()
{
	const string id = "349d15ec-a811-42f3-80ec-82457dd0085e";

	// get the contact for the given id
	// make few changes in the contact that received
	// send a put request with the modified content

	cpr::Response resp = cpr::Get(cpr::Url{ base_url + id });
	if (resp.status_code == 200)
	{
		cpr::Header headers = {
			{"Content-Type", "application/json"}
		};

		json c = json::parse(resp.text);
		c["city"] = "Bangalore";
		c["state"] = "Karnataka";
		c["country"] = "India";
		resp = cpr::Put(cpr::Url{ base_url + id }, cpr::Body{ c.dump() }, headers);
		if (resp.status_code == 200)
		{
			cout << "contact data updated." << endl;
			cout << json::parse(resp.text).dump(4) << endl;
			return;
		}
		cout << resp.text << endl;
		return;
	}

	cout << resp.text << endl;
}


void update_contact_address(string id, string address)
{
	cpr::Header headers = {
		{"Content-Type", "application/json"},
		{"Accept", "application/json"}
	};

	const json data = {
		{"address", address}
	};

	cpr::Response resp = cpr::Patch(cpr::Url{ base_url + id }, cpr::Body{ data.dump() }, headers);

	if (resp.status_code == 200)
	{
		cout << "data updated successfully" << endl
			<< json::parse(resp.text).dump(4) << endl;
	}
	else
	{
		cout << json::parse(resp.text).dump(4) << endl;
	}

}

int main()
{

	// get_all_contacts();
	// get_contact_by_id("bc34148f-323d-4048-aeb7-af4942fd4147", "application/json"); // existing id
	// get_contact_by_id("bc34148f-323d-4048-aeb7-af4942fd4147", "application/xml"); // existing id
	// get_contact_by_id("bc34148f-323d-4048-aeb7-af4942fd4147", "text/plain"); // existing id
	// get_contact_by_id("bc34148f-323d-4048-aeb7-af4942fd4147", "text/csv"); // existing id; not acceptable mime type
	// get_contact_by_id("bc34148f-323d-4048-aeb7-af4942fd4148"); // invalid id (valid uuid)
	// get_contacts_from_state("Texas");
	// get_contacts_from_state("Karnataka");

	// add_new_customer_as_json();
	// add_new_customer_as_xml();
	// update_contact();
	update_contact_address("349d15ec-a811-42f3-80ec-82457dd0085e", "1st cross, 1st main, ISRO lyt");


	cout << endl << "hit ENTER/RETURN to exit" << endl;

	cin.get();
	return 0;
}
