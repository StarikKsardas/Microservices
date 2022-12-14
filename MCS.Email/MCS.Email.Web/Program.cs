using FluentValidation;
using FluentValidation.AspNetCore;
using MCS.Email.Domain.Services.Configurations;
using MCS.Email.Infrastructure.Di;
using MCS.Email.Web.Contracts.Models;
using MCS.Email.Web.Contracts.Validators;
using MCS.Email.Web.Helpers;
using MCS.Email.Web.Middlewares;
using MCS.Email.Web.RabbitMqApi;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(
        new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
        { 
            IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.Now.ToString("MM-yyyy")}",
            AutoRegisterTemplate = true,
            NumberOfShards = 2,
            NumberOfReplicas = 1
        })    
    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
    .ReadFrom.Configuration(context.Configuration);      
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddValidatorsFromAssembly(typeof(EmailWebValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();   
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();
builder.Services.AddEmailServerConfigurations();
builder.Services.AddAutoMapperService();
builder.Services.AddSingleton<IRemap, Remap>();

if (bool.Parse(Environment.GetEnvironmentVariable("ISUSERABBITMQ") ?? "false"))
{
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEmailRabbitConfiguration();
    builder.Services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
    builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
    builder.Services.AddSingleton<AbstractValidator<EmailWeb>, EmailWebValidator>();
    builder.Services.AddHostedService<RabbitMqConsumer>();
}

var app = builder.Build();
 
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.AddMiddleware();

app.Run();
