using MediatR;

namespace CoffeeScaffolding.Controllers
{
    public class doSometingBeforeSendMailMediatR:INotification
    {
        public string msg {get; private set;}
        public doSometingBeforeSendMailMediatR(string msg)
        {
            Console.WriteLine("doing something1...");
            Console.WriteLine("doing something2...");
            Console.WriteLine("doing something3...");
            Console.WriteLine("doing something4...");
            Console.WriteLine("doing something5...");
            Console.WriteLine("msg...");
            this.msg = msg;
        }
    }
}