# Development

This document covers conventions used throughout the DOTelL repository, as well as how to build and test DOTelL locally.

## Conventional Commits

[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) are being used to make the process of generating releases automatic.

In pratice, this means that any commit message needs to be structured as:

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

The possible values for [`<type>`](https://github.com/commitizen/conventional-commit-types/blob/master/index.json)

Common values used in this repository are: 

* `feat` - a new feature
* `fix` - a bug fix
* `docs` - documentation changes
* `chore` - changes that don't modify source or test files
* `ci` - changes to the CI process

Some examples,

```
feat: Adding additional API endpoint for root spans

Create custom SQL to locate root spans (those without a parent span) and expose via the API
```

```
feat!: Redesign the database schema

Rework the entire database schema for better performance on large telemetry datasets

BREAKING CHANGE: Whilst the API is unchanged, the persistance layer has breaking changes which may cause existing custom SQL queries to fail
```

## Conventional PR Titles

A [GitHub workflow](../.github/workflows/check-semantic-prs.yml) runs on pull requests that enforces each PR `Title` to conform to the Conventional Commit spec.

Some example PR titles,

```
fix: Correct root span trace SQL query
```

```
feat!: Redesign the database schema
```

If your PR fails due to incorrect title, simply update the title to conform to the Conventional Commit spec.

## Technology

1. [.NET](https://dotnet.microsoft.com/en-us/) for hosting gRPC services to ingest telemetry
2. [PostgreSQL](https://www.postgresql.org/) for storing and querying telemetry
3. [pgweb](https://github.com/sosedoff/pgweb) for telemetry signal visualisation and exploration

All of the above are [combined into a single Docker image](https://docs.docker.com/config/containers/multi-service_container/) that can be easily run locally. 

_NOTE: This is done purposefully for ease of use by developers and in no way endorses running multiple processes inside a single container in deployed environments._

## Building

1. Clone this repository and ensure you use the `--recurse-submodules` git clone argument, which will pull down the [opentelemetry-proto](https://github.com/open-telemetry/opentelemetry-proto) git [submodule](https://git-scm.com/book/en/v2/Git-Tools-Submodules) e.g. `git clone --recurse-submodules https://github.com/timcallaghan/dotell.git`
   1. In cases where `--recurse-submodules` wasn't specified when cloning you can restore the submodules by issuing the command `git submodule update --init --recursive`
2. Install prerequisites:
   1. [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
3. Build a local docker image with `docker build -t dotell.api:dev -f src/DOTelL.Api/Dockerfile .`
4. Run the image with `docker run -p 4317:4317 -p 5042:5042 --name dotell-testing -d dotell.api:dev`

Because of the way the DOTelL Docker image runs multiple processes it is non-trivial to debug the gRPC endpoints inside the running Docker container.

If you need to debug DOTelL gRPC endpoints it is easiest to run a separate instance of PostgreSQL locally and configure the DB connection string in [appsettings.json](src/DOTelL.Api/appsettings.json), and then debug the application from your IDE of choice (JetBrains Rider is preferred).