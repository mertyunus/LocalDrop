using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http.Features;
using QRCoder;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(5000));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseDefaultFiles();
app.UseStaticFiles();

string GetLocalIPAddress()
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        if (ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip))
        {
            return ip.ToString();
        }
    }
    return "127.0.0.1";
}

var localIp = GetLocalIPAddress();
var serverUrl = $"http://{localIp}:5000";

var downloadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
if (!Directory.Exists(downloadsPath))
{
    Directory.CreateDirectory(downloadsPath);
}

app.MapPost("/api/upload", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
        return Results.BadRequest("Invalid content type.");

    var form = await request.ReadFormAsync();
    var files = form.Files;

    if (files.Count == 0)
        return Results.BadRequest("No files received.");

    foreach (var file in files)
    {
        if (file.Length > 0)
        {
            var filePath = Path.Combine(downloadsPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Console.WriteLine($"[+] File received: {file.FileName} ({file.Length} bytes)");
        }
    }

    return Results.Ok(new { message = "Files uploaded successfully." });
});

app.MapGet("/api/qr", () =>
{
    using var qrGenerator = new QRCodeGenerator();
    using var qrData = qrGenerator.CreateQrCode(serverUrl, QRCodeGenerator.ECCLevel.L);
    using var qrCode = new PngByteQRCode(qrData);
    var qrCodeBytes = qrCode.GetGraphic(20);
    return Results.File(qrCodeBytes, "image/png");
});

app.MapGet("/api/info", () => Results.Ok(new { url = serverUrl, ip = localIp }));

Console.WriteLine("--- Sunucu Basladi ---");
Console.WriteLine($"PC: http://localhost:5000/dashboard.html");
Console.WriteLine($"Tel: {serverUrl}");
Console.WriteLine($"Inenler: {downloadsPath}");

app.Run();
