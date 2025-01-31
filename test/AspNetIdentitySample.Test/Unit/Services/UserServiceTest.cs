﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.ApplicationCore.Services.Test
{
  using AspNetIdentitySample.ApplicationCore.Entities;
  using AspNetIdentitySample.ApplicationCore.Identities;
  using AspNetIdentitySample.ApplicationCore.Repositories;

  [TestClass]
  public sealed class UserServiceTest
  {
    private CancellationToken _cancellationToken;

#pragma warning disable CS8618
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserRoleService> _userRoleServiceMock;

    private UserService _userService;
#pragma warning restore CS8618

    [TestInitialize]
    public void Initialize()
    {
      _cancellationToken = CancellationToken.None;

      _userRepositoryMock = new Mock<IUserRepository>();
      _userRoleServiceMock = new Mock<IUserRoleService>();

      _userService = new UserService(
        _userRepositoryMock.Object,
        _userRoleServiceMock.Object);
    }

    [TestMethod]
    public async Task GetUsersAsync_Should_Return_Users_With_Roles()
    {
      var controlUserEntity = new UserEntity
      {
        UserId = Guid.NewGuid(),
      };

      var controlUserEntityCollection = new List<UserEntity>
      {
        new UserEntity(),
        controlUserEntity,
      };

      _userRepositoryMock.Setup(repository => repository.GetUsersAsync(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(controlUserEntityCollection)
                         .Verifiable();

      var controlUserRoleEntityCollection = new List<UserRoleEntity>
      {
        new UserRoleEntity(),
      };

      var controlUserRoleEntityDictionary = new Dictionary<IUserIdentity, List<UserRoleEntity>>(new UserRoleService.UserIdentityComparer())
      {
        { controlUserEntity, controlUserRoleEntityCollection },
      };

      _userRoleServiceMock.Setup(repository => repository.GetRolesAsync(It.IsAny<IEnumerable<IUserIdentity>>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(controlUserRoleEntityDictionary)
                             .Verifiable();

      var actualUserEntityCollection = await _userService.GetUsersAsync(_cancellationToken);

      Assert.IsNotNull(actualUserEntityCollection);
      Assert.AreEqual(controlUserEntityCollection, actualUserEntityCollection);
      Assert.AreEqual(controlUserEntity.Roles, controlUserRoleEntityCollection);

      _userRepositoryMock.Verify(repository => repository.GetUsersAsync(_cancellationToken));
      _userRepositoryMock.VerifyNoOtherCalls();

      _userRoleServiceMock.Verify(repository => repository.GetRolesAsync(controlUserEntityCollection, _cancellationToken));
      _userRoleServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetUserAsync_Should_Return_Null()
    {
      _userRepositoryMock.Setup(repository => repository.GetUserAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(default(UserEntity))
                         .Verifiable();

      var userIdentity = new UserEntity();

      var actualUserEntity = await _userService.GetUserAsync(userIdentity, _cancellationToken);

      Assert.IsNull(actualUserEntity);

      _userRepositoryMock.Verify(repository => repository.GetUserAsync(userIdentity, _cancellationToken));
      _userRepositoryMock.VerifyNoOtherCalls();

      _userRoleServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task GetUserAsync_Should_Return_User_With_Roles()
    {
      var controlUserEntity = new UserEntity
      {
        UserId = Guid.NewGuid(),
      };

      _userRepositoryMock.Setup(repository => repository.GetUserAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(controlUserEntity)
                         .Verifiable();

      var controlUserRoleEntityCollection = new List<UserRoleEntity>();

      _userRoleServiceMock.Setup(repository => repository.GetRolesAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(controlUserRoleEntityCollection)
                             .Verifiable();

      var userIdentity = controlUserEntity;

      var actualUserEntity = await _userService.GetUserAsync(userIdentity, _cancellationToken);

      Assert.IsNotNull(actualUserEntity);
      Assert.AreEqual(actualUserEntity, controlUserEntity);
      Assert.AreEqual(actualUserEntity.Roles, controlUserRoleEntityCollection);

      _userRepositoryMock.Verify(repository => repository.GetUserAsync(userIdentity, _cancellationToken));
      _userRepositoryMock.VerifyNoOtherCalls();

      _userRoleServiceMock.Verify(repository => repository.GetRolesAsync(userIdentity, _cancellationToken));
      _userRoleServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task AddUserAsync_Should_Create_New_User()
    {
      _userRepositoryMock.Setup(repository => repository.AddUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask)
                         .Verifiable();

      var userEntity = new UserEntity();

      await _userService.AddUserAsync(userEntity, _cancellationToken);

      _userRepositoryMock.Verify(repository => repository.AddUserAsync(userEntity, _cancellationToken));
      _userRepositoryMock.VerifyNoOtherCalls();

      _userRoleServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task UpdateUserAsync_Should_Update_User()
    {
      _userRepositoryMock.Setup(repository => repository.UpdateUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask)
                         .Verifiable();

      var userEntity = new UserEntity();

      await _userService.UpdateUserAsync(userEntity, _cancellationToken);

      _userRepositoryMock.Verify(repository => repository.UpdateUserAsync(userEntity, _cancellationToken));
      _userRepositoryMock.VerifyNoOtherCalls();

      _userRoleServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task DeleteUserAsync_Should_Update_User()
    {
      var controlUserEntity = new UserEntity();

      _userRepositoryMock.Setup(repository => repository.GetUserAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(controlUserEntity)
                         .Verifiable();

      _userRepositoryMock.Setup(repository => repository.DeleteUserAsync(It.IsAny<UserEntity>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.CompletedTask)
                         .Verifiable();

      var userRoleEntityCollection = new List<UserRoleEntity>();

      _userRoleServiceMock.Setup(repository => repository.DeleteRolesAsync(It.IsAny<IUserIdentity>(), It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable();

      var userIdentity = new UserRoleEntity();

      await _userService.DeleteUserAsync(userIdentity, _cancellationToken);

      _userRepositoryMock.Verify(repository => repository.GetUserAsync(userIdentity, _cancellationToken));
      _userRepositoryMock.Verify(repository => repository.DeleteUserAsync(controlUserEntity, _cancellationToken));
      _userRepositoryMock.VerifyNoOtherCalls();

      _userRoleServiceMock.Verify(service => service.DeleteRolesAsync(userIdentity, _cancellationToken));
      _userRoleServiceMock.VerifyNoOtherCalls();
    }
  }
}
