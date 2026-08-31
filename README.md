[![](https://img.shields.io/nuget/v/soenneker.highlevel.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.highlevel.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.highlevel.openapiclient/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.highlevel.openapiclient/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.highlevel.openapiclient.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.highlevel.openapiclient/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.highlevel.openapiclient/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.highlevel.openapiclient/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.HighLevel.OpenApiClient

Call HighLevel endpoints through a Kiota-generated client with typed request builders and models.

## Install

```bash
dotnet add package Soenneker.HighLevel.OpenApiClient
```

## Create a client

```csharp
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Soenneker.HighLevel.OpenApiClient;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://services.leadconnectorhq.com/")
};
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", apiKey);
httpClient.DefaultRequestHeaders.Add("Version", "2021-07-28");

var adapter = new HttpClientRequestAdapter(
    new AnonymousAuthenticationProvider(),
    httpClient: httpClient);

var client = new HighLevelOpenApiClient(adapter);
```

The `HttpClient` supplies authentication and the HighLevel API version, so the Kiota adapter uses anonymous authentication. Reuse the transport rather than constructing one per request, and dispose it when its owning application component shuts down.

For application registration, per-API-key client reuse, and coordinated transport ownership, use `Soenneker.HighLevel.ClientUtil` instead of constructing the generated client directly.

## Call an endpoint

```csharp
using Soenneker.HighLevel.OpenApiClient.Models;

ContactsByIdSuccessfulResponseDto? response = await client.Contacts[contactId]
    .GetAsync(cancellationToken: cancellationToken);
```

The generated surface follows the OpenAPI document: top-level properties such as `Contacts`, `Calendars`, `Conversations`, `Locations`, and `Opportunities` expose request builders, while types under `Soenneker.HighLevel.OpenApiClient.Models` represent request and response bodies.

HTTP failures are surfaced through Kiota exceptions. Nullable results indicate that an endpoint returned no response body.

This repository contains generated code. Put reusable helpers and behavior changes in a separate package so regeneration does not overwrite them.
