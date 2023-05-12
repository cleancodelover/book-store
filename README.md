# Book Store API Documentation

# Summary <br>
The project uses a layered architecture, divided into 3 tiers, the API layer (BookStore.API) which is where the endpoints are exposed and every other thing that has to do with clients, the Business Logic layer (BookStore.BLL), this is where the business logic, complex compuations are handled and lastly the Data Access Layer (BookStore.DAL) which is where the connection to the database is established, housing the models and everything that has to do with the database. Also, to keep things clean and tidy, I have both unit and integration testing in seperate assemplies.
Note: I used MSSQL so you must have it installed with MSSQL Management Studio. The API strictly follows the RESt API rules, using the appropriate verbs to represent exactly what they do.

I'm logging everything to files, it's a global logger, such that where ever an error occurs, it tracks it and logs it to a file. It's not good practice to log to db since they can easily become really large.

In the BookStore.API, there is a folder called "Logs", there you find all the logs sorted by date.

I utilize Dependency Injection, neatly abstracted things, modularize the application.

I Kept everything, module, simple and understandable. I utilize Microsoft Identity for authentication and jwt token for authorization.

# Follow me steadily let me explain.

## Step 1:<br>
 - Clone the project to your local machine
 - Open it with visual studio 2022 preferrably 
 - Make sure you have .NET 6 installed.
 - Make sure all dependencies are installed.

## Step 2:<br>
  - Launch your MSSQL Management Studio
  - Create a Datase and name it "BOOK_STORE"
  - Copy your database server name from MSSQL Management Studio Connection Dialogue Box.

## Step 3:<br>
  - Open the API layer of the project "BookStore.API"
  - Look down on the project file tree and locate "appsettings.json"
  - Expand the appsettings flyout to expose "appsettings.Development.json"
  - Click on "appsettings.Development.json" to open it in your VS Editor window.
  - Locate "BookStoreConnection", in the value section, replace everything after "Server=" and replace everything after the "=" up to before ";" with the server name you copied from MSSQL Management Studio.
  - Save and close the file.

## Step 4:<br>
 - Open "Package Manager Console" on Tools->Nuget Package Manager->Package Manager Console.
 - Wait for it to initialize.
 - Make sure that your "Default Project" is set to "BookStore.API"
 - On the console, run following command: <br><br>
   *  Update-Database
 - Wait for the migration to apply to the database.
 - When it shows you "Done" in the console.
 - Check your database in MSSQL Management Studio to be sure that your tables are all created.

## Step 5:<br>
  - To run test, run the following command:
    * dotnet test
  - Wait for it to run to completion. <br> NOTE: It will show you the number of tests passed, possibly 2 for Integration and Unit Tests on Book creation and book ordering for integration test.

# Step 6 - RUNNING THE PROJECT:<br>
  - Click on the Play button at the top bar of your visual studio with the name "BookStore.API".
  - Wait for it to load.
  - It will automatically open your default browser, and land you on the swagger documentation page. Thanks to swagger for the clean API documentation.

# Step 7 - To Sign Up
  - Thankfully, swagger usually sort his APIs so you will have to scroll down to "Users Section",
  - Expand the "Post" endpoint /users, the model is there with all the required field to signup.
  - The password must contain letters and numbers with special characters, upper and lower case. I can help you with one "Pa$$w00rd".
  - Fill the fields and click on "Execute", right in your browser, it will send the request to the API and return success message.

# Step 8 - To Sign-In
  - Scroll back to the top on the "Auth Section", and find Login,
  - Enter your email and password and click on "Execute"
  - You'll be logged in and a toke will be generated for you in the response you'll get from the server.
  - Copy the token and Look a little above the login endpoint, top to the right and find "Authorize" with a padlock on it,
  - Click it and paste the token you copied in teh popup and click "Authorize" the "close".
  - To see your details after after sign up because you'll need your userId, COPY IT. It's just "Ctrl C" for windows users and "Command C" for mac users.

# Step 9 - To Create a Book
  - Scroll to the "Books Section" and locate the enpoint "/books" with the verb "Post", expand it and fill in the parameters including your user name where user name is required and click on "Execute".
  - Repeat the book creation process for up to 3 books.
  - On the same section, you can find the endpint to view all books, book details by book id, view your own books that you have created by your user ID.

# Step 9 - To Create a Wallet
  - You need a wallet to be able to purchase a book. Scroll down to "Wallets Section".
  - On the "/wallets" endpoint with "Post" verb, expand it and and click "Try Out", 
  - Enter your "UserId", an amount "Balance" that you can be able to buy as much books as you can, it's free money, just enter any amount and click on "Execute".
  - You can also check your wallet balance, if you expand the first get method and enter your user id and click on "Execute"

# Step 10 - Add Books to your cart
  - Collect the IDs of the books you want to buy, there is no UI, let's just do it. Copy all the IDs and go to the "Cart Section"
  - Enter the IDs with the quantity of the books you want to purchase, in the "Items" field, it's an array of book objects, just forget the cartId and status for now, and click on "Execute" to save your cart.
  - Above it are endpoints to view your cart items, add items to cart, update cart, delete items and the rest.
  - Open the "/carts" endpoint with the verb "GET", enter your userid and "execute" to get your cartID, if you didn't copy it when you created the cart. Cuz you'll need it.

# Step 11 - To checkout
  - Scroll down to "Transactions Section" and locate "/checkout" enpoint with the verb "Post".
  - Enter your cart ID, type anything as your transaction reference, enter a narration and click "Execute",
  - You are done making a purchase.

# Step 12 - To View your transactions
  - On thesame transactions, use the "/transactions" endpoint with the "Get" verb, expand it and enter your user name and view all  your transactions.

# Step 13 - Review Books
  - Go down to "Reviews" and expand the endpoint "books/reviews" with the verb "Post", enter your book ID, your user ID, a description and a waited rating, from 0 to 5.


That's it.
Thank you for staying tuned.
