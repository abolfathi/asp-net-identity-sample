﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.ViewModels
{
  using System.ComponentModel.DataAnnotations;

  using AspNetIdentitySample.ApplicationCore.Identities;

  /// <summary>Represents the view model of the user action.</summary>
  public sealed class UserViewModel : ViewModelBase, IUserIdentity
  {
    /// <summary>Gets/sets an object that represents an ID of a user.</summary>
    public Guid UserId { get; set; }

    /// <summary>Gets/sets an object that represents a fist name of a user.</summary>
    [Required]
    public string? FirstName { get; set; }

    /// <summary>Gets/sets an object that represents a last name of a user.</summary>
    [Required]
    public string? LastName { get; set; }

    /// <summary>Gets/sets an object that represents an email of a user.</summary>
    public string? Email { get; set; }
  }
}
