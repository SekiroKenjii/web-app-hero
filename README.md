# APS.NET Web API Template.

I am excited to announce the availability of a new template for .NET 8 Web API system. This template is based on the [Microsoft ASP.NET Core Web API template](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-8.0), but has been modified to implement [Clean architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture) and all the best stuff of .NET ;)

## About The Project:

WebAppHero is a Clean Architecture Solution Template for APS.NET Web API 8.0 that fully adheres to the C# best practices and [Microsoft Best practices in cloud applications article](https://learn.microsoft.com/en-us/azure/architecture/best-practices/index-best-practices) such as SOLID principles, [Repository and Unit of Work Patterns](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#the-repository-and-unit-of-work-patterns), [API design](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design), [API implementation](https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-implementation), [Background jobs](https://learn.microsoft.com/en-us/azure/architecture/best-practices/background-jobs), [Caching](https://learn.microsoft.com/en-us/azure/architecture/best-practices/caching), [Transactional outbox Pattern](https://microservices.io/patterns/data/transactional-outbox.html)...

This project will make building your ASP.NET Web API project a lot easier than you anticipate. WebAppHero is meant to be an enterprise-grade, free, completely open source template. So if you like this template, consider supporting me with a cup of coffee? Let's get started.

### Tech Stack:

- APS.NET Web API 8.0
- [Entity Framework Core 8.0](https://docs.microsoft.com/en-us/ef/core/)
- Minimal API
- Serilog + Seq
- Docker container
- Redis
- RabbitMQ
- MongoDB/Microsoft SQL Server

# Getting Started

1. Install the latest [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Install the latest DOTNET & EF CLI Tools by using this command `dotnet tool install --global dotnet-ef` 
3. Install the latest version of Visual Studio or JetBrains Rider or Visual Studio Code
4. Install Docker on Windows via `https://docs.docker.com/docker-for-windows/install/`
5. Open Terminal and clone this project `git clone https://github.com/SekiroKenjii/web-app-hero.git`
6. Go to the project folder and run `dotnet restore` to restore all nuget packages
7. Open project solution with your favorite IDE/Editor and let's start coding

## Run the project

1. Pull Seq docker image and run a container for Seq log endpoint with the following command:
    ```bash
    docker run -d --restart unless-stopped --name seq-local -e ACCEPT_EULA=Y -p 8083:80 datalust/seq:latest
    ```
- Note - By default Seq has been configured with port `8083` in the `appsettings.json` file. You could also change the ports to your desired (make sure that port are free)
2. Now navigate back to the root of the project and run the following commands:
    ```bash
    cd .\src\WebAppHero.API\
    dotnet run watch
    ```
3. 5000 & 7001 are the ports setup to run WebAppHero, so make sure that these ports are free. You could also change the ports in the `src\WebAppHero.API\Properties\launchSettings.json` file

## Getting Started with Docker in Windows

1. Open up Windows Terminal and run the following commands:
    ```bash
    cd ~
    dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p YOUR_SECURE_PASSWORD
    dotnet dev-certs https --trust
    ```
- Note - Make sure that you use the same password that has been configured in the `docker-compose.yml` file. By default, `YOUR_SECURE_PASSWORD` is configured.
1. 5000 & 7001 are the ports setup to run WebAppHero and 8080 is set to run Seq on Docker, so make sure that these ports are free. You could also change the ports in the `docker-compose.yml` and `src\WebAppHero.API\Dockerfile` files
2. Now navigate back to the root of the project and run the following command:
    ```bash
    docker-compose up --build -d
    ```

# Features

Update soon...

## Contributing

Any contributions you make are **greatly appreciated**

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/YourFeature`)
3. Commit your Changes (`git commit -m 'Add some features...'`)
4. Push to the Branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

## Support

If this project has helped you learn something new or helped you in your career. Please consider supporting it.

-   Leave a star! :star:
-   Recommend this awesome project to your colleagues.
-   Do consider endorsing me on LinkedIn - [Connect via LinkedIn](https://www.linkedin.com/in/thuong-vo-dev-020395213/)
-   Or, If you want to support this project on the long run, consider donating me via [MoMo](https://www.momo.vn/): 0375274267
