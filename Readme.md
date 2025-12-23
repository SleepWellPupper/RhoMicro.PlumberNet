![Logo](https://raw.githubusercontent.com/PaulBraetz/RhoMicro.PlumberNet/release/ReadmeLogo.svg)

# PlumberNet

This is a library allowing for functional, allocation free, highly efficient pipe operator use in C#.
Now we OOP SOUP developers can do Functional Programming too!

*Note: The author was very much preoccupied with whether or not they could, they didn't stop to think if they should.*

## Licensing

This library is licensed to you under the MPL-2.0 license.

## Features

- allocation free
- closure-free
- support the full gamut of `System.Func`
- opinionated, holistic api surface

## Installation

Terminal:

`dotnet add package RhoMicro.PlumberNet`

PackageReference:

```xml

<PropertyGroup>
    <PackageReference Include="RhoMicro.PlumberNet" Version="1.0.0"/>
</PropertyGroup>
```

## Example

Pipes are explicitly entered and exited using the `Plumbing.Pipe` property.
We recommend adding a `using static RhoMicro.PlumberNet.Plumbing` to your file for this.

Pipes contain arguments and function names.
Note that functions are eagerly evaluated, no lazy evaluation occurs (like in Linq).

Enter a pipe like so:

```cs
var username = Pipe | ReadUserNameInput;
var password = Pipe | "Password1";

static String ReadUserNameInput() => "Foo";
```

Here, `username` is a new pipe that contains a single argument with the value
returned by the `ReadUserNameInput` function.
`password` is a new pipe that contains a single argument with the value of the literal.

Combine pipes with pipes:

```cs
var usernameAndPassword = username | password;
```

`usernameAndPassword` is a new pipe that contains both the `username` and `password` arguments, in that order.

Invoke methods with pipes:

```cs
var isValidPassword = usernameAndPassword | IsValidPassword;

private bool IsValidPassword(string? username, string password) 
    => password is "Password1" && username is "Foo";
```

`isValidPassword` is a pipe containing a single argument.
Its value is the result of invoking `IsValidPassword` with the two arguments present in the `usernameAndPassword` pipe.

Exit the pipe (exfiltrate values from pipe arguments) by appending the `Pipe` value to the pipe:

```cw
User? user = isValidPassword | username | password
           | GetUser
           | Pipe;

private User? GetUser(bool isValid, string username, string password)
    => isValid ? new User(username, password) : null;
        
record User(string Name, string Password);
```

Compose and inline pipes for functional expressions:

```cs
var username = Pipe | ReadUserNameInput;
var password = Pipe | "Password1";
User? user = Pipe
           | username | password
           | IsValidPassword
           | username | password
           | GetUser
           | Pipe;
```

## TODO

- Task/ValueTask aware source generator for more concise async/await use within pipe
