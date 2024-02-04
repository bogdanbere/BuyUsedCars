
# BuyUsedCars

This is a used cars website project for the postgraduate program exam.


## Tech Stack

**Client:** Bootstrap, Razor Pages

**Server:** Asp.Net Mvc, Entity Framework, Sqlite, Cloudinary


## Run Locally

Clone the project

```bash
  gh repo clone bogdanbere/BuyUsedCars
```

Go to the project directory

```bash
  cd BuyUsedCars
```
Initialize secrets
```bash
dotnet user-secrets init
```

Open secrets.json and add the following structure
```bash
{
    "CloudinarySettings": {
        "CloudName": "your name",
        "ApiKey": "your api key",
        "ApiSecret": "your apisecret"
    },
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=Cars.db"
      }
}
```

Install dependencies

```bash
  dotnet build
```

Start the server

```bash
  dotnet watch run
```

