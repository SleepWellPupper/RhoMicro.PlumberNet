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

    static String ReadUserNameInput() => "Foo";

    [Fact]
    public async Task Test1()
    {
        var username = Pipe | ReadUserNameInput;
        var password = Pipe | "Password1";
        User? user = Pipe
                   | username | password
                   | IsValidPassword
                   | username | password
                   | GetUser
                   | Pipe;

        Assert.NotNull(user);
        Assert.Equal("Foo", user.Name);
        Assert.Equal("Password1", user.Password);
    }

    private User? GetUser(bool isValid, string username, string password)
        => isValid ? new User(username, password) : null;

    private bool IsValidPassword(string? username, string password)
        => password is "Password1" && username is "Foo";
}
