namespace Pipes.Tests;

/*
- PlumberNet
*/
using System;
using System.Runtime.CompilerServices;
using RhoMicro.PlumberNet;
using static RhoMicro.PlumberNet.Plumbing;

public class UnitTest1
{
    record User(String Name, String Password);

    static Task<String> ReadUserNameInput() => Task.FromResult("Foo");

    static String ReadUserPasswordInput() => "Password1";

    [Fact]
    public async Task Test1()
    {
        var username = Pipe | ReadUserNameInput;
        var password = Pipe | ReadUserPasswordInput;
        User? user = Pipe |
                     await username.Argument1 | password
                   | IsValidPassword
                   | await username.Argument1 | password
                   | GetUser
                   | Pipe;

        Assert.NotNull(user);
        Assert.Equal("Foo", user.Name);
        Assert.Equal("Password1", user.Password);
    }

    private User? GetUser(bool isValid, string username, string password)
    {
        return isValid
            ? new User(username, password)
            : null;
    }

    private bool IsValidPassword(string username, string password)
    {
        return password is "Password1" && username is "Foo";
    }
}
