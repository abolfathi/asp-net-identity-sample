﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.Stores
{
  using Microsoft.AspNetCore.Identity;

  using AspNetIdentitySample.ApplicationCore.Entities;
  using AspNetIdentitySample.ApplicationCore.Services;

  /// <summary>Provides an abstraction for a store which manages user accounts.</summary>
  public sealed class UserStore : IUserStore<UserEntity>, IUserPasswordStore<UserEntity>, IUserRoleStore<UserEntity>
  {
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;

    /// <summary>Initializes a new instance of the <see cref="AspNetIdentitySample.WebApplication.Stores.UserStore"/> class.</summary>
    /// <param name="userService">An object that provides a simple API to execute queries and commands with the <see cref="AspNetIdentitySample.ApplicationCore.Entities.UserEntity"/>.</param>
    /// <param name="userRoleService">An object that provides a simple API to execute queries and commands with the <see cref="AspNetIdentitySample.ApplicationCore.Entities.UserRoleEntity"/>.</param>
    public UserStore(IUserService userService, IUserRoleService userRoleService)
    {
      _userService = userService ?? throw new ArgumentNullException(nameof(userService));
      _userRoleService = userRoleService ?? throw new ArgumentNullException(nameof(userRoleService));
    }

    #region Members of IUserStore

    /// <summary>
    /// Gets the user identifier for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose identifier should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the identifier for the specified <paramref name="user"/>.</returns>
    public Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken)
      => Task.FromResult(user.Id.ToString());

    /// <summary>
    /// Gets the user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the name for the specified <paramref name="user"/>.</returns>
    public Task<string?> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken)
      => Task.FromResult(user.Email);

    /// <summary>
    /// Sets the given <paramref name="userName" /> for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="userName">The user name to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetUserNameAsync(UserEntity user, string? userName, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    /// <summary>
    /// Gets the normalized user name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose normalized name should be retrieved.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the normalized user name for the specified <paramref name="user"/>.</returns>
    public Task<string?> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    /// <summary>
    /// Sets the given normalized name for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose name should be set.</param>
    /// <param name="normalizedName">The normalized name to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetNormalizedUserNameAsync(UserEntity user, string? normalizedName, CancellationToken cancellationToken)
    {
      user.Email = normalizedName;

      return Task.CompletedTask;
    }

    /// <summary>
    /// Creates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to create.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the creation operation.</returns>
    public async Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
    {
      await _userService.AddUserAsync(user, cancellationToken);

      return IdentityResult.Success;
    }

    /// <summary>
    /// Updates the specified <paramref name="user"/> in the user store.
    /// </summary>
    /// <param name="user">The user to update.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the update operation.</returns>
    public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
    {
      await _userService.UpdateUserAsync(user, cancellationToken);

      return IdentityResult.Success;
    }

    /// <summary>
    /// Deletes the specified <paramref name="user"/> from the user store.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the delete operation.</returns>
    public async Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
    {
      await _userService.DeleteUserAsync(user, cancellationToken);

      return IdentityResult.Success;
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
    /// </summary>
    /// <param name="userId">The user ID to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
    /// </returns>
    public Task<UserEntity?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      if (Guid.TryParse(userId, out var userIdValue))
      {
        var userEntity = new UserEntity
        {
          UserId = userIdValue,
        };

        return _userService.GetUserAsync(userEntity, cancellationToken);
      }

      return Task.FromResult(default(UserEntity));
    }

    /// <summary>
    /// Finds and returns a user, if any, who has the specified normalized user name.
    /// </summary>
    /// <param name="normalizedUserName">The normalized user name to search for.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="normalizedUserName"/> if it exists.
    /// </returns>
    public Task<UserEntity?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
      => _userService.GetUserAsync(normalizedUserName, cancellationToken);

    #endregion

    #region Members of IUserPasswordStore

    /// <summary>
    /// Sets the password hash for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose password hash to set.</param>
    /// <param name="passwordHash">The password hash to set.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task SetPasswordHashAsync(UserEntity user, string? passwordHash, CancellationToken cancellationToken)
    {
      user.PasswordHash = passwordHash;

      return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the password hash for the specified <paramref name="user"/>.
    /// </summary>
    /// <param name="user">The user whose password hash to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, returning the password hash for the specified <paramref name="user"/>.</returns>
    public Task<string?> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken)
      => Task.FromResult(user.PasswordHash);

    /// <summary>
    /// Gets a flag indicating whether the specified <paramref name="user"/> has a password.
    /// </summary>
    /// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, returning true if the specified <paramref name="user"/> has a password
    /// otherwise false.
    /// </returns>
    public Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    #endregion

    #region Members of IUserRoleStore

    /// <summary>
    /// Add the specified <paramref name="user"/> to the named role.
    /// </summary>
    /// <param name="user">The user to add to the named role.</param>
    /// <param name="roleName">The name of the role to add the user to.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task AddToRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    /// <summary>
    /// Remove the specified <paramref name="user"/> from the named role.
    /// </summary>
    /// <param name="user">The user to remove the named role from.</param>
    /// <param name="roleName">The name of the role to remove.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
    public Task RemoveFromRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    /// <summary>
    /// Gets a list of role names the specified <paramref name="user"/> belongs to.
    /// </summary>
    /// <param name="user">The user whose role names to retrieve.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of role names.</returns>
    public Task<IList<string>> GetRolesAsync(UserEntity user, CancellationToken cancellationToken)
      => _userRoleService.GetRoleNamesAsync(user, cancellationToken);

    /// <summary>
    /// Returns a flag indicating whether the specified <paramref name="user"/> is a member of the given named role.
    /// </summary>
    /// <param name="user">The user whose role membership should be checked.</param>
    /// <param name="roleName">The name of the role to be checked.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user"/> is
    /// a member of the named role.
    /// </returns>
    public Task<bool> IsInRoleAsync(UserEntity user, string roleName, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    /// <summary>
    /// Returns a list of Users who are members of the named role.
    /// </summary>
    /// <param name="roleName">The name of the role whose membership should be returned.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
    /// <returns>
    /// The <see cref="Task"/> that represents the asynchronous operation, containing a list of users who are in the named role.
    /// </returns>
    public Task<IList<UserEntity>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
      => throw new NotImplementedException();

    #endregion

    #region Members of IDisposable

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() { }

    #endregion
  }
}
