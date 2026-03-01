using Banking.Accounts.Infrastructure.Contracts.Input.Request.Accounts;
using Banking.Accounts.Infrastructure.Contracts.Output.Response.Account;
using Banking.Accounts.Models.Account;
using Banking.Accounts.Models.Commands;
using Banking.Accounts.Models.Transaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Accounts.Service.Controllers;

/// <summary>
/// Контроллер для управления банковскими счетами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountsController : ControllerBase
{
    /// <summary>
    /// Инициализирует новый экземпляр контроллера <see cref="AccountsController"/>.
    /// </summary>
    /// <param name="mediator">
    /// Экземпляр MediatR для отправки команд и запросов.
    /// </param>
    public AccountsController(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        _mediator = mediator;
    }

    /// <summary>
    /// Открыть новый счет для клиента.
    /// </summary>
    /// <param name="request">
    /// Данные для создания счета.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Идентификатор созданного счета.
    /// </returns>
    [ProducesResponseType(typeof(AccountId), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateAccount(
        [FromBody] CreateAccountRequest request,
        CancellationToken token)
    {
        var command = new OpenAccountCommand(new(request.CustomerId), request.Currency);
        var accountId = await _mediator.Send(command, token);

        return StatusCode(StatusCodes.Status201Created, new CreateAccountResponse { AccountId = accountId.Value });
    }

    /// <summary>
    /// Пополнение счета клиента.
    /// </summary>
    /// <param name="idempotencyKey">
    /// Уникальный ключ идемпотентности.
    /// </param>
    /// <param name="id">
    /// Идентификатор счета.
    /// </param>
    /// <param name="request">
    /// Данные для пополнения.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Текущий баланс счета после пополнения.
    /// </returns>
    [ProducesResponseType(typeof(AccountId), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointDescription("Пополнение счета. Требует X-Idempotency-Key в заголовках.")]
    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(
        [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey,
        [FromRoute] Guid id,
        [FromBody] DepositRequest request,
        CancellationToken token)
    {
        var referenceId = new ReferenceId(Guid.Parse(idempotencyKey));

        var command = new DepositCommand(
            new(id),
            referenceId,
            new(request.Amount.Value, request.Amount.Currency));

        var balance = await _mediator.Send(command, token);

        return StatusCode(StatusCodes.Status202Accepted, new DepositResponse { Balance = balance.Value });
    }

    /// <summary>
    /// Списание средств со счета клиента.
    /// </summary>
    /// <param name="idempotencyKey">
    /// Уникальный ключ идемпотентности.
    /// </param>
    /// <param name="id">
    /// Идентификатор счета.
    /// </param>
    /// <param name="request">
    /// Данные для списания.
    /// </param>
    /// <param name="token">
    /// Токен отмены операции.
    /// </param>
    /// <returns>
    /// Текущий баланс счета после списания.
    /// </returns>
    [ProducesResponseType(typeof(AccountId), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EndpointDescription("Списание со счета. Требует X-Idempotency-Key в заголовках.")]
    [HttpPost("{id}/withdraw")]
    public async Task<IActionResult> Withdraw(
        [FromHeader(Name = "X-Idempotency-Key")] string idempotencyKey,
        [FromRoute] Guid id,
        [FromBody] WithdrawRequest request,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(request);

        var referenceId = new ReferenceId(Guid.Parse(idempotencyKey));

        var command = new WithdrawCommand(
            new(id),
            referenceId,
            new(request.Amount.Value, request.Amount.Currency));

        var balance = await _mediator.Send(command, token);

        return StatusCode(StatusCodes.Status202Accepted, new WithdrawResponse { Balance = balance.Value });
    }

    private readonly IMediator _mediator;
}