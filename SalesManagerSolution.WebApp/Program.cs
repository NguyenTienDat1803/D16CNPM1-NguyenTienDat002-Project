using Microsoft.AspNetCore.Authentication.Cookies;
using SalesManagerSolution.WebApp.DI;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSession(options =>
{
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.ConfigureAppServices(builder.Configuration, builder.Environment.IsProduction());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

//app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
