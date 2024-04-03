using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using HW4.DataAccess;
using HW4.DataAccess.Abstractions;
using HW4.DataAccess.Services;
using Microsoft.OpenApi.Models;
using HW4.Interceptors;
using HW4.BusinessLogic.Services;
using HW4.BusinessLogic.Services.Base;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
	.AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddGrpc(options =>
{
	options.Interceptors.Add<ErrorInterceptor>();
	options.Interceptors.Add<LogInterceptor>();
}).AddJsonTranscoding();

builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Title = "HW4Service.Api",
		Version = "v1"
	});
});
builder.Services.AddGrpcReflection();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddAutoMapper(typeof(IProductService));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapGrpcReflectionService();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<ProductService>();

app.Run();
