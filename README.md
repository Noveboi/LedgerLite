# LedgerLite - Web API
Simple, intuitive accounting software.

This repository contains the back-end infrastructure for LedgerLite, if you'd like to visit the front-end repository, click [here](https://github.com/Noveboi/LedgerLite.Web).

## Architecture / Solution Structure
The back-end of LedgerLite is architected as a loosely coupled **modular monolith**, each module is internally structured using **clean architecture** principles. Below is a glossary for project naming conventions:
- `LedgerLite.{Module}`: Describes a project which encapsulates a module. Everything in a module project should be internal as to avoid cross-module dependencies. Public classes/interfaces should be included in the corresponding `Contracts` project .
- `LedgerLite.{Module}.Contracts`: Describe a project which defines a public API for the related module. Other modules can use `Contracts` projects for cross-module communication
- `LedgerLite.WebApi`: This specific project contains the web API configuration and program entry point.

## Module Structure
As mentioned above, each module follows clean architecture guidelines for structure:
- `Application`: Module use cases and services.
- `Domain`: Core business model and rules of the module.
- `Endpoints`: Web API endpoints and DTOs
- `Infrastructure`: Handles external dependencies such as databases and third-party web API calls. Mainly used for configuring a `DbContext` for the module with EF Core.
- `DependencyInjection.cs`: Registers the module's services in ASP.NET Core's dependency injection.
