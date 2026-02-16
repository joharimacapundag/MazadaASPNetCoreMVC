Mazada - E-Commerce Website (ASP.NET Core MVC + SQLite)
Mazada is an e-commerce website I built as my final project, inspired by Lazada. My goal was to create a functional online marketplace where users can register, log in, become sellers, create products, manage their shop, and also add items to a cart. Even though I was not able to finish everything I originally planned like a complete checkout system, adding multiple product images, or creating a full admin panel, I am still happy with what I achieved and how much I learned while building it.
One big learning experience for me was the database design. At the beginning, I created my own SQLite helper class and wrote raw SQL queries manually. This was fine for very simple things, but as the project grew, it became harder to maintain. I often mistyped queries or forgot column names, and it resulted in too much trial and error. So I switched to using DbContext from ASP.NET Core along with LINQ. This was a huge improvement. LINQ is much stricter and cleaner than raw SQL, which helped me avoid mistakes and made everything easier to update. I still use SQLite, just now with Entity Framework Core managing the models and tables.
My project has a lot of models because they represent the structure of my database tables like users, products, shops, categories, cart items, and more. But I also created ViewModels whenever a page only needed certain fields. Sending too much data to the frontend can be risky, so ViewModels allowed me to control what gets exposed and keep things cleaner and safer.
Most of my controllers return JSON responses like Ok() and BadRequest(), especially for pages that update dynamically through JavaScript. Some controllers return partial views to avoid full page reloads and make the user experience smoother. This approach makes the website feel more modern.
On the frontend side, I use a mix of jQuery and plain JavaScript. For handling forms with images, I use fetch() instead of full AJAX because it feels simpler and more natural to me.
For features, Mazada includes user login, registration, becoming a seller, adding products, editing products, viewing your shop, adding items to the cart, and removing them. The cart works for both logged in users and guests. I originally wanted to make an admin panel to manage featured products, but I did not finish it, so I ended up adding featured products directly inside the SQLite database using DB Browser.
One part of the project I am proud of is how I handle JavaScript error and success messages. Because my controllers return JSON, I can take specific values from those responses and update the UI instantly. For example, when the user logs out, a status modal appears without reloading the page. For forms, each field has its own error container so users immediately see messages like "Password field is empty!".
Another important thing I used a lot in this project is asynchronous programming (async and await). Almost all of my database operations use async versions like ToListAsync, FindAsync, and SaveChangesAsync. This helped keep the application responsive, especially during tasks that need to fetch or update data. I also used async JavaScript functions with fetch so the frontend can send requests and update the UI smoothly without blocking anything. Using async on both backend and frontend made everything feel faster and more responsive overall.
Another design decision I made was using bcrypt for password hashing instead of ASP.NET Core Identity. Identity is powerful, but I personally found it more complicated than what I needed. bcrypt was easier and simpler to integrate, so I went with that method.
Overall, I learned a lot from building Mazada. I learned how to better structure controllers, organize my models vs view models, write cleaner JavaScript for handling JSON responses, and manage a database using Entity Framework Core. Even though I did not finish every planned feature, the core functionality is solid, and the structure gives me room to expand the project in the future. Writing this README made me realize how much effort actually went into this project, and I am proud of the learning journey.

Project File Structure
Controllers/
 This folder contains all the logic that responds to user actions. Each controller handles a specific feature of the site. For example:
LoginController.cs manages logging in and logging out


RegisterController.cs handles user registration


ProductController.cs handles displaying products and adding new ones


CartController.cs manages adding and removing items in the cart
 Other controllers follow the same structure depending on the feature like search, shop pages, product details, etc. Most actions are asynchronous and return JSON or partial views for dynamic UI updates.


Models/
 These represent the database tables like User, Product, Shop, Category, CartItem, and so on. Entity Framework Core uses these model classes to build and manage the SQLite database.
ViewModels/
 These are simplified models that contain only the data needed for specific pages. This prevents exposing unnecessary properties to the frontend and improves security.
Views/
 The .cshtml Razor pages used to render the UI. Several pages use partial views to update content without a full page reload.
wwwroot/
 Contains JavaScript, CSS, and static files. The JavaScript here handles asynchronous fetch requests, processes JSON responses, shows error or success messages, and updates the DOM dynamically.
Data/MvcAppDbContext.cs
 This file configures the SQLite database using Entity Framework Core. It contains all the DbSet<> properties for each model. It replaces the manual SQLite helper I used originally, making the project much easier to maintain and reducing errors from manually written SQL. It also supports async database operations to keep everything responsive.
