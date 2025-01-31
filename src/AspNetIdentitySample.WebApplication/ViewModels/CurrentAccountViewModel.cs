﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.ViewModels
{
  using AspNetIdentitySample.ApplicationCore.Entities;
  using AspNetIdentitySample.ApplicationCore.Identities;

  /// <summary>Represents details of the authenticated user.</summary>
  public sealed class CurrentAccountViewModel : IUserIdentity
  {
    /// <summary>Gets/sets an object that represents an ID of a user.</summary>
    public Guid UserId { get; set; }

    /// <summary>Gets/sets an object that represents a first name of a user.</summary>
    public string? FirstName { get; private set; }

    /// <summary>Gets/sets an object that represents a last name of a user.</summary>
    public string? LastName { get; private set; }

    /// <summary>Gets/sets an object that indicates if the user is authenticated.</summary>
    public bool IsAuthenticated { get; private set; }

    /// <summary>Gets/sets an object that indicates if the user is admin.</summary>
    public bool IsAdmin { get; private set; }

    /// <summary>Populates the view model with the <see cref="AspNetIdentitySample.ApplicationCore.Entities.UserEntity"/>.</summary>
    /// <param name="userEntity">An object that represents details of a user.</param>
    public void FromEntity(UserEntity userEntity)
    {
      UserId = userEntity.Id;
      FirstName = userEntity.FirstName;
      LastName = userEntity.LastName;
      IsAuthenticated = true;
      IsAdmin = userEntity.Roles != null && userEntity.Roles.Any(entity => entity.RoleName == "admin");
    }
  }
}
