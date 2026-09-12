using Microsoft.OpenApi;
using SRSProject.Application;
using SRSProject.Infrastructure;
using SRSProject.Infrastructure.Jwt;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.InfrastructureRegisterServiceMethod(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddOpenApi();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(document =>
     new OpenApiSecurityRequirement
     {
         [new OpenApiSecuritySchemeReference("Bearer", document)] =
             new List<string>()
     });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
