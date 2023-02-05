Types of Authentication
1- Basic Authentication
2- Jwt Authentication
3- Role Based Authentication

 Basic Authentication
1- Basic authentication is a simple authentication scheme built into the http protocol

2- The client send http requests with the Authorizatiom header containing word "Basic", a space character, and a "username:password" string encided in Base64

Basic AuthenticationProcess
1- Enable Authentication adding (app.UseAuthentication();) in Program.cs file
2- 