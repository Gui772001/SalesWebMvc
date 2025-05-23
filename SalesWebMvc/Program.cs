﻿using SalesWebMvc.Data;
using SalesWebMvc.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("SalesWebMvcContext");
  
    // Usar o MySQL com a string de conexão
    options.UseMySql(connectionString,
        new MySqlServerVersion(new Version(8, 0, 13)),
        mysqlOptions => mysqlOptions.MigrationsAssembly("SalesWebMvc"));
});

builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SelleService>();
builder.Services.AddScoped<SalesRecordService>();
builder.Services.AddScoped<DepartmentService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();
var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();
        seedingService.Seed();  // Seeding dos dados caso necessário
    }
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();