using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LMS.Data;
using LMS.Models;
using Stripe;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<LMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LMSContext") ?? throw new InvalidOperationException("Connection string 'LMSContext' not found.")));

builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
    {
        options.Cookie.Name = "MyCookieAuth";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

StripeConfiguration.ApiKey = "sk_test_51OgsLhBWhaZwsKbkQ9PFD9TcRLRegBy0IwKVAgcaOO95woY0ex8AfjpfeTFb7U2lplBFTfaTdBd6E9fUnkhdSkFx00zqzkXRLE";

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
