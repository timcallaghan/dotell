# DOTelL

Developer's OpenTelemetry Lens ([DOTelL](https://en.wiktionary.org/wiki/do_tell)) solves the problem of providing a simple, locally hosted OpenTelemetry back-end.

## Why

Many OpenTelemetry back-ends either require you to use a hosted service (e.g. Honeycomb, Datadog) or one or more resource-heavy local services (e.g. SigNoz) to be able to ingest data from OpenTelemetry signals and view that data. Sometimes all a developer wants is a simplified view over locally emitted telemetry to validate that their applications are instrumented well before deploying them.

DOTelL aims to do the following things:

* Be as resource-light as possible
* Provide OpenTelemetry ingestion endpoints for all signal types e.g. traces/metrics/logs
* Provide a simple UI over the data so that developers can easily explore the telemetry being emitted by services they are writing locally

DOTelL is _not_ meant to replace a production-grade OTel backend. It is only meant to be used during local development to provide a simple view over locally emitted telemetry. There is no support for authentication and authorisation which means it should _never_ be deployed to a publicly facing environment.

Because it only implements [OTLP](https://opentelemetry.io/docs/specs/otel/protocol/) ingestion formats it is trivial to switch your application to a different telemetry backend in deployed environments. It works well either as a direct export target from your applications, or as a target from a locally running instance of the OpenTelemetry Collector.

## Installation

DOTelL is designed to run as a single Docker container.

TODO: Update with the docker run command once we have ghcr images via GitHub workflow
TODO: Different run options depending on what the dev wants to expose locally.

Once installed you can configure your local services to send OTLP telemetry via gRPC to the locally exposed port `4317`.

The PostgreSQL database is (optionally) exposed on the default port of 5432 with user `postgres` and password `password`. The database name is `dotell`.

The bundled pgweb admin UI is exposed locally on port `5042` and can be accessed in your browser of choice on [http://localhost:5042](http://localhost:5042).
