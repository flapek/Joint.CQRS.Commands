# Joint.CQRS.Commands

| Branch  | Build status                                                                                                                             |
| ------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| master  | [![Build Status](https://travis-ci.org/flapek/Joint.CQRS.Commands.svg?branch=master)](https://travis-ci.org/flapek/Joint.CQRS.Commands)  |
| develop | [![Build Status](https://travis-ci.org/flapek/Joint.CQRS.Commands.svg?branch=develop)](https://travis-ci.org/flapek/Joint.CQRS.Commands) |

## Commands

Adds an ability to create and process commands in the sense of [CQRS](https://martinfowler.com/bliki/CQRS.html).

## Installation

```
dotnet add package Joint.CQRS.Commands
```

## Dependencies

- [Joint](https://www.nuget.org/packages/Joint/)
- [Microsoft.Extensions.DependencyInjection](https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection/)

## Usage

### Without return type

Implement `ICommand` (marker) interface in the selected class. Since the command represents the user’s intention you should follow the convention:

- keep all the commands **immutable**
- name of your commands should be **imperative**

```c#
public class CreateAccount : ICommand
{
    public Guid Id { get; }
    public string Email { get; }
    public string Password { get; }

    public CreateUser(id, email, password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}
```

Create dedicated **command handler** class that implements `ICommandHandler<TCommand>` interface with `HandleAsync()` method:

```c#
public class CreateAccountHandler : ICommandHandler<CreateAccount>
{
    public Task HandleAsync(CreateAccount command)
    {
        //put the handling code here
    }
}
```

Then simply inject `ICommandDispatcher` into a class and call `SendAsync()` method:

```c#
public class AccountsService
{
    private readonly ICommandDispatcher _dispatcher;

    public AccountsService(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public Task CreateAccountAsync(CreateAccount command)
        => _dispatcher.SendAsync(command);
}
```

### With return type

Implement `ICommand<TResult>` (marker) interface in the selected class. Since the command represents the user’s intention you should follow the convention:

- keep all the commands **immutable**
- name of your commands should be **imperative**

```c#
public class SignIn : ICommand<AuthDto>
{
    public Guid Id { get; }
    public string Email { get; }
    public string Password { get; }

    public SignIn(id, email, password)
    {
        Id = id;
        Email = email;
        Password = password;
    }
}
```

Create dedicated **command handler** class that implements `ICommandHandler<TCommand, TResult>` interface with `HandleAsync()` method:

```c#
public class SignInHandler : ICommandHandler<SignIn, AuthDto>
{
    public Task<AuthDto> HandleAsync(SignIn command)
    {
        //put the handling code here
    }
}
```

Then simply inject `ICommandDispatcher` into a class and call `SendAsync<TResult>()` method:

```c#
public class AccountsService
{
    private readonly ICommandDispatcher _dispatcher;

    public AccountsService(ICommandDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public Task CreateAccountAsync(SignIn command)
    {
        var return = _dispatcher.SendAsync<AuthDto>(command);
        //do something with return type
    }
}
```

### Common

You can easily register all command handlers in DI container by calling `AddCommandHandlers()` method on `IJointBuilder`:

```c#
public IServiceProvider ConfigureServices(this IServiceCollection services)
{
    var builder = JointBuilder
        .Create(services)
        .AddCommandHandlers();

    //other registrations
    return builder.Build();
}
```

Dispatching a particular command object can be also done using Joint package. Start with registering in-memory dispatcher on your `IJointBuilder` by calling a `AddInMemoryCommandDispatcher()` method:

```c#
public IServiceProvider ConfigureServices(this IServiceCollection services)
{
    var builder = JointBuilder
        .Create(services)
        .AddCommandHandlers()
        .AddInMemoryCommandDispatcher();

    //other registrations
    return builder.Build();
}
```

## Table of Contents

- [Joint](https://github.com/flapek/Joint)
- [Joint.Auth](https://github.com/flapek/Joint.Auth)
- [Joint.CQRS.Commands](https://github.com/flapek/Joint.CQRS.Commands)
- [Joint.CQRS.Events](https://github.com/flapek/Joint.CQRS.Events)
- [Joint.CQRS.Queries](https://github.com/flapek/Joint.CQRS.Queries)
- [Joint.DB.Mongo](https://github.com/flapek/Joint.DB.Mongo)
- [Joint.DB.Redis](https://github.com/flapek/Joint.DB.Redis)
- [Joint.Docs.Swagger](https://github.com/flapek/Joint.Docs.Swagger)
- [Joint.Exception](https://github.com/flapek/Joint.Exception)
- [Joint.Logging](https://github.com/flapek/Joint.Logging)
- [Joint.WebApi](https://github.com/flapek/Joint.WebApi)
