Inventory Management API - README

Overview
This API provides a complete inventory management system with product, customer, and sales tracking capabilities, including JWT authentication, concurrent sales limiting, and discount/VAT calculations.

Technology Used: Dot net 9 Web API AND SQL SERVER

Table of Contents
- Setup Instructions
- Configuration
- Authentication
- API Endpoints
- Rate Limiting
- Sample Requests

SETUP INSTRUCTIONS

Prerequisites
- Visual Studio 2022
- .NET 9.0 SDK.
- SQL Server 2019+
- Redis (optional, for distributed rate limiting)

Installation
1. Clone the repository:
   git clone https://github.com/ahfcse/Inventory.API.git
   
   cd Inventory.API

3. Configure the database:
   - Update connection string in appsettings.json
   - Run migrations Or Execute attached(InventoryDB.sql) script manually.
     dotnet ef database update

4. Run the application:
   dotnet run

5. Access Swagger UI at:
  http://localhost:5245/swagger/index.html

CONFIGURATION

Environment Variables
Key                            Description                             Default
ConnectionStrings__DefaultConnection SQL Server connection string       Server=localhost;Database=InventoryDB;Trusted_Connection=True;
Jwt__Key                       JWT secret key                          your_secure_key_here
Jwt__Issuer                    JWT issuer                              InventoryAPI
Jwt__Audience                  JWT audience                            InventoryApp
Jwt__ExpiryInMinutes           Token expiration                        60

appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=Your SErver Name;Database=InventoryDB;User Id=Your User Id;Password=Your Password;TrustServerCertificate=true"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyHereAtLeast32CharactersLong",
    "Issuer": "InventoryAPI",
    "Audience": "InventoryAppClient",
    "ExpiryInMinutes": 1000
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}


AUTHENTICATION

Sample Login Credentials
Username    Password      Role
admin       admin123      Admin
manager     manager123    Manager
user        user123       User

Obtaining a Token
1. POST to /api/auth/login
   {
     "username": "admin",
     "password": "admin123"
   }

2. Use the returned token in subsequent requests:
   Authorization: Bearer <your_token> and Authorize it top right Click on Authorize button , Paste and Submit.

API ENDPOINTS

Products
Method  Endpoint                Description
GET     /api/Products/GetAllProducts          Get all products
GET     /api/Products/GetProduct/{id}      Get product by ID
POST    /api/Products/CreateProduct         Create new product
PUT     /api/Products/UpdateProduct/{id}      Update product
DELETE  /api/Products/DeleteProduct/{id}      Delete product

Customers
Method  Endpoint                Description
GET     /api/Customers/GetAllCustomers          Get all customers
GET     /api/Customers/GetAllCustomers     Get customer by ID
POST    /api/Customers/CreateCustomer          Create new customer
PUT     /api/Customers/UpdateCustomer/{id}     Update customer
DELETE  /api/Customers/DeleteCustomer/{id}     Delete customer

Sales
Method  Endpoint                Description
GET     /api/Sales/GetAllSales              Get all sales
GET     /api/Sales/GetSale/{id}         Get sale by ID
POST    /api/Sales/CreateSale              Create new sale
GET     /api/Sales/GetSalesReport/report       Get sales report

RATE LIMITING
- Global limit of 3 concurrent sales requests
- Exceeding limit returns HTTP 429 (Too Many Requests)
- Distributed rate limiting available when Redis is configured

SAMPLE REQUESTS

1. Login
curl -X POST "https://localhost:5245/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "admin123"
  }'

2. Get Current User
curl -X GET "https://localhost:5245/api/auth/me" \
  -H "Authorization: Bearer <your_token>"

3. Create Product
curl -X POST "http://localhost:5245/api/Products/CreateProduct" \
  -H "Authorization: Bearer <your_token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Wireless Mouse 2",
    "barcode": "MOUSE124",
    "price": 25.99,
    "stockQty": 50,
    "category": "Computer Accessories",
    "status": true
  }'

4. Create Sale with Discount/VAT
curl -X POST "http://localhost:5245/api/Sales/CreateSale" \
  -H "Authorization: Bearer <your_token>" \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": 1,
    "discountPercentage": 10,
    "vatPercentage": 8,
    "paidAmount": 100,
    "saleDetails": [
      {
        "productId": 1,
        "quantity": 2,
        "price": 24.99
      }
    ]
  }'

5. Get Sales Report
curl -X GET "[https://localhost:5001/api/sales/report?fromDate=2023-01-01&toDate=2023-12-31](http://localhost:5245/api/Sales/GetSalesReport/report?fromDate=2025-07-12&toDate=2025-07-15')" \
  -H "Authorization: Bearer <your_token>"

6. Test Concurrent Sales Limiter
curl -X POST "http://localhost:5245/api/Sales/CreateSale" \
  -H "Authorization: Bearer <your_token>" \
  -H "Content-Type: application/json" \
  -d '{
    "customerId": 1,
    "saleDetails": [
      {
        "productId": 1,
        "quantity": 1,
        "price": 24.99
      }
    ]
  }'

NOTES
1. Replace <your_token> with the actual JWT token received from the login endpoint
2. Dates should be in ISO 8601 format (YYYY-MM-DD)
3. For testing concurrent sales limit, use tools like Postman Runner or parallel curl commands
4. All monetary values should be sent as numbers (not strings)
5. The API will return 429 Too Many Requests when exceeding the 3 concurrent sales limit
