using System.IdentityModel.Tokens.Jwt;

namespace WebClient;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5001";

                options.ClientId = "web";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");

                options.SaveTokens = true;
            });
        return builder.Build();
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();
        
        return app;
    }
}