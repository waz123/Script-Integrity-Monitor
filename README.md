# Script Integrity Monitor

This repository contains a Blazor web application and a small Python API used to monitor JavaScript integrity on websites.

## Setup

1. Install [.NET](https://dotnet.microsoft.com/) 7 or later and Python 3.
2. Set the following environment variables:
   - `AWS_SES_ACCESS_KEY` – your AWS access key.
   - `AWS_SES_SECRET_KEY` – your AWS secret key.
   - `AWS_SES_SENDER` – default sender email address.
   - `DB_CONNECTION_STRING` – PostgreSQL connection string used by Entity Framework.
3. Optional configuration values can be set in `Security App  Blazor/appsettings.json`.

The Python API can be found in `Security App  Blazor/API`. Run it with `python Script_Checker.py`.

The Blazor application can be started with `dotnet run` in the `Security App  Blazor` directory.

