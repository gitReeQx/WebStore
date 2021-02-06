using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.ServiceHosting.Controllers.Identity
{
    [Route(WebAPI.Identity.User)]
    [ApiController]
    public class UsersApiController : ControllerBase
    {
        private readonly UserStore<User, Role, WebStoreDB> _UserStore;

        public UsersApiController(WebStoreDB db)
        {
            _UserStore = new UserStore<User, Role, WebStoreDB>(db);
        }

        [HttpGet("all")]
        public async Task<IEnumerable<User>> GetAllUsers() => await _UserStore.Users.ToArrayAsync();

        #region Users
        [HttpPost("userId")]
        public async Task<string> GetUserIdAsync([FromBody] User user) => await _UserStore.GetUserIdAsync(user);

        [HttpPost("userName")]
        public async Task<string> GetUserNameAsync([FromBody] User user) => await _UserStore.GetUserNameAsync(user);

        [HttpPost("userName/{userName}")]
        public async Task<string> SetUserNameAsync([FromBody] User user, string userName)
        {
            await _UserStore.SetUserNameAsync(user, userName);
            await _UserStore.UpdateAsync(user);
            return user.UserName;
        }

        [HttpPost("normalUserName")]
        public async Task<string> GetNormalizedUserNameAsync([FromBody]User user)
        {
            var result = await _UserStore.GetNormalizedUserNameAsync(user);
            return result;
        }

        [HttpPost("normalUserName/{normalizedName}")]
        public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string normalizedName)
        {
            await _UserStore.SetNormalizedUserNameAsync(user, normalizedName);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedUserName;
        }

        [HttpPost("user")]
        public async Task<bool> CreateAsync([FromBody] User user)
        {
            var result = await _UserStore.CreateAsync(user);
            return result.Succeeded;
        }

        [HttpPut("user")]
        public async Task<bool> UpdateAsync([FromBody] User user)
        {
            var result = await _UserStore.UpdateAsync(user);
            return result.Succeeded;
        }

        [HttpPost("user/delete")]
        public async Task<bool> DeleteAsync([FromBody] User user)
        {
            var result = await _UserStore.DeleteAsync(user);
            return result.Succeeded;
        }

        [HttpGet("user/find/{userId}")]
        public async Task<User> FindByIdAsync(string userId)
        {
            var result = await _UserStore.FindByIdAsync(userId);
            return result;
        }
        [HttpGet("user/normal/{normalizedUserName}")]
        public async Task<User> FindByNameAsync(string normalizedUserName)
        {
            var result = await _UserStore.FindByNameAsync(normalizedUserName);
            return result;
        }

        [HttpPost("role/{roleName}")]
        public async Task AddToRoleAsync([FromBody] User user, string roleName, [FromServices] WebStoreDB db)
        {
            await _UserStore.AddToRoleAsync(user, roleName);
            await db.SaveChangesAsync();
        }

        [HttpPost("role/delete/{roleName}")]
        public async Task RemoveFromRoleAsync([FromBody] User user, string roleName) => await _UserStore.RemoveFromRoleAsync(user, roleName);

        [HttpPost("roles")]
        public async Task<IList<string>> GetRolesAsync([FromBody] User user) => await _UserStore.GetRolesAsync(user);

        [HttpPost("inrole/{roleName}")]
        public async Task<bool> IsInRoleAsync([FromBody] User user, string roleName) => await _UserStore.IsInRoleAsync(user, roleName);

        [HttpGet("usersInRole/{roleName}")]
        public async Task<IList<User>> GetUsersInRoleAsync(string roleName) => await _UserStore.GetUsersInRoleAsync(roleName);

        [HttpPost("setPasswordHash")]
        public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDTO hashDto)
        {
            await _UserStore.SetPasswordHashAsync(hashDto.User, hashDto.Hash);
            await _UserStore.UpdateAsync(hashDto.User);
            return hashDto.User.PasswordHash;
        }

        [HttpPost("getPasswordHash")]
        public async Task<string> GetPasswordHashAsync([FromBody] User user)
        {
            var result = await _UserStore.GetPasswordHashAsync(user);
            return result;
        }

        [HttpPost("hasPassword")]
        public async Task<bool> HasPasswordAsync([FromBody] User user) => await _UserStore.HasPasswordAsync(user);

        #endregion
        #region Claims
        [HttpPost("getClaims")]
        public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user) => await _UserStore.GetClaimsAsync(user);

        [HttpPost("addClaims")]
        public async Task AddClaimsAsync([FromBody] AddClaimDTO claimsDto, [FromServices] WebStoreDB db)
        {
            await _UserStore.AddClaimsAsync(claimsDto.User, claimsDto.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost("replaceClaim")]
        public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDTO claimDTO, [FromServices] WebStoreDB db)
        {
            await _UserStore.ReplaceClaimAsync(claimDTO.User, claimDTO.Claim, claimDTO.NewClaim);
            await db.SaveChangesAsync();
        }

        [HttpPost("removeClaims")]
        public async Task RemoveClaimsAsync([FromBody] RemoveClaimDTO claimsDto, [FromServices] WebStoreDB db)
        {
            await _UserStore.RemoveClaimsAsync(claimsDto.User, claimsDto.Claims);
            await db.SaveChangesAsync();
        }

        [HttpPost("getUsersForClaim")]
        public async Task<IList<User>> GetUsersForClaimAsync([FromBody]Claim claim)
        {
            return await _UserStore.GetUsersForClaimAsync(claim);
        }

        #endregion
        #region TwoFactor
        [HttpPost("setTwoFactor/{enabled}")]
        public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enabled)
        {
            await _UserStore.SetTwoFactorEnabledAsync(user, enabled);
            await _UserStore.UpdateAsync(user);
            return user.TwoFactorEnabled;
        }

        [HttpPost("getTwoFactorEnabled")]
        public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user) => await _UserStore.GetTwoFactorEnabledAsync(user);

        #endregion
        #region Email/Phone
        [HttpPost("setEmail/{email}")]
        public async Task<string> SetEmailAsync([FromBody] User user, string email)
        {
            await _UserStore.SetEmailAsync(user, email);
            await _UserStore.UpdateAsync(user);
            return user.Email;
        }

        [HttpPost("getEmail")]
        public async Task<string> GetEmailAsync([FromBody] User user) => await _UserStore.GetEmailAsync(user);

        [HttpPost("getEmailConfirmed")]
        public async Task<bool> GetEmailConfirmedAsync([FromBody] User user) => await _UserStore.GetEmailConfirmedAsync(user);

        [HttpPost("setEmailConfirmed/{confirmed}")]
        public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _UserStore.SetEmailConfirmedAsync(user, confirmed);
            await _UserStore.UpdateAsync(user);
            return user.EmailConfirmed;
        }

        [HttpGet("user/findByEmail/{normalizedEmail}")]
        public async Task<User> FindByEmailAsync(string normalizedEmail)
        {
            return await _UserStore.FindByEmailAsync(normalizedEmail);
        }

        [HttpPost("getNormalizedEmail")]
        public async Task<string> GetNormalizedEmailAsync([FromBody] User user) => await _UserStore.GetNormalizedEmailAsync(user);

        [HttpPost("setEmail/{normalizedEmail}")]
        public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string normalizedEmail)
        {
            await _UserStore.SetNormalizedEmailAsync(user, normalizedEmail);
            await _UserStore.UpdateAsync(user);
            return user.NormalizedEmail;
        }

        [HttpPost("setPhoneNumber/{phoneNumber}")]
        public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phoneNumber)
        {
            await _UserStore.SetPhoneNumberAsync(user, phoneNumber);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumber;
        }

        [HttpPost("getPhoneNumber")]
        public async Task<string> GetPhoneNumberAsync([FromBody] User user) => await _UserStore.GetPhoneNumberAsync(user);

        [HttpPost("getPhoneNumberConfirmed")]
        public async Task<bool> GetPhoneNumberConfirmedAsync([FromBody] User user) => await _UserStore.GetPhoneNumberConfirmedAsync(user);

        [HttpPost("setPhoneNumberConfirmed/{confirmed}")]
        public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
        {
            await _UserStore.SetPhoneNumberConfirmedAsync(user, confirmed);
            await _UserStore.UpdateAsync(user);
            return user.PhoneNumberConfirmed;
        }

        #endregion
        #region Login/Lockout
        [HttpPost("addLogin")]
        public async Task AddLoginAsync([FromBody] AddLoginDTO loginDto, [FromServices] WebStoreDB db)
        {
            await _UserStore.AddLoginAsync(loginDto.User, loginDto.UserLoginInfo);
            await db.SaveChangesAsync();
        }

        [HttpPost("removeLogin/{loginProvider}/{providerKey}")]
        public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey, [FromServices] WebStoreDB db)
        {
            await _UserStore.RemoveLoginAsync(user, loginProvider, providerKey);
            await db.SaveChangesAsync();
        }

        [HttpPost("getLogins")]
        public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user) => await _UserStore.GetLoginsAsync(user);

        [HttpGet("user/findbylogin/{loginProvider}/{providerKey}")]
        public async Task<User> FindByLoginAsync(string loginProvider, string providerKey) => await _UserStore.FindByLoginAsync(loginProvider, providerKey);

        [HttpPost("getLockoutEndDate")]
        public async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user) => await _UserStore.GetLockoutEndDateAsync(user);

        [HttpPost("setLockoutEndDate")]
        public async Task<DateTimeOffset?> SetLockoutEndDateAsync(SetLockoutDTO setLockoutDto)
        {
            await _UserStore.SetLockoutEndDateAsync(setLockoutDto.User, setLockoutDto.LockoutEnd);
            await _UserStore.UpdateAsync(setLockoutDto.User);
            return setLockoutDto.User.LockoutEnd;
        }

        [HttpPost("IncrementAccessFailedCount")]
        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            var count = await _UserStore.IncrementAccessFailedCountAsync(user);
            await _UserStore.UpdateAsync(user);
            return count;
        }

        [HttpPost("ResetAccessFailedCount")]
        public async Task<int> ResetAccessFailedCountAsync(User user)
        {
            await _UserStore.ResetAccessFailedCountAsync(user);
            await _UserStore.UpdateAsync(user);
            return user.AccessFailedCount;
        }

        [HttpPost("GetAccessFailedCount")]
        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            return await _UserStore.GetAccessFailedCountAsync(user);
        }

        [HttpPost("GetLockoutEnabled")]
        public async Task<bool> GetLockoutEnabledAsync(User user) => await _UserStore.GetLockoutEnabledAsync(user);

        [HttpPost("SetLockoutEnabled/{enabled}")]
        public async Task<bool> SetLockoutEnabledAsync(User user, bool enabled)
        {
            await _UserStore.SetLockoutEnabledAsync(user, enabled);
            await _UserStore.UpdateAsync(user);
            return user.LockoutEnabled;
        }
        #endregion

    }
}
