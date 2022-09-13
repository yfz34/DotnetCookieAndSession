using Demo.Api;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<ISessionWrapper, SessionWrapper>();

// 
builder.Services.AddDistributedMemoryCache();

//
builder.Services.AddSession(options => 
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.Name = "mywebsite";
    options.IdleTimeout = TimeSpan.FromMinutes(5);
});

// cookie 登入驗證設定
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => 
    {
        options.Cookie.Name = "identityId"; // 預設名稱 .AspNetCore.Cookies
        options.LoginPath = new PathString("/api/Login/NoLogin");
    });

builder.Services.AddControllers(options => 
{
    // options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseCookiePolicy();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
