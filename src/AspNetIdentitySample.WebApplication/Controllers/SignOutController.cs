﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.Controllers
{
  using System;

  using Microsoft.AspNetCore.Identity;

  using AspNetIdentitySample.ApplicationCore.Entities;
  using AspNetIdentitySample.WebApplication.Defaults;
  using AspNetIdentitySample.WebApplication.ViewModels;

  /// <summary>Provides a simple API to handle HTTP requests.</summary>
  [Route(Routing.AccountRoute)]
  public sealed class SignOutController : Controller
  {
    public const string ViewName = "SignOutView";

    private readonly SignInManager<UserEntity> _signInManager;

    /// <summary>Initializes a new instance of the <see cref="AspNetIdentitySample.WebApplication.Controllers.SignOutController"/> class.</summary>
    /// <param name="signInManager">An object that provides the APIs for user sign in.</param>
    public SignOutController(SignInManager<UserEntity> signInManager)
    {
      _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    /// <summary>Handles the GET request.</summary>
    /// <param name="vm">An object that represents data to sign out an account.</param>
    /// <returns>An object that defines a contract that represents the result of an action method.</returns>
    [HttpGet(Routing.SignOutEndpoint)]
    public IActionResult Get(SignOutAccountViewModel vm)
    {
      return View(SignOutController.ViewName, vm);
    }

    /// <summary>Handles the POST request.</summary>
    /// <param name="vm">An object that represents data to sign out an account.</param>
    /// <returns>An object that represents an asynchronous operation that produces a result at some time in the future. The result is an object that defines a contract that represents the result of an action method.</returns>
    [HttpPost(Routing.SignOutEndpoint)]
    public async Task<IActionResult> Post(SignOutAccountViewModel vm)
    {
      await _signInManager.SignOutAsync();

      return LocalRedirect(vm.ReturnUrl);
    }
  }
}
