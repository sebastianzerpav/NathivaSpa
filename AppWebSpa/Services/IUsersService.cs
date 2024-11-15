﻿using Microsoft.AspNetCore.Identity;
using AppWebSpa.Data.Entities;
using AppWebSpa.DTOs;
using AppWebSpa.Data;
using Microsoft.EntityFrameworkCore;
using AppWebSpa.Core;
using AppWebSpa.Core.Pagination;
using AppWebSpa.Helpers;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Azure.Core;

namespace AppWebSpa.Services
{
    public interface IUsersService
    {
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        public Task<Response<User>> CreateAsync(UserDTO dto);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        public Task<Response<PaginationResponse<User>>> GetListAsync(PaginationRequest request);
        public Task<User> GetUserAsync(string email);
        public Task<User> GetUserAsync(Guid id);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<IdentityResult> UpdateUserAsync(User user);
        public Task<Response<User>> UpdateUserAsync(UserDTO dto);
    }

    public class UsersService : IUsersService
    {
        private readonly DataContext _context;
        private readonly SignInManager<User> _signInManager;
        //clase de manejo de usarios de Identity framework
        private readonly UserManager<User> _userManager;
        private readonly IConverterHelper _converterHelper;

        public UsersService(DataContext context, SignInManager<User> signInManager, UserManager<User> userManager, IConverterHelper converterHelper)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _converterHelper = converterHelper;
        }
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<Response<User>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = _converterHelper.ToUser(dto);
                //Generacion de Guid aleatorio
                Guid id= Guid.NewGuid();
                user.Id.ToString();

                IdentityResult result = await AddUserAsync(user, dto.Document);

                //autovalidacion del usuario generando token
                //TODO: Ajustar cuando se realice funcionalidad para envio de email
                string token = await GenerateEmailConfirmationTokenAsync(user);
                await ConfirmEmailAsync(user, token);

                return ResponseHelper<User>.MakeResponseSuccess(user, "Usuario creado con exito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }

        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<Response<PaginationResponse<User>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<User> query = _context.User.AsQueryable().Include(u => u.NathivaRole);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower())
                                             || s.Email.ToLower().Contains(request.Filter.ToLower())
                                             || s.PhoneNumber.ToLower().Contains(request.Filter.ToLower())
                                             || s.Document.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<User> list = await PagedList<User>.ToPagedListAsync(query, request);

                PaginationResponse<User> result = new PaginationResponse<User>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,

                };

                return ResponseHelper<PaginationResponse<User>>.MakeResponseSuccess(result, "Usuarios obtenidos con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<User>>.MakeResponseFail(ex);
            }
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user =
                await _context.User.Include(u=> u.NathivaRole)
                                   .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _context.User.Include(u => u.NathivaRole)
                                   .FirstOrDefaultAsync(u => u.Id == id.ToString());

            
        }

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        }

        
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<Response<User>> UpdateUserAsync(UserDTO dto)
        {
            try
            {
                User user = await GetUserAsync(dto.Id);
                user.Document=dto.Document;
                user.Name = dto.Name;
                user.PhoneNumber = dto.PhoneNumber;
                user.BirthDate = dto.BirthDate;
                user.NathivaRoleId = dto.NathivaRoleId;

                _context.User.Update(user);
                await _context.SaveChangesAsync();

                return ResponseHelper<User>.MakeResponseSuccess(user, "usuario actualizado con exito");

            }
            catch (Exception ex)
            {
                return ResponseHelper<User>.MakeResponseFail(ex);
            }
        }

    }
}