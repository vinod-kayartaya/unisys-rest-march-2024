# Day 1 notes

REST - Representational state transfer

Transfer (exchange) of state (information/data) in different representations (xml/json/csv/text/html/yaml/..)

late 90's
W3C -> XML and XHTML

```xml
<laptop slno="282821">
    <make>Apple</make>
    <model>Macbook pro</model>
</laptop>
```

SOAP web services
-> HTTP protocol for carrying data
-> XML (standardized by SOAP)

```xml
<envelope>
    <header>
    </header>
    <body>
        <getFlights>
            <from>BLR</from>
            <to>MUM</to>
            <date>2024-04-04</date>
            ...
            ..
        </getFlights>
    </body>
</envelope>
```

JSON examples

```json
// empty object
{}

// string object
"vinod kumar"

// object with a key/value

{
    "firstname": "Vinod",
    "lastname": "Kumar"
}

// object with another object for a key

{
    "name": "Vinod Kumar",
    "contact": {
        "emails": ["vinod@vinod.co", "vinod@knowledgeworksindia.com"]
    },
    "married": true
}
```


Key features/ terms:

1. Resource --> made up of states
    - Noun
    - represents a pool of data
    - flights, employees, products, brands, categories, orders, customers

2. Uniform resource identifier (URI)
    - http://example.com/api/products --> resource (source of states)
    - http://example.com/products --> source of HTML page with product information

3. HTTP methods (verbs)
    - GET       --> get one or more state/s from the resource
    - POST      --> creates one (mostly) or more state in the resource
    - PUT       --> updates the one retrieved earlier with a new state
    - PATCH     --> updates a state with few properties
    - DELETE    --> removes a state in the resource 

4. Representations --> XML/JSON (most used today)/CSV/plain text/ html/ etc

6 Constraints

1. Client/server
2. Stateless
3. Uniform interface
    - identification of resource 
        - http://example.com/api/products       [ GET, POST ]
        - http://example.com/api/products/45    [ GET, PUT, PATCH, DELETE]
        - Any format (JSON/XML/...)
4. Layered system
5. Cacheability
6. Code on demand (Optional)


MIME

- Multipart Internet Mail Extension
- like a file extension for the OS
    - text/plain
    - application/json
    - application/xml
    - image/jpeg
    - application/pdf
    - text/html

HTTP response status codes:

- (100-199) Informational response
- (200-299) Successful response
- (300-399) Redirectional response
- (400-499) Client error
- (500-599) Server error

200 - OK
201 - Created
    - in the context of REST, should also have a "Location" response header pointing to the newly created resource state
204 - No content

400 - Bad request
401 - Unauthorized
403 - Forbidden
404 - Not found
405 - Method not allowed
406 - Not acceptable (corresponds to the `Accept` header)

500 - Internal server error


HATEOAS

Hypermedia As The Engine Of Application State

```http
POST {{base_url}}/api/contacts
Content-Type: application/json
Accept: application/json

{
    "firstname": "Vinod",
    "email": "vinod@vinod.co",
    "phone": "9731424784"
}
```

Expected response:

```http
HTTP/1.1 201 
Vary: Origin, Access-Control-Request-Method, Access-Control-Request-Headers
Content-Type: application/json
Transfer-Encoding: chunked
Location: http://13.210.249.205:8080/api/contacts/c8a3aa43-b99d-4901-bbb3-13ed799d3864
Date: Fri, 29 Mar 2024 11:01:40 GMT
Connection: close
```

or

```http
HTTP/1.1 201 
Vary: Origin, Access-Control-Request-Method, Access-Control-Request-Headers
Content-Type: application/json
Transfer-Encoding: chunked
Date: Fri, 29 Mar 2024 11:01:40 GMT
Connection: close

{
    "status": "success",
    "resource": "http://13.210.249.205:8080/api/contacts/c8a3aa43-b99d-4901-bbb3-13ed799d3864"
}
```

Another example:

```http
GET /api/departments/33
Host: example.com
Acception: application/json
```

expected response:

```http
HTTP/1.1 200 ok
Content-Type: application/json

{
    "department": {
        "id": 33,
        "name": "administration",
        "location": "Mumbai"
    },
    {
        "employees": "http://example.com/api/departments/33/employees",
        "headOfDepartment": "http://example.com/api/departments/33/hod",
    }
}
```

Features/advantages:

1. dynamic navigation
2. reduced coupling
3. increased flexibility
4. self descriptivie apis