authy-net-client
================

.Net Client for using Authy's API


###Example usage

```
//instantiate client
var client = new AuthyClient("someapikey", testMode:false);

// create an authy user
var authyUserId = client.CreateAuthyUser("name@example.com", "555-555-5555");

// take someToken passed from the user and verify
var isValid = client.VerifyUserToken(authyUserId, someToken);

```