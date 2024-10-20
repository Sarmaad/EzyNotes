using Ardalis.Result.AspNetCore;
using EzyNotes.Infrastructure.Auth0;
using EzyNotes.Infrastructure.Mongodb;
using EzyNotes.Infrastructure.Swagger;
using EzyNotes.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>(optional: true);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.

builder.Services.AddControllers(opt =>
{
    opt.AddDefaultResultConvention();
}).AddNewtonsoftJson(opt =>
{
    opt.SerializerSettings.Converters.Add(new StringEnumConverter());
});

builder.Services.AddCors(opt => opt.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

builder.Services.Configure<ApiBehaviorOptions>(opt => opt.SuppressInferBindingSourcesForParameters = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.ConfigureSwagger();
builder.Services.AddMediatR(opt => opt.RegisterServicesFromAssemblyContaining<Program>());


// fluent validator settings
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Continue;
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });
builder.Services.AddRouting(options => { options.LowercaseUrls = true; });
builder.Services.AddHttpContextAccessor();

builder.Services.AddLazyCache();

// mongodb
builder.ConfigureMangoDb();

//authentication
builder.ConfigureAuthentication();
builder.ConfigureAuth0();

// services
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(opt =>
    {
        opt.OAuthClientId(builder.Configuration["Swagger:OAuthClientId"]);
        opt.OAuthUsePkce();
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
