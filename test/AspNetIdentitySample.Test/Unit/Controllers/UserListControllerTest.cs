﻿// Copyright (c) Dennis Shevtsov. All rights reserved.
// Licensed under the MIT License.
// See LICENSE in the project root for license information.

namespace AspNetIdentitySample.WebApplication.Controllers.Test
{
  using System.Security.Claims;

  using AutoMapper;
  using Microsoft.AspNetCore.Identity;
  using Microsoft.AspNetCore.Mvc;

  using AspNetIdentitySample.ApplicationCore.Entities;
  using AspNetIdentitySample.ApplicationCore.Identities;
  using AspNetIdentitySample.ApplicationCore.Services;
  using AspNetIdentitySample.WebApplication.ViewModels;

  [TestClass]
  public sealed class UserListControllerTest : IdentityControllerTestBase
  {
    private CancellationToken _cancellationToken;

#pragma warning disable CS8618
    private Mock<IMapper> _mapperMock;

    private Mock<IUserService> _userServiceMock;

    private UserListController _userListController;
#pragma warning restore CS8618

    protected override void InitializeInternal()
    {
      _cancellationToken = CancellationToken.None;

      _mapperMock = new Mock<IMapper>();

      _userServiceMock = new Mock<IUserService>();

      _userListController = new UserListController(
        _mapperMock.Object, _userServiceMock.Object, UserManagerMock.Object);
    }

    [TestMethod]
    public async Task Get_Should_Return_View_Result_With_View_Model()
    {
      var userListViewModelCollection = new List<UserListViewModel.UserViewModel>();

      _mapperMock.Setup(mapper => mapper.Map<List<UserListViewModel.UserViewModel>>(It.IsAny<List<UserEntity>>()))
                 .Returns(userListViewModelCollection)
                 .Verifiable();

      var userEntityCollection = new List<UserEntity>();

      _userServiceMock.Setup(service => service.GetUsersAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(userEntityCollection)
                      .Verifiable();

      var viewModel = new UserListViewModel();

      var actionResult = await _userListController.Get(viewModel, _cancellationToken);

      Assert.IsNotNull(actionResult);

      var viewResult = actionResult as ViewResult;

      Assert.IsNotNull(viewResult);
      Assert.AreEqual(UserListController.ViewName, viewResult.ViewName);
      Assert.AreEqual(viewModel, viewResult.Model);
      Assert.AreEqual(userListViewModelCollection, viewModel.Users);

      _mapperMock.Verify(mapper => mapper.Map<List<UserListViewModel.UserViewModel>>(userEntityCollection));
      _mapperMock.VerifyNoOtherCalls();

      _userServiceMock.Verify(service => service.GetUsersAsync(_cancellationToken));
      _userServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public async Task Delete_Should_Delete_User()
    {
      var principal = new ClaimsPrincipal();

      _mapperMock.Setup(mapper => mapper.Map<ClaimsPrincipal>(It.IsAny<IUserIdentity>()))
                 .Returns(principal)
                 .Verifiable();

      var userEntity = new UserEntity();

      UserManagerMock.Setup(repository => repository.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                     .ReturnsAsync(userEntity)
                     .Verifiable();

      UserManagerMock.Setup(service => service.DeleteAsync(It.IsAny<UserEntity>()))
                     .ReturnsAsync(IdentityResult.Success)
                     .Verifiable();

      var viewModel = new DeleteAccountViewModel();

      var actionResult = await _userListController.Delete(viewModel);

      Assert.IsNotNull(actionResult);

      var redirectResult = actionResult as RedirectToActionResult;

      Assert.IsNotNull(redirectResult);
      Assert.AreEqual(nameof(UserListController.Get), redirectResult.ActionName);

      _mapperMock.Verify(mapper => mapper.Map<ClaimsPrincipal>(viewModel));
      _mapperMock.VerifyNoOtherCalls();

      UserManagerMock.Verify(manager => manager.GetUserAsync(principal));
      UserManagerMock.Verify(manager => manager.DeleteAsync(userEntity));
      UserManagerMock.VerifyNoOtherCalls();

      _userServiceMock.VerifyNoOtherCalls();
    }
  }
}
