Authy .NET Client
---
[![NuGet version](https://badge.fury.io/nu/authy-net-client.svg)](http://badge.fury.io/nu/authy-net-client)

C# .Net Client for using [Authy's](https://www.authy.com/) [API](http://docs.authy.com/)


## Installation

### NuGet
This package is available via NuGet as `authy-net-client`.

```powershell
> Install-Package authy-net-client
```


## Example usage
The library is intended to be super easy to use and friendly with IoC frameworks.

#### Instantiating a client

```csharp
IAuthyApiClient client = new AuthyApiClient("someapikey", testMode:false);
```
#### Creating an Authy User
```csharp
string authyUserId = client.CreateAuthyUser("name@example.com", "555-555-5555");
```

#### Verify a user provided token
```csharp
bool isValid = client.VerifyUserToken(authyUserId, someToken);
```


## Testing
This package contains [mspec](https://github.com/machine/machine.specifications) specs that outline happy-paths and edgecases. They are intended to read like behaviors and should be a good way to reference how to use the library as well.

## Contributing
This library is open for contribution here are the steps to contrubte:

1. [Fork Repo](https://github.com/davidsulpy/authy-net-client#fork-destination-box)
2. Add specs for changes
3. Make changes
4. Ensure passing specs
5. Create pull request
6. Grab beverage


