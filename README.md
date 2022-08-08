# financial-chat

App chat using .Net and React with a bot to get stock quotes

## Features

Allow registered users to log in and talk with other users in a chatroom.

Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code

Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
stock_code)

The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ.

The message will be a stock quote
using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot.

Have the chat messages ordered by their timestamps and show only the last 50
messages

Have more than one chatroom.

Use .NET identity for users authentication

# How to run

## For SqlServer and RabbitMq

With a docker installed

In main folder

run docker-compose up

## For frontend

With npm installed

In frontend folder

Run npm install after npm start

## For backend

With .net 6.0 installed

In backend\FinancialChat folder

dotnet restore

dotnet build

dotnet run
