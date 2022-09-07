namespace MsalAuthInMaui
{
    public class Settings
    {
        // Azure AD B2C Microsoft Authentication
        public string ClientId { get; set; } = null;
        public string TenantId { get; set; } = null;
        public string Authority { get; set; } = null;
        public NestedSettings[] Scopes { get; set; } = null;

        // Azure AD B2C Twitter Authentication
        public string ClientIdSocial { get; set; } = null;
        public string TenantSocial { get; set; } = null;
        public string TenantIdSocial { get; set; } = null;

        public string InstanceUrlSocial { get; set; } = null;
        public string PolicySignUpSignInSocial { get; set; } = null;
        public string AuthoritySocial { get; set; } = null;
        public NestedSettings[] ScopesSocial { get; set; } = null;
    }
}
