﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.Stores.Test
{
  using Microsoft.AspNetCore.Identity;

  using AspNetIdentitySample.ApplicationCore.Identities;
  using AspNetIdentitySample.ApplicationCore.Services;

  [TestClass]
  public sealed class UserStoreTest
  {
    private CancellationToken _cancellationToken;

#pragma warning disable CS8618
    private Mock<IUserService> _userServiceMock;
    private Mock<IUserRoleService> _userRoleServiceMock;

    private UserStore _userStore;
#pragma warning restore CS8618

    [TestInitialize]
    public void Initialize()
    {
      _cancellationToken = CancellationToken.None;

      _userServiceMock = new Mock<IUserService>();
      _userRoleServiceMock = new Mock<IUserRoleService>();

      _userStore = new UserStore(
        _userServiceMock.Object,
        _userRoleServiceMock.Object);
    }

    [TestMethod]
    public async Task GetUserIdAsync_Should_Return_Id()
    {
      var controlUserId = Guid.NewGuid();
      var userEntity = new UserEntity
      {
        Id = controlUserId,
      };

      var actualUserIdString =
        await _userStore.GetUserIdAsync(userEntity, _cancellationToken);

      Assert.IsNotNull(actualUserIdString);

      Guid actualUserId;
      Assert.IsTrue(Guid.TryParse(actualUserIdString, out actualUserId));

      Assert.AreEqual(controlUserId, actualUserId);

      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetUserNameAsync_Should_Return_Email()
    {
      var controlEmail = Guid.NewGuid().ToString();
      var userEntity = new UserEntity
      {
        Email = controlEmail,
      };

      var actualEmail =
        await _userStore.GetUserNameAsync(userEntity, _cancellationToken);

      Assert.IsNotNull(actualEmail);
      Assert.AreEqual(controlEmail, actualEmail);

      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task SetNormalizedUserNameAsync_Should_Update_Email()
    {
      var controlEmail = Guid.NewGuid().ToString();
      var userEntity = new UserEntity
      {
        Email = controlEmail,
      };

      var nomolizedUserName = Guid.NewGuid().ToString();

      await _userStore.SetNormalizedUserNameAsync(
        userEntity, nomolizedUserName, _cancellationToken);

      Assert.AreNotEqual(controlEmail, userEntity.Email);
      Assert.AreEqual(nomolizedUserName, userEntity.Email);

      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task CreateAsync_Should_Create_New_User()
    {
      _userServiceMock.Setup(repository => repository.AddUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask)
                      .Verifiable();

      var userEntity = new UserEntity();

      var identityResult =
        await _userStore.CreateAsync(userEntity, _cancellationToken);

      Assert.IsNotNull(identityResult);
      Assert.AreEqual(IdentityResult.Success, identityResult);

      _userServiceMock.Verify(repository => repository.AddUserAsync(userEntity, _cancellationToken));
      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task UpdateAsync_Should_Update_User()
    {
      _userServiceMock.Setup(repository => repository.UpdateUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                      .Returns(Task.CompletedTask)
                      .Verifiable();

      var userEntity = new UserEntity();

      var identityResult =
        await _userStore.UpdateAsync(userEntity, _cancellationToken);

      Assert.IsNotNull(identityResult);
      Assert.AreEqual(IdentityResult.Success, identityResult);

      _userServiceMock.Verify(repository => repository.UpdateUserAsync(userEntity, _cancellationToken));
      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task FindByIdAsync_Should_Return_User()
    {
      var controlUserEntity = new UserEntity();

      _userServiceMock.Setup(repository => repository.GetUserAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(controlUserEntity)
                      .Verifiable();

      var userId = Guid.NewGuid().ToString();

      var actualUserEntity =
        await _userStore.FindByIdAsync(userId, _cancellationToken);

      Assert.IsNotNull(actualUserEntity);
      Assert.AreEqual(controlUserEntity, actualUserEntity);

      _userServiceMock.Verify();
      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task FindByIdAsync_Should_Return_Null()
    {
      var userId = Guid.NewGuid().ToString() + "test";

      var actualUserEntity =
        await _userStore.FindByIdAsync(userId, _cancellationToken);

      Assert.IsNull(actualUserEntity);

      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task FindByNameAsync_Should_Get_User_By_Email()
    {
      var controlUserEntity = new UserEntity();

      _userServiceMock.Setup(repository => repository.GetUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(controlUserEntity)
                      .Verifiable();

      var username = Guid.NewGuid().ToString();

      var actualUserEntity =
        await _userStore.FindByNameAsync(username, _cancellationToken);

      Assert.IsNotNull(actualUserEntity);
      Assert.AreEqual(controlUserEntity, actualUserEntity);

      _userServiceMock.Verify(repository => repository.GetUserAsync(username, _cancellationToken));
      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task SetPasswordHashAsync_Should_Return_Password_Hash()
    {
      var userEntity = new UserEntity();
      var passwordHash = Guid.NewGuid().ToString();

      await _userStore.SetPasswordHashAsync(userEntity, passwordHash, _cancellationToken);

      Assert.AreEqual(passwordHash, userEntity.PasswordHash);

      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetPasswordHashAsync_Should_Return_Password_Hash()
    {
      var controlPasswordHash = Guid.NewGuid().ToString();
      var userEntity = new UserEntity
      {
        PasswordHash = controlPasswordHash,
      };

      var actualPasswordHash =
        await _userStore.GetPasswordHashAsync(userEntity, _cancellationToken);

      Assert.IsNotNull(actualPasswordHash);
      Assert.AreEqual(controlPasswordHash, actualPasswordHash);

      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetRolesAsync_Should_Return_Role_Collection_For_User()
    {
      var controlUserRole = Guid.NewGuid().ToString();
      var controlUserRoleEntityCollection = new List<string>
      {
        controlUserRole,
      };

      _userRoleServiceMock.Setup(repository => repository.GetRoleNamesAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(controlUserRoleEntityCollection)
                          .Verifiable();

      var userEntity = new UserEntity();
      var roleCollection = await _userStore.GetRolesAsync(userEntity, _cancellationToken);

      Assert.IsNotNull(roleCollection);
      Assert.AreEqual(controlUserRoleEntityCollection.Count, roleCollection.Count);
      Assert.IsTrue(controlUserRoleEntityCollection.All(role => roleCollection.Contains(role)));

      _userRoleServiceMock.Verify(repository => repository.GetRoleNamesAsync(userEntity, _cancellationToken));
      _userRoleServiceMock.VerifyNoOtherCalls();

      _userServiceMock.VerifyNoOtherCalls();
    }
  }
}
