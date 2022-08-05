using GrpcService1;
using GrpcService1.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "x-protobuf", "Grpc-Accept-Encoding", "X-Grpc-Web", "User-Agent")
        .AllowAnyHeader();
}));
builder.Services.AddSingleton<CounterServiceImp>();

var app = builder.Build();
app.UseHttpsRedirection();

app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcReflectionService();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcCounterService>().EnableGrpcWeb().RequireCors("AllowAll");
    endpoints.MapGrpcService<GrpcGreeterService>().EnableGrpcWeb().RequireCors("AllowAll");
});
app.Run();