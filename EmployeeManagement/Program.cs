using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

//logging
builder.Logging.AddNLog();

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContextPool<AppDbContext>(options => 
        options.UseSqlServer(configuration.GetConnectionString("EmployeeDBConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 2;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddMvc(Options => { Options.EnableEndpointRouting = false;
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    Options.Filters.Add(new AuthorizeFilter(policy));   
});
builder.Services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
var app = builder.Build();

//app.MapGet("/", () => configuration["MyKey"]);

/*app.Use(async (context,next) =>
{
    app.Logger.LogInformation("MW1: Incoming request");
    await next();
    app.Logger.LogInformation("MW1: Outgoing Response");
});
app.Use(async (context,next) =>
{
    app.Logger.LogInformation("MW2: Incoming request");
    await next();
    app.Logger.LogInformation("MW2: Outgoing Response");
})
app.Run(async context =>
{
    //throw new Exception("Some error processing the request");
    await context.Response.WriteAsync("Hello World");
    app.Logger.LogInformation("MW3: Request handled and response produced");
});

FileServerOptions options = new FileServerOptions();
options.DefaultFilesOptions.DefaultFileNames.Clear();
options.DefaultFilesOptions.DefaultFileNames.Add("food.html");*/






if (app.Environment.IsDevelopment()) { 
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
}
app.UseStaticFiles();
app.UseAuthentication();
//app.UseMvcWithDefaultRoute();
app.UseMvc(routes =>
{
    routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
});
app.Run();
