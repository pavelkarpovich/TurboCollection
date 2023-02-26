using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurboCollection.ApplicationCore.Entities;
using TurboCollection.Infrastructure;
using TurboCollection.Infrastructure.Data;
using TurboCollection.Web.Interfaces;
using TurboCollection.Web.Services;

var builder = WebApplication.CreateBuilder(args);

Dependencies.ConfigureServices(builder.Configuration, builder.Services);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
//});

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddIdentity<Account, IdentityRole>().AddEntityFrameworkStores<AccountDbContext>();

builder.Services.AddAuthorization();
builder.Services.AddScoped<ICollectionViewModelService, CollectionViewModelServicecs>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var applicationContext = scopedProvider.GetRequiredService<ApplicationDbContext>();
        if (applicationContext.Database.IsSqlServer())
        {
            applicationContext.Database.Migrate();
        }
        await ApplicationDbContextSeed.SeedAsync(applicationContext);

        var accountContext = scopedProvider.GetRequiredService<AccountDbContext>();
        if (accountContext.Database.IsSqlServer())
        {
            accountContext.Database.Migrate();
        }
        //await AccountDbContext.SeedAsync(accountContext);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred adding migration to Database");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();//Identity
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
