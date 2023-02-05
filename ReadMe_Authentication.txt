Types of Authentication
1- Basic Authentication
2- Jwt Authentication
3- Role Based Authentication

 Basic Authentication
1- Basic authentication is a simple authentication scheme built into the http protocol

2- The client send http requests with the Authorizatiom header containing word "Basic", a space character, and a "username:password" string encided in Base64

Basic AuthenticationProcess
1- Enable Authentication adding (app.UseAuthentication();) in Program.cs file


Jwt Authentication
1, JSON Web Tokens(Commony known as JWT) is an open standard pass data between client and server
and enables you to transmit data back and forth between the server and the consumers in a secure manner.


Create Token
1-Initiate handler using class "JwtSecurityHandler"
2- create token using CreateToken method it expects as token descriptor
3- Initiate token descriptors here just define Subjects ,Expire time,Signing Credentials
4- write and return token

Validate Token 
1-Enable Jwt Authentication using AddAuthentication & add value for DefaultAuthenticationScheme,Challenging scheme
2- Add AddJwtBeare here set values for Require https metadata,SaveTokens & Validaationparameter
ValidateIsssuerSigningKey=true
IssuerSigningKey=new
SymmetricSecurityKey(Encoding.TF8.GetBytes(authkey)),
ValidateIssuer=false,
ValidateAudience=false,
ClockSkew=TimeSpan.Zero

Steps
1- Enable Authentication adding "useAuthentication"
2- Create Method for generate token
3- Register the security key in the appsettings
4- Access and inject the key into the controller
5- Validate user crendentials based on input request




