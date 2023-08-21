# Use-Wheels
Dot NET API Training project

**Description**
   To build APIs for second-hand cars selling site that displays various car models from different categories.
  
**Primary Actors involved**
1. End-Users [buy vehicles from site]
2. Admin users [add category and vehicle to site]

**Use-cases:**
1. User Registration API
      • There are two types of users – Admin and regular users. This registration portal is only for
      regular users of the application who wants to book cars on the platform.
      • Get details of users like first name, last name, date of birth, gender, email address and
      password
      • Registration should be successful only if the user is above 18 years old, return appropriate
      response when registration request is initiated.
2. Login API
      • Allow the user to login when the entered credentials are correct.
      • If Incorrect credentials are used – show appropriate error response with status code.
      • Allow the user to logout when appropriate API is triggered.
      • User should be logged out after 3hrs from login.
3. E-Comm APIs
      • Create CRUD APIs for Cars and Categories [Mandatory attributes can be cars pre-owner
      count, product image, RC number]. Feel free to include other attributes based on
      creativity/references.
      • Create some sample test data based on any used-cars website of your choice.
      • DB can be SQL or No-SQL DB
      • Build APIs that allow users to add cars to Wishlist. Add to wishlist should increase product
      view counter in database. If views are not available, show custom messages that attracts the
      user when hitting the API.
      • Build APIs to read cars from Wishlist.
      • Delete the wish list once the user logs out.
      • Users should not be able to consume the APIs without proper authentication & authorization.
4. Role-Based APIs
      • The normal user must only have ability to read the data (cars & categories) (GET)
      • Normal user can add vehicles to wish list.
      • The admin user must have ability to add/update/delete both cars and categories.
5. Common Scenarios
      • Same data should not be continuously retrieved from database.
      • Try to minimize the data consumption from the DB.
   
**Failure Scenarios:**
      1. If the Portal is down, display a user-friendly message.
      2. If there are no products/cars in the hop, display a user-friendly message.
      3. Handle exceptions wherever necessary

Note: Custom error messages to be displayed to the user during error flows will be chosen by the developer

Recommended Tech Stack:
- C#
- .NET 6 & above
- Mongo/embedded DB (H2 - SQL)
- Unit tests (services & controllers)
  
Expected Deliverables:
- GitHub (Private repo)
- Postman collection for the APIs built.
- Readme file for instructions to setup & test
- Db dump (If local SQL database is used) or JSON Document (If local Mongo is used)
