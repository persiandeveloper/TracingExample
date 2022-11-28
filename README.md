# TracingExample
The aim of this repository, is to show how you can integrate distributaed tracing into your .NET based API app and worker service, using OpenTelemetry.

To collect tracing data, instead of directly sending data to Jager, an OTEL collector container is configured which then acts as a mediator to export trace data to Jager or whatever collector you would like to use.

In this way, when you configure exporter, you do not care about the type or protocol, because the OTEL collector will take care of it. 

## Structure of the solution
### Tracing.Shared.TraceAndLong
This is a shared library that is responsible to configure logging and tracing. This way the tracing and logging is abstracted.

### Tracing.Shared.Messages
Containt the message that will be send from API and be processed by worker service. 

### Tracing.WebAapi
It has a simple controller to use MassTransit send method and put the message in the queue.

### Tracing.WorkerService
A configured MassTransit exist in the project to recieve all the messsages in the queue and process them.

## How to see the traces
Open the http://localhost:16686/search in browser and find the service you want to see its traces. Click on "Find traces" and you dig deep into each trace.
![image](https://user-images.githubusercontent.com/8937406/204265624-47bf3052-f2bd-4e8f-853d-0a88a93b719b.png)

## How to see logs
I could not find an easy to configure free log management tool, so I stick to Console Exporter, therefore you can see the logs by opening either "Tracing.WebAapi" or "Tracing.WorkerService" container, then navigate to "Logs" to see the logs. 
![image](https://user-images.githubusercontent.com/8937406/204267012-3b11caa9-5bdd-4ce3-b557-bf1c03ae5cb0.png)

## How to run
You need Visual Studio 2022 and Dokcer Desktop. After the solution loaded, run it using "Docker Compose" option.

An important note: I did not use MassTransit instrumention because the latest version (8) already has all the configuration, and you just need to call AddSource so the MAssTransit event can be traced.
Read more : https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/326
