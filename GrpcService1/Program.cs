using GrpcService1;
using Grpc.AspNetCore.Web;
using GrpcService1.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();


builder.Services.AddSingleton<CounterServiceImp>();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();
app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true }); // all services support Web
app.UseCors();
app.UseEndpoints(endpoints => {
    endpoints.MapGrpcService<GrpcCounterService>().EnableGrpcWeb().RequireCors("AllowAll");
    endpoints.MapGrpcService<GrpcGreeterService>().EnableGrpcWeb().RequireCors("AllowAll");
});
app.Run();