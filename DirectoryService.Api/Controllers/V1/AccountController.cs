using System.Text.Json;
using DirectoryService.Api.Attributes;
using DirectoryService.Api.Controllers.V1.Models;
using DirectoryService.Api.Helpers;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.Services;
using DirectoryService.Shared;
using DirectoryService.Shared.Config;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace DirectoryService.Api.Controllers.V1;

[Produces("application/json")]
[Route("api/v1/account")]
[ApiController]
public sealed class AccountController : V1ApiController
{
    private readonly ServiceConfiguration _configuration;
    private readonly UserActivationService _userActivationService;
    private readonly SessionTokenService _sessionTokenService;
    private readonly UserService _userService;
    
    public AccountController(UserActivationService userActivationService,
        SessionTokenService sessionTokenService,
        UserService userService)
    {
        _configuration = ServiceConfigurationContainer.Config;
        _userActivationService = userActivationService;
        _sessionTokenService = sessionTokenService;
        _userService = userService;
    }
    
    /// <summary>
    /// Fetch a list of accounts
    /// </summary>
    [HttpGet("api/v1/accounts")]
    [Authorise]
    public async Task<IActionResult> GetAccounts()
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Get account by user reference (user id or username)
    /// </summary>
    [HttpGet("{userReference}")]
    [Authorise]
    public async Task<IActionResult> GetUser(string userReference)
    {
        var user = await _userService.FindUser(userReference);
        if (user is null)
            throw new UserNotFoundApiException();
        
        RestrictToSelfOrAdmin(user.Id);

        var userInfo = await _userService.GetUserInfo(user.Id);
        
        if (userInfo is null)
            throw new UserNotFoundApiException();
        
        return Success(new
        {
            Account = new AccountInfoModel(userInfo)
        });
    }
    
    /// <summary>
    /// Update account
    /// </summary>
    [HttpPost("{accountId:string}")]
    [Authorise]
    public async Task<IActionResult> UpdateAccount(string accountId, [FromBody] UpdateAccountDto updateAccount)
    {
        updateAccount.AccountId = accountId;
        
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// (Admin) Delete user by user id, username or email
    /// </summary>
    [HttpDelete("{userReference}")]
    [Authorise(UserRole.Admin)]
    public async Task<IActionResult> Delete(string userReference)
    {
        var account = await _userService.FindUser(userReference);
        if (account is null)
            throw new UserNotFoundApiException();
        
        await _userService.DeleteUser(account.Id);
        return Success();
    }
    
    /// <summary>
    /// Fetch specific account property by field name
    /// </summary>
    // Is this in use?
    [HttpGet("{accountId:string}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> GetAccountField(string accountId, string fieldName)
    {
        //TODO
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Update account property by field name
    /// </summary>
    // Is this in use?
    [HttpPost("{accountId:string}/field/{fieldName}")]
    [Authorise]
    public async Task<IActionResult> SetAccountField(string accountId, string fieldName, [FromBodyOrDefault] UpdateFieldRootModel fieldUpdate)
    {
        if (fieldUpdate.Set is null || fieldUpdate.Set.HasValue == false)
            return Failure();

        switch (fieldUpdate.Set.Value.ValueKind)
        {
            case JsonValueKind.String:
                await _userService.UpdateUserByField(accountId, fieldName, new []{fieldUpdate.Set.Value.ToString()});
                return Success();
            
            case JsonValueKind.Array:
                await _userService.UpdateUserByField(accountId, fieldName, 
                    fieldUpdate.Set.Value.EnumerateArray()
                        .Select(value => value.ToString()).ToArray());
                
                return Success();

            default:
                throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// Return active token(s) for account
    /// </summary>
    [HttpGet("{userReference}/tokens")]
    [Authorise]
    public async Task<IActionResult> GetTokens(string userReference)
    {
        var user = await _userService.FindUser(userReference);
        if (user is null)
            throw new UserNotFoundApiException();
        
        var page = PaginatedRequest();

        var result = await _sessionTokenService.ListUserTokens(user.Id, page);
        return new JsonResult(result.Data is not null ? new UserTokenListModel(result.Data) : new UserTokenListModel());
    }

    /// <summary>
    /// Delete an account's token
    /// </summary>
    [HttpDelete("{userReference}/tokens/{token:string}")]
    [Authorise]
    public async Task<IActionResult> DeleteToken(string userReference, string token)
    {
        var user = await _userService.FindUser(userReference);
        if (user is null)
            throw new UserNotFoundApiException();
      
        await _sessionTokenService.RevokeUserToken(user.Id, token);
        
        return Success();
    }

    /// <summary>
    /// Email verification endpoint
    /// </summary>
    [HttpGet("verify/email")]
    [AllowAnonymous]
    public async Task<IActionResult> EmailVerificationEndpoint([FromQuery] EmailVerificationModel verification)
    {
        if (verification.AccountId == null || verification.VerificationToken == null)
            return new RedirectResult(_configuration.Registration!.EmailVerificationFailRedirect!);

        try
        {
            await _userActivationService.ReceiveUserActivationResponse(verification.AccountId, verification.VerificationToken);
            return new RedirectResult(_configuration.Registration!.EmailVerificationSuccessRedirect!);
        }
        catch (Exception e)
        {
            return new RedirectResult(_configuration.Registration!.EmailVerificationFailRedirect!);
        }
    }
}