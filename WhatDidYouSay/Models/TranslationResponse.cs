namespace WhatDidYouSay.Models;

public class TranslationResponse
{
    public string OriginalText { get; set; } = "";
    public string TranslatedText { get; set; } = "";
    public string TargetLanguageCode { get; set; } = "";
    public string SourceLanguageCode { get; set; } = "";
}