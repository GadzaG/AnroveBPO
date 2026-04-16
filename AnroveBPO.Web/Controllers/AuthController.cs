using AnroveBPO.Contracts.Auth.Requests;
using AnroveBPO.Domain.Shared;
using AnroveBPO.Infrastructure.Identity.Jwt;
using AnroveBPO.Infrastructure.Identity.Models;
using AnroveBPO.Web.EndpointResults;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = AnroveBPO.Contracts.Auth.Requests.LoginRequest;

namespace AnroveBPO.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<EndpointResult<string>> Login(
        [FromBody] LoginRequest request,
        [FromServices] UserManager<User> userManager,
        [FromServices] JwtTokenProvider jwtTokenProvider,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.UserName))
        {
            return Result.Failure<string, Error>(
                Error.Validation("auth.username.required", "Username is required", "userName"));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return Result.Failure<string, Error>(
                Error.Validation("auth.password.required", "Password is required", "password"));
        }

        User? user = await userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return Result.Failure<string, Error>(
                Error.Authentication("auth.invalid.credentials", "Invalid username or password"));
        }

        bool isPasswordValid = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
        {
            return Result.Failure<string, Error>(
                Error.Authentication("auth.invalid.credentials", "Invalid username or password"));
        }

        IList<string> roles = await userManager.GetRolesAsync(user);
        Result<string, Error> tokenResult = jwtTokenProvider.GenerateAccessToken(user, roles.ToArray(), ct);

        return tokenResult.IsFailure
            ? Result.Failure<string, Error>(tokenResult.Error)
            : Result.Success<string, Error>(tokenResult.Value);
    }
}
/*Сделал все что успел.
    Оформил слои :
- Domain слой для описания доменных корневых сущностей, а так же свой класс для ошибок и переопредедления для Exceptions
    - Application слой с бизнес логикой(тут объявленны интерфейсы репозиториев, а так же  ITransactionManager(UnitOfWork) и TransactionScope(но его я тут не использовал))
+ метод расширения для добавления в DI(я там использовал Scruttor, чтобы автоматически все Handler'ы добавлялись)
я тут ипользовал еще паттерн CQS разделяя бизнес - логику на обработчики чтения и записи.
- Infrastructure.Postgres тут реализация интерфейсов для работы с бд, миграции, конфигурации таблиц в базе данных + метод расширения для добавления в DI
    - Infrastructure.Identity тут я реализовал работу с пользователями, используя AspNetCore.Identity.
    тут логика генерации jwtToken'ов и так же метод расширения для DI. Я хотел сделать тут реализовать пользователя и совместить его с Customer(или можно было бы прям тут реализовать Customer'a), но я до конца не успел.
- Web слой представления. Тут чисто webapi. в папке Configurations настройки для Di и для самого приложения. так же я добавил middlewere для обработок unhandled exceptions. еще я тут сделал свой класс endpointResult наследованный от IResult, для удобной работы с возвратом результата


    ПО УМОЛЧАНИЮ СОЗДАЕТСЯ ПОЛЬЗОВАТЕЛЬ С РОЛЬЮ АДМИНА (admin admin)
для запуска просто запустить docker-compose up */