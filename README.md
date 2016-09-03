# AspNetCoreCompression

AspNetCoreCompression is an ASP.NET Core compression solution designed to GZip the http responses for all requests which accept GZip encoding.

**Startup.cs code:**

    public void Configure(IApplicationBuilder app)
    {
        // Place this after UseStaticFiles
        app.UseCompression();
    }