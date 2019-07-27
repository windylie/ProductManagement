# ProductManagement

## Project structure

- `ProductManagementApi`: Backend API, written using ASP.NET Core
- `ProductManagementApiTests : Integration and Unit Tests of ProductManagementApi, written using Nunit framework
- `product-management-client`: Frontend web, written using ReactJS

## Start project in local

- This project using in memory data store. It contains 2 data stores : ProductDataStore and UserDataStore
- Start backend API either through:
    + Visual Studio
    + or command line
    ```
    dotnet run --project ProductManagementApi
    ```
- Start frontend web
    ```
    cd product-management-client
    npm install
    npm start
    ```
  Note: frontend application assumes backend is started at port 44326
  
## Backend APIs

Backend offers 7 APIs:

Authorized APIs :
- `GET /api/products`
      + get all products
      + it accepts query string to do the filtering on description, model, and brand

- `GET /api/products/{productId}`
      + get product based on productId

- `POST /api/products`
      + create new product

- `PUT /api/products/{productId}`
      + update product with id {productId}

- `DELETE /api/products/{productId}`
      + delete product with id {productId}
      
AllowAnonymous APIs :
- `POST /api/users`
      + create new users
- `POST /api/users/authenticate`
      + authenticate user

## Authentication

Authentication is done with JWT (JSON Web Token) Authentication.
This method is used for its simplicity and popularity trait.

How it works :

- user will provide his credentials to the server
- if the credential is correct (user with given user name exists in UserDataStore and hashed password of given password matches hashed password from data store) then server will issue jwt token to user
- front end application stores retrieved JWT token in browser's local storage
- everytime front end application sends a request to any authorized url, `axios` request interceptor will check for availability of token from local storage and set `Authorization` header of that request if token is available

## Assumptions

- 2 products are deemed same when model and brand are same, this assumption will be used to validate duplication when creating a new product
      
