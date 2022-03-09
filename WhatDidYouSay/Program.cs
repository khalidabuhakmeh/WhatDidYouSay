using DeepL;
using Google.Cloud.Translate.V3;
using MiniValidation;
using WhatDidYouSay.Models;

var builder = WebApplication.CreateBuilder(args);
var credentialsPath = builder.Configuration["google:credentials"];

if (!File.Exists(credentialsPath))
{
    throw new ApplicationException(
        "Google Credentials file was not found. Either download the JSON credentials file" +
        " from GCP or alter the code to use the environment variable.");
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Google Translation API
builder.Services.AddSingleton<TranslationServiceClient>(_ =>
{
    var clientBuilder = new TranslationServiceClientBuilder
    {
        CredentialsPath = credentialsPath
    };
    return clientBuilder.Build();
});

// DeepL Translation
builder.Services.AddSingleton(_ =>
{
    var authKey = builder.Configuration["deepl:key"];
    return new Translator(authKey);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

app.MapPost("/google-translate", async (TranslationRequest request, TranslationServiceClient google) =>
    {
        if (!MiniValidator.TryValidate(request, out var errors))
        {
            return Results.ValidationProblem(errors);
        }

        var translations = await google.TranslateTextAsync(new TranslateTextRequest
        {
            TargetLanguageCode = request.TargetLanguageCode,
            SourceLanguageCode = request.SourceLanguageCode,
            Contents = {request.Text},
            MimeType = "text/plain",
            Parent = "projects/composed-anvil-343613"
        });

        // if no results, 
        var result = translations.Translations.First();

        return Results.Ok(new TranslationResponse
        {
            OriginalText = request.Text,
            TranslatedText = result.TranslatedText,
            SourceLanguageCode = result.DetectedLanguageCode,
            TargetLanguageCode = request.TargetLanguageCode
        });
    })
    .Accepts<TranslationRequest>("application/json")
    .Produces<TranslationResponse>()
    .ProducesValidationProblem();

app.MapPost("/deepl-translate", async (TranslationRequest request, Translator deepl) =>
    {
        if (!MiniValidator.TryValidate(request, out var errors))
        {
            return Results.ValidationProblem(errors);
        }

        var result = await deepl.TranslateTextAsync(
            request.Text,
            request.SourceLanguageCode!,
            request.TargetLanguageCode,
            new TextTranslateOptions()
        );

        return Results.Ok(new TranslationResponse
        {
            OriginalText = request.Text,
            TranslatedText = result.Text,
            SourceLanguageCode = result.DetectedSourceLanguageCode,
            TargetLanguageCode = request.TargetLanguageCode
        });
    })
    .Accepts<TranslationRequest>("application/json")
    .Produces<TranslationResponse>()
    .ProducesValidationProblem();

app.Run();