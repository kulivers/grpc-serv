using Greet;
using Grpc.Core;

namespace GrpcService1.Services;

public class GrpcGreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
    }

    public override async Task SayHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
    {
        var i = 0;
        while (!context.CancellationToken.IsCancellationRequested && i<5)
        {
            await responseStream.WriteAsync(new HelloReply { Message = $"Hello {request.Name} {i}" });
            await Task.Delay(TimeSpan.FromMilliseconds(500));
            i++;
        }
    }
    
}