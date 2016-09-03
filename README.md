# AspNetCoreCompression

The [AspNetCoreCompression package](https://www.nuget.org/packages/AspNetCoreCompression/) is an ASP.NET Core compression solution designed to GZip the http responses for all requests which accept GZip encoding.

[![NuGet](https://img.shields.io/nuget/v/AspNetCoreCompression.svg?maxAge=259200)](https://www.nuget.org/packages/AspNetCoreCompression/) 

**NuGet install:**

Install-Package AspNetCoreCompression

**Startup.cs code:**

    public void Configure(IApplicationBuilder app)
    {
        // Place this after UseStaticFiles
        app.UseCompression();
    }