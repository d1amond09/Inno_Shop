﻿using Inno_Shop.Services.UserAPI.Core.Application.Commands;
using Inno_Shop.Services.UserAPI.Core.Domain.DataTransferObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Inno_Shop.Services.UserAPI.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Inno_Shop.Services.UserAPI.Presentation.Extensions;
using Inno_Shop.Services.UserAPI.Core.Application.Contracts;

namespace Inno_Shop.Services.UserAPI.Presentation.Controllers;

[ApiExplorerSettings(GroupName = "v1")]
[Consumes("application/json")]
[Route("api/authentication")]
[ApiController]
public class AuthenticationController(ISender sender, UserManager<User> userManager, IEmailSender emailSender) : ApiControllerBase
{
	private readonly ISender _sender = sender;
	private readonly UserManager<User> _userManager = userManager;
	private readonly IEmailSender _emailSender = emailSender;

	[HttpPost(Name = "SignUp")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
	{
        var baseResult = await _sender.Send(new RegisterUserCommand(userForRegistration));

        if (!baseResult.Success)
            return ProcessError(baseResult);

        var (result, user) = baseResult.GetResult<(IdentityResult, User)>();

		if (!result.Succeeded)
		{
			foreach (var error in result.Errors)
			{
				ModelState.TryAddModelError(error.Code, error.Description);
			}
			return BadRequest(ModelState);
		}

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = user.Email }, Request.Scheme);
        var message = new Message([user.Email!], "Confirmation email link", confirmationLink!, null);
        await _emailSender.SendEmailAsync(message);

        return StatusCode(201);
	}

	[HttpPost("login", Name = "SignIn")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuth)
	{
		var baseResult = await _sender.Send(new ValidateUserCommand(userForAuth));

        if (!baseResult.Success)
            return ProcessError(baseResult);

		var isValidUser = baseResult.GetResult<bool>();

        if (!isValidUser)
			return Unauthorized("Invalid username or password.");

		var user = await _userManager.FindByNameAsync(userForAuth.UserName!);

		if (user == null)
			return Unauthorized("Invalid username or password.");

		var tokenDtoBaseResult = await _sender.Send(new CreateTokenCommand(user, PopulateExp: true));

        if (!tokenDtoBaseResult.Success)
            return ProcessError(tokenDtoBaseResult);

		var tokenDto = tokenDtoBaseResult.GetResult<TokenDto>();

        return Ok(tokenDto);
	}

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return NotFound();

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return Ok(new { Message = "Email verified successfully." });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel forgotPasswordModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(forgotPasswordModel);
        
        var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email!);
        
        if (user == null)
            return NotFound();
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        string callback = Url.Action(nameof(ResetPassword), "Authentication", new { token, email = user.Email }, Request.Scheme)!;
        
        Message message = new([user.Email!], "Reset password token", callback, null);
        
        await _emailSender.SendEmailAsync(message);
        
        return Ok(new { Message = "Password reset link has been sent to your email." });
    }


    [HttpGet("reset-password")]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPasswordModel { Token = token, Email = email };
        return Ok(model);
    }


    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
    {
        if (!ModelState.IsValid)
            return BadRequest(resetPasswordModel);

        var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email!);
        
        if (user == null)
            return NotFound();

        var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token!, resetPasswordModel.Password!);
        
        if (!resetPassResult.Succeeded)
        {
            foreach (var error in resetPassResult.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return Ok(new { Message = "Password reset successful." });
    }
}

