# DOTelL

Developer's OpenTelemetry Lens (DOTelL) solves the problem of providing a simple, locally hosted OpenTelemetry back-end.

## Why

Many OpenTelemetry back-ends either require you to use a hosted service (e.g. Honeycomb) or one or more resource-heavy local services to be able to ingest data from OpenTelemetry signals and view that data. Sometimes all a developer wants is a simplified view over locally emitted telemetry to validate that their applications are instrumented well before deploying them.

DOTelL aims to do the following things:

* Be as resource-light as possible
* Provide OpenTelemetry ingestion endpoints for all signal types e.g. traces/metrics/logs etc.
* Provide a simple UI over the data so that developers can easily explore the telemetry being emitted by services they are writing locally

DOTelL is _not_ meant to replace a production-grade OTel backend. It is only meant to be used during local development to provide a simple view over locally emitted telemetry. Because it only implements OTLP ingestion formats it is trivial to switch your application to a different backend in deployed environments. It works well either as a direct export target from your applications, or as a target from a locally running instance of the OpenTelemetry Collector.

## Technology

1. .NET for hosting gRPC services to ingest telemetry, and exposing APIs for querying the ingested telemetry
2. Flutter for the SPA frontend, which is served statically by the .NET backend
3. SignalR for realtime UI updates whenever telemetry is ingested
4. DuckDB for storing and querying telemetry

## Building

1. Clone this repository and ensure you use the `--recurse-submodules` git clone argument, which will pull down the [opentelemetry-proto](https://github.com/open-telemetry/opentelemetry-proto) git [submodule](https://git-scm.com/book/en/v2/Git-Tools-Submodules) e.g. `git clone --recurse-submodules https://github.com/timcallaghan/dotell.git`
   1. In cases where `--recurse-submodules` wasn't specified when cloning you can restore the submodules by issuing the command `git submodule update --init --recursive`
2. Install prerequisites:
   1. [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
   2. TODO...