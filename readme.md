# What Did You Say?

A Minimal API that allows you to test translation services Google Translate and DeepL.

You'll need credentials from both of these services to proceed.

- Google Translate - https://cloud.google.com
- Deepl - https://deepl.com

Both are very good translation service providers and operate similarly.

## Getting Started

For Google, you'll need to download the JSON credentials on to your machine.
I have it in the root directory of the project but you may also use the environment variable
(you'll need to modify the code).

For DeepL,
set the configuration setting of `deepl:key` either
in the `appSettings.json` or using `dotnet user-secrets set deepl:key <key>`.


## Making Translation calls

Each service supports different language sets, so read the documentation.
You can make calls using the [`Requests.http`](./WhatDidYouSay/Requests.http) file and JetBrains Rider's HTTP Client.

I have also included OpenAPI, and you can make requests from there as well.

## License

Don't sue me.

