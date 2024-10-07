## Prerequities

* Asp.Net Aspire
* Asp.Net Core 8
* Docker Desktop


## Running in Debug Mode

* Open Docker Desktop
* Run 'AspNetAspireMicroservice.AppHost' project Visual Studio 2022 or Visual Studio Code or Rider

## Running in local k8s cluster

```bash
dotnet tool install -g aspirate --prerelease

aspirate init

aspirate generate

aspirate apply
```

## Tool Set

* Asp.Net Aspire
* Asp.Net Core 8
* Entity Framework Core 8
* OpenTelemetry
* PostgreSQL
* RabbitMQ
* Visual Studio 2022 or Visual Studio Code or Rider

## Aspire Dashboard

<img src = "https://github.com/ahmettugur/AspNetAspireMicroservice/blob/master/AspNetAspireMicroservice/_images/aspire_dashboard.png" />


## Swagger

```JSON
{
  "name": "Ahmet Tügür",
  "email": "ahmet@random.com",
  "membership": "Regular",
  "BirthDate": "1990-01-01"
}
```

<img src = "https://github.com/ahmettugur/AspNetAspireMicroservice/blob/master/AspNetAspireMicroservice/_images/swagger_ui.png" />


## Tracing

<img src = "https://github.com/ahmettugur/AspNetAspireMicroservice/blob/master/AspNetAspireMicroservice/_images/tracing.png" />

<img src = "https://github.com/ahmettugur/AspNetAspireMicroservice/blob/master/AspNetAspireMicroservice/_images/tracing_detail.png" />

## Structured Logs

<img src = "https://github.com/ahmettugur/AspNetAspireMicroservice/blob/master/AspNetAspireMicroservice/_images/logs.png" />
