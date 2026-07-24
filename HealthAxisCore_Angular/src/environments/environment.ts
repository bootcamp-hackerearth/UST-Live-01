export const environment = {
  production: true,

  // Angular and ASP.NET Core are hosted by the same application.
  // AuthController uses [Route("api/auth")], so AuthService will
  // produce URLs such as /api/auth/login.
  apiBaseUrl: '/api',

  // The Blazor application is hosted under /blazor in Program.cs.
  // A relative URL automatically uses the current protocol and host.
  adminAppUrl: '/blazor/external-login'
};
