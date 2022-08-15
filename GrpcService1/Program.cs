using System.Reflection;
using GrpcService1;
using GrpcService1.Extensions;
using GrpcService1.Services;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "x-protobuf", "Grpc-Accept-Encoding",
            "X-Grpc-Web", "User-Agent")
        .AllowAnyHeader();
}));
builder.Services.AddSingleton<CounterServiceImp>();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));


var app = builder.Build();
app.UseHttpsRedirection();

app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.MapGrpcReflectionService();
app.UseCors("AllowAll");
app.UseEndpoints(endpointsBuilder =>
{
    var ns = "GrpcService1.Services";
    var grpcServicesTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.Namespace == ns);
    var counterService = grpcServicesTypes.First(s => s.Name == "GrpcCounterService");

    // var grpcEndpointsBuilderType = typeof(GrpcEndpointRouteBuilderExtensions);
    // var mapGrpcMethod =  grpcEndpointsBuilderType.GetMethod("MapGrpcService");
    // var grpcMapperConstructed = mapGrpcMethod.MakeGenericMethod(counterService);
    // grpcMapperConstructed.Invoke(null, new[] { endpointsBuilder });
    foreach (var grpcServicesType in grpcServicesTypes)
    {
        endpointsBuilder.MapGrpcService(grpcServicesType);    
    }
    
    // endpointsBuilder.MapGrpcService<GrpcGreeterService>().EnableGrpcWeb().RequireCors("AllowAll");
    endpointsBuilder.MapControllers();
});
app.MapControllers();
app.Run();