// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace Kontravers.GoodJob.Auth.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IClock _clock;

        public ConfirmEmailModel(UserManager<IdentityUser> userManager,
            IEventPublisher eventPublisher, IClock clock)
        {
            _userManager = userManager;
            _eventPublisher = eventPublisher;
            _clock = clock;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            if (!result.Succeeded) return Page();
            
            var userAccountConfirmedEvent = new UserAccountConfirmedEvent(_clock.UtcNow, user.Id);
            await _eventPublisher.PublishAsync(userAccountConfirmedEvent, default);
            return Page();
        }
    }
}
