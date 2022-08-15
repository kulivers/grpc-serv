namespace GrpcService1
{
    interface ICounterServiceImp
    {
        public int Counter { get; set; }
        public void Increment(int amount);
    }
    public class CounterServiceImp
    {
        public int Counter { get; set; } 
        public void Increment(int amount)
        {
            Counter += amount;
        }
        public CounterServiceImp(int counter)
        {
            Counter = counter;
        }
        public CounterServiceImp()
        {
            Counter = 228;
        }
    }
}
