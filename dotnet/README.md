# Checkout single subscription

An [.NET Core](https://dotnet.microsoft.com/download/dotnet-core) implementation

## Requirements

* .NET Core
* (../../README.md)

## How to run

1. First Run the application as making the server project as startup

It will call the Authorize api with amount set as 100 and currency set as USD.
Inaddition it will run this url too http://localhost:4242/checkout.html.

1. Please click Paynow button and enter the sample card details:
                  card number: 4242424242424242  expriydate: any future date CCV: any 3 digit Pincode: any 5 digit no.
2. Now the payment will be authorized and login to the stripe dashboard and copy the Payment_id Foreg: pi_1IvccdH4DR7BOnAWLJuW78y8
3. Open postman http://localhost:4242/api/paymentgateway/capture 
   1. Enter the follwoing JWT Token as Bearer Token(eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJyb2xlIjoiYWRtaW4iLCJuYmYiOjE2MjIwMjM4MDAsImV4cCI6MTYyMjYyODYwMCwiaWF0IjoxNjIyMDIzODAwfQ.kNEIQGFSWJDDAAJANxXrLdQ6BVPHbHgViYfGJ8UT9MI) which was generated using the Usercontroller and expires in 48 hours 
   2. Then in body enter  amount and payment_id (which was copied from Stripe dashboard) and this will call the Capture Api.
   3. After the Capture api is called the status for the corresponding transaction_id will be updated as completed.

4. Ways to generate the JWT Token:
	1. Call the post method http://localhost:4242/users/authenticate and then in body pass
	Username as 'test' and  Password as 'test'. Then the token will be send as response which is valid for 48 hours.
5. Script for Transaction table:

CREATE TABLE [dbo].[Transaction](
	[id] [varchar](50) NOT NULL,
	[amount] [float] NULL,
	[transaction_id] [varchar](200) NULL,
	[created_date] [date] NULL,
	[type] [varchar](50) NULL,
	[status] [varchar](50) NULL,
	[currency] [nchar](10) NULL,
	[email] [nvarchar](50) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

The credentials for stripe dashboard login: aneez.rafeek123@gmail.com
	              pwd: Password@250521
stripe dashboard url : https://dashboard.stripe.com

To create a model based on existing database.
command :- Scaffold-DbContext "Server=144.91.97.73;Database=stripe;User Id=sa;Password=741#code;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities-force
* select Entit Project as default project while executing this command
```
