using Microsoft.EntityFrameworkCore;
using SearchAggregator;
using SearchAggregator.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "Frontend/dist";
});

string RPServerConnection = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<SearchContext>(options => options.UseSqlServer(RPServerConnection));

builder.Services.AddScoped<ISearchContextRepository, SearchContextRepository>();

var app = builder.Build();

app.UseStaticFiles();

if (!builder.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseSpa(spa =>
{
    spa.Options.SourcePath = "Frontend";

    if (builder.Environment.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
    }
});

app.MapControllers();

app.Run();