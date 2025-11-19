using BioTranslatorApi.Configuration;
using BioTranslatorApi.Exceptions;
using BusinessTranslator;
using Infrastructure.Encriptors;
using UserCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<AppExceptionHandler>(); //Centralized Exception Handler
builder.Services.AddSingleton<IBioSystemsEncryptor, BioSystemsEncryptor>(); //Centralized Encryption Service

//UserCase services
builder.Services.AddTransient<IOptionsTranslation, OptionsTranslation>();
builder.Services.AddTransient<ITextTranslation, TextTranslation>();

//Business Services
builder.Services.AddTransient<IOptionsTranslationBusiness, OptionsTranslationBusiness>();
builder.Services.AddTransient<ITextTranslationBusiness, TextTranslationBusiness>();

//AppSettings.json Configuration
ApplicationConfigurator.SetupConfigurationRoot();

////Read configuration TranslatorApi
//var url = ApplicationConfigurator.GetApiUrl();
//var port = ApplicationConfigurator.GetApiPort();

////Configure url and port
//builder.WebHost.UseUrls($"{url}:{port}");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseExceptionHandler(_ => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

