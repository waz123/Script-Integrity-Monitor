using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Security_App__Blazor.Data.Services;
using DBA;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<ScriptCheckerService>();
builder.Services.AddScoped<FileService>();
builder.Services.AddSingleton<ScriptResultService>();
builder.Services.AddHostedService<ScriptCheckerBackgroundService>();
builder.Services.AddSingleton<SharedResultService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddHttpClient();

//Adding Entity Framework Core
builder.Services.AddDbContext<DbaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

//using var cts = new CancellationTokenSource();
//app.Services.GetRequiredService<ScriptCheckerBackgroundService>().StartAsync(cts.Token);

app.Run();
