using System.ComponentModel.DataAnnotations;

namespace WhatDidYouSay.Models;

public class TranslationRequest
{
    [Required, MinLength(1)]
    public string Text { get; set; } = "";
    public string TargetLanguageCode { get; set; } = "en-US";
    public string? SourceLanguageCode { get; set; } = "";
}