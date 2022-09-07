// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using static System.Formats.Asn1.AsnWriter;

namespace MsalAuthInMaui
{
    /// <summary>
    /// This is a wrapper for PCA. It is singleton and can be utilized by both application and the MAM callback
    /// </summary>
    public class PCAWrapper : IPCAWrapper
    {
        private IConfiguration _configuration;
        private Settings _settings { get; set; }

        internal IPublicClientApplication PCA { get; }

        internal bool UseEmbedded { get; set; } = false;
        public string[] Scopes { get; set; }

        // public constructor
        public PCAWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = _configuration.GetRequiredSection("Settings").Get<Settings>();
            Scopes = _settings.Scopes.ToStringArray();

            // Create PCA once. Make sure that all the config parameters below are passed
            PCA = PublicClientApplicationBuilder
                                        .Create(_settings.ClientId)
                                        .WithRedirectUri(PlatformConfig.Instance.RedirectUri)
                                        .WithIosKeychainSecurityGroup("com.microsoft.adalcache")
                                        .Build();
        }

        /// <summary>
        /// Acquire the token silently
        /// </summary>
        /// <param name="scopes">desired scopes</param>
        /// <returns>Authentication result</returns>
        public async Task<AuthenticationResult> AcquireTokenSilentAsync(string[] scopes)
        {
            var accounts = await PCA.GetAccountsAsync().ConfigureAwait(false);
            var account = accounts.FirstOrDefault();

            var authResult = await PCA.AcquireTokenSilent(scopes, account)
                                        .ExecuteAsync().ConfigureAwait(false);
            return authResult;

        }

        /// <summary>
        /// Perform the interactive acquisition of the token for the given scope
        /// </summary>
        /// <param name="scopes">desired scopes</param>
        /// <returns></returns>
        public async Task<AuthenticationResult> AcquireTokenInteractiveAsync(string[] scopes)
        {
            var systemWebViewOptions = new SystemWebViewOptions();
#if IOS
            // embedded view is not supported on Android
            if (UseEmbedded)
            {

                return await PCA.AcquireTokenInteractive(scopes)
                                        .WithUseEmbeddedWebView(true)
                                        .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                                        .ExecuteAsync()
                                        .ConfigureAwait(false);
            }

            // Hide the privacy prompt in iOS
            systemWebViewOptions.iOSHidePrivacyPrompt = true;
#endif

            return await PCA.AcquireTokenInteractive(scopes)
                                    .WithAuthority(_settings.Authority)
                                    .WithTenantId(_settings.TenantId)
                                    .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                                    .WithUseEmbeddedWebView(true)
                                    .ExecuteAsync()
                                    .ConfigureAwait(false);
        }

        /// <summary>
        /// Signout may not perform the complete signout as company portal may hold
        /// the token.
        /// </summary>
        /// <returns></returns>
        public async Task SignOutAsync()
        {
            var accounts = await PCA.GetAccountsAsync().ConfigureAwait(false);
            foreach (var acct in accounts)
            {
                await PCA.RemoveAsync(acct).ConfigureAwait(false);
            }
        }
    }
}
