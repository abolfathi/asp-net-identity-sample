﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.ViewModels
{
  using AspNetIdentitySample.ApplicationCore.Identities;

  /// <summary>Represents the view model for the user list action.</summary>
  public sealed class UserListViewModel : ViewModelBase
  {
    /// <summary>Gets/sets an object that represents a collection of user records.</summary>
    public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();

    /// <summary>Represents the view model for the user record.</summary>
    public sealed class UserViewModel : IUserIdentity
    {
      /// <summary>Gets/sets an object that represents an ID of a user.</summary>
      public Guid UserId { get; set; }

      /// <summary>Gets/sets an object that represents a fist name of a user.</summary>
      public string? FirstName { get; set; }

      /// <summary>Gets/sets an object that represents a last name of a user.</summary>
      public string? LastName { get; set; }

      /// <summary>Gets/sets an object that represents an email of a user.</summary>
      public string? Email { get; set; }

      /// <summary>Gets/sets an object that represents a string of user roles.</summary>
      public string? Roles { get; set; }
    }
  }
}
