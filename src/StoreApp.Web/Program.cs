using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Application;
using StoreApp.Data.Persistence.Context;
using StoreApp.Data.Persistence.SeedData;
using StoreApp.Web;
using StoreApp.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddDataServices(builder.Configuration);
builder.AddWebConfigureServices(builder.Configuration);

var app = builder.Build();
//app.UseDeveloperExceptionPage();

app.AddWebAppService();

await app.RunAsync();
