using SalesManagerSolution.WebApi.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureAppServices(builder.Configuration, builder.Environment.IsProduction());

builder.Services.AddControllers();
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

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
