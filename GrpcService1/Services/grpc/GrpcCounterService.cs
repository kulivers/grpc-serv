using Count;
using Grpc.Core;

namespace GrpcService1.Services
{
    public class GrpcCounterService : Counter.CounterBase
    {
        public GrpcCounterService(CounterServiceImp imp)
        {
            CounterServImp = imp;
        }

        public CounterServiceImp CounterServImp { get; private set; }


        //client streaming AccumulateCount
        //todo rename to counter accumulator
        public override async Task<CounterReply> AccumulateCount(IAsyncStreamReader<CounterRequest> requestStream,
            ServerCallContext context)
        {
            await foreach (var counterRequest in requestStream.ReadAllAsync())
            {
                CounterServImp.Counter += counterRequest.Count;
            }

            return new CounterReply { CurCounter = CounterServImp.Counter };
        }


        //serverStreaming
        //todo rename to counter resetter with output
        public override async Task Countdown(Empty request, IServerStreamWriter<CounterReply> responseStream,
            ServerCallContext context)
        {
            while (CounterServImp.Counter != 0)
            {
                await responseStream.WriteAsync(new CounterReply() { CurCounter = CounterServImp.Counter });
                CounterServImp.Counter--;
                await Task.Delay(300);
            }
        }

        public override Task<CounterReply> Increment(Empty request, ServerCallContext context)
        {
            CounterServImp.Increment(1);
            return Task.FromResult(new CounterReply() { CurCounter = CounterServImp.Counter });
        }

        public override Task<Empty> SetCounter(CounterRequest request, ServerCallContext context)
        {
            CounterServImp.Counter = request.Count; 
            return Task.FromResult(new Empty());
        }

        public override Task<CounterReply> GetCounter(Empty request, ServerCallContext context)
        {
            //if reply counter = 0, it returns {{}} instead of {"curCounter": 0}
            var reply = new CounterReply() { CurCounter = CounterServImp.Counter };
            var reply0 = new CounterReply() { CurCounter = 0 }; //this one returns {} too unfortunately
            var reply2 = new CounterReply() { CurCounter = 3 };
            return Task.FromResult(reply);
        }
    }
}