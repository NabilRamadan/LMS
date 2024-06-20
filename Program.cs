using CRUDApi.Context;
using CRUDApi.Data;
using CRUDApi.Extensions;
using CRUDApi.Interfaces;
using CRUDApi.Middleware;
using CRUDApi.Services;
using CRUDApi.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LMSContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(config.GetConnectionString("IdentityConnection")));
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddControllers();
builder.Services.AddDirectoryBrowser();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var validationConfigs = new List<EndpointValidationConfig>
        {
            new EndpointValidationConfig { EndpointPath = "/api/Students/CurrentCourseMaterial", HttpMethod = "GET", RequiredParameter = "CycleId" },
            new EndpointValidationConfig { EndpointPath = "/api/Students/CurrentCourseQuizzes", HttpMethod = "GET", RequiredParameter = "CycleId" },
            new EndpointValidationConfig { EndpointPath = "/api/Students/CurrentCourseTasks", HttpMethod = "GET", RequiredParameter = "CycleId" },
            new EndpointValidationConfig { EndpointPath = "/api/Students/Quiz", HttpMethod = "GET", RequiredParameter = "quizId" },
        };

builder.Services.AddSingleton(validationConfigs);

/*builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });*/


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<DataValidationMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(/*c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dispatch API V1");
        c.RoutePrefix = string.Empty;
    }*/);
}

app.UseDirectoryBrowser();

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});*/

app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(); // Apply CORS policy
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
