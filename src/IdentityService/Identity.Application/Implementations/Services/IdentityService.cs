using AutoMapper;
using Identity.Application.Abstractions.Providers;
using Identity.Application.Abstractions.Services;
using Identity.Application.Shared.Exceptions;
using Identity.Application.Shared.Models;
using Identity.Application.Shared.Models.Requests;
using Identity.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Implementations.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IMapper _mapper;
    private readonly IJwtProvider _jwtProvider;
    private readonly ICurrentUserProvider _currentUserProvider;

    public IdentityService(UserManager<User> userManager, 
                            SignInManager<User> signInManager, 
                            RoleManager<IdentityRole<Guid>> roleManager, 
                            IMapper mapper, IJwtProvider jwtProvider, 
                            ICurrentUserProvider currentUserProvider)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _jwtProvider = jwtProvider;
        _currentUserProvider = currentUserProvider;
    }

    public async Task RegistrationAsync(RegistrationUserRequest registrationRequest, CancellationToken cancellationToken)
    {
        var dbUser = await _userManager.FindByEmailAsync(registrationRequest.Email);

        if (dbUser is not null)
        {
            throw new BadRequestException($"User with email {registrationRequest.Email} already exists");
        }

        dbUser = await _userManager.FindByNameAsync(registrationRequest.Username);

        if (dbUser is not null)
        {
            throw new BadRequestException($"User with username {registrationRequest.Username} already exists");
        }

        User user = _mapper.Map<User>(registrationRequest);

        var result = await _userManager.CreateAsync(user, registrationRequest.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join('\n', result.Errors));
        }
    }

    public async Task<AccessToken> LoginAsync(LoginUserRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(loginRequest.Username, loginRequest.Password, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Invalid username or password");
        }

        var dbUser = await _userManager.FindByNameAsync(loginRequest.Username);

        if (dbUser is null)
        {
            throw new BadRequestException($"User with username {loginRequest.Username} not found");
        }

        var principals = await _signInManager.CreateUserPrincipalAsync(dbUser);

        string jwt = _jwtProvider.GenerateJwt(dbUser, principals.Claims);

        UpdateRefresh(dbUser);

        await _userManager.UpdateAsync(dbUser);

        return new()
        {
            JwtToken = jwt,
            RefreshToken = dbUser.RefreshToken!,
            RefreshTokenExpiry = dbUser.RefreshTokenExpiry!.Value,
        };
    }

    public async Task<AccessToken> ReefreshAsync(RefreshTokenRequest refreshRequest, CancellationToken cancellationToken = default)
    {
        var user = _currentUserProvider.GetCurrentUser(refreshRequest.Jwt);

        if (user is null)
        {
            throw new UnauthorizedException("User is not authenticated");
        }

        var dbUser = await _userManager.FindByEmailAsync(user.Email);

        if (dbUser is null || dbUser.Email != user.Email || dbUser.Id != user.Id)
        {
            throw new BadRequestException("Invalid user for refresh");
        }

        if (dbUser.RefreshTokenExpiry is null 
            || dbUser.RefreshTokenExpiry <= DateTime.UtcNow 
            || dbUser.RefreshToken is null)
        {
            throw new BadRequestException("Invalid refresh token");
        }

        var principals = await _signInManager.CreateUserPrincipalAsync(dbUser);

        string jwt = _jwtProvider.GenerateJwt(dbUser, principals.Claims);

        return new()
        {
            JwtToken = jwt,
            RefreshToken = dbUser.RefreshToken!,
            RefreshTokenExpiry = dbUser.RefreshTokenExpiry!.Value,
        };

    }

    public async Task AddUserToRoleAsync(AddUserToRoleRequest userToRoleRequest, CancellationToken cancellationToken = default)
    {
        var dbUser = await _userManager.FindByNameAsync(userToRoleRequest.Username);

        if (dbUser is null)
        {
            throw new BadRequestException($"User with username {userToRoleRequest.Username} was not found");
        }

        var role = await _roleManager.FindByNameAsync(userToRoleRequest.RoleName);

        var roles = _roleManager.Roles.ToArray();

        if (role is null)
        {
            throw new BadRequestException($"Role with name {userToRoleRequest.RoleName} was not found");
        }

        var result = await _userManager.AddToRoleAsync(dbUser, role.Name!);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join('\n', result.Errors));
        }
    }

    private void UpdateRefresh(User user)
    {
        string refresh = _jwtProvider.GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(1);
    }
}
