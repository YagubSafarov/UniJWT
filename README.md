# UniJWT

A simple utility for encoding and decoding JWT tokens in Unity. Supports HMAC algorithms and allows adding custom algorithms.

## Features

- Encode and decode JWT tokens.
- Supports HMAC algorithms (`HS256`).
- Custom algorithms can be added.
- Provides error handling for invalid tokens and signatures.

## Installation

Add the package to your Unity project using Unity Package Manager (UPM).
   - Go to `Window` > `Package Manager`.
   - Click the `+` button and select **Add package from git URL**.
   - Enter the URL: `https://github.com/YagubSafarov/UniJWT.git`

## Usage

### Encoding and Decoding JWT Tokens

Below is an example of how to use the JWT utility to generate and validate JWT tokens.

```csharp
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace UniJWT
{
    public class Sample : MonoBehaviour
    {
        private void Start()
        {
            const string secret = "secret_key";

            JwtHandler jwt = JwtHandler.Create();

            Dictionary<string, object> payload = new Dictionary<string, object>();
            payload.Add("Key", "Value");

            // Generate a JWT token
            string token = jwt.GenerateToken(payload, secret);

            Debug.Log($"Token: {token}");

            try
            {
                // Validate the token and get the decoded payload
                Dictionary<string, object> decodedPayload = jwt.ValidateTokenSignatureAndGetPayload(token, secret);
                Debug.Log($"Token decoded. \r\nPayload json: \r\n{JsonConvert.SerializeObject(decodedPayload)}");
            }
            catch (InvalidTokenException invalidTokenException)
            {
                Debug.LogError(invalidTokenException.Message);
            }
            catch (InvalidSignatureException invalidSignatureException)
            {
                Debug.LogError(invalidSignatureException.Message);
            }
            catch (UnsupportedAlgorithmException unsupportedAlgorithmException)
            {
                Debug.LogError(unsupportedAlgorithmException.Message);
            }
            catch (UnsupportedTypeException unsupportedTypeException)
            {
                Debug.LogError(unsupportedTypeException.Message);
            }
        }
    }
}
