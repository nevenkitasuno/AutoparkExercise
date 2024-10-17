using Autopark.API.Data;
using Autopark.API.Entities;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews(); // AddControllers - without Anti-Forgery
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer(); // don't need this because I use controllers
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
    .AddCookie(IdentityConstants.ApplicationScheme)
    .AddBearerToken(IdentityConstants.BearerScheme);


builder.Services.AddIdentityCore<Manager>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AutoparkDbContext>()
    .AddApiEndpoints();

builder.Services.ConfigureApplicationCookie(options =>
{
    // options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.HttpOnly = true;
    // options.LoginPath = "/identity/login";
    //using Microsoft.AspNetCore.Authentication.Cookies;
    // options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.Cookie.Domain = "localhost";
    // options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Events.OnRedirectToAccessDenied =
        options.Events.OnRedirectToLogin = c =>
        {
            if (c.Request.Path.StartsWithSegments("/api")
                && c.Response.StatusCode == StatusCodes.Status200OK)
            {
                c.Response.Headers.Location = c.RedirectUri;
                c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.FromResult<object>(null);
            }
            c.Response.Redirect(c.RedirectUri);
            return Task.FromResult<object>(null);
        };
});

builder.Services.AddDbContext<AutoparkDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AutoparkDbContext)),
    o => o.UseNetTopologySuite()));

builder.Services.AddAntiforgery(options =>
{
    // Set Cookie properties using CookieBuilder properties†.
    options.FormFieldName = "AntiforgeryFieldname";
    options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
    options.SuppressXFrameOptionsHeader = false;
});

var app = builder.Build();

app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseAuthentication();
app.UseAuthorization();

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value;

    var tokenSet = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
        new CookieOptions { HttpOnly = false });

    return next(context);
});

app.MapGroup("/identity").MapIdentityApi<Manager>();

// app.MapPost("/identity/logout", async (SignInManager<Manager> signInManager) => {
//     await signInManager.SignOutAsync();
//     return Results.Ok();
// }).RequireAuthorization();
// TODO: 404
// TODO: pingauth

app.MapControllers();

// app.UseHttpsRedirection();

await app.RunAsync();