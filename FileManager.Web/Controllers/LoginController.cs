﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using FileManager.Application.Services.Interface;
using FileManager.Web.Manager.Interfaces;
using FileManager.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Dto;

namespace FileManager.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IUserService _userService;
        private readonly INotyfService _notyfService;

        public LoginController(IAuthenticationManager authenticationManager, IUserService userService,
            INotyfService notyfService)
        {
            _authenticationManager = authenticationManager;
            _userService = userService;
            _notyfService = notyfService;
        }

        public IActionResult Index() => View(new LoginVm());

        [HttpPost]
        public async Task<IActionResult> Index(LoginVm vm)
        {
            try
            {
                var result = await _authenticationManager.Login(vm.Email, vm.Password);
                if (result.Success) return RedirectToAction("Index", "Home");
                ModelState.AddModelError(nameof(vm.Password), result.Errors.FirstOrDefault()!);
                vm.Password = "";
                return View(vm);
            }
            catch (Exception e)
            {
                _notyfService.Error(e.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register() => View(new UserVm());

        [HttpPost]
        public async Task<IActionResult> Register(UserVm vm)
        {
            try
            {
                var userDto = new UserDto(vm.Name, vm.Gender, vm.Email, vm.Password, vm.Address, vm.Phone);
                var user = await _userService.CreateUser(userDto);
                _notyfService.Success("Success");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _notyfService.Error(e.Message);
                return View();
            }
        }
    }
}