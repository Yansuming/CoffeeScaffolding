using CoffeeScaffolding.CoffeeScaffoldingUtil.Msg;
using MediatR;

namespace CoffeeScaffolding.Controllers
{
    public class doSendEmailMediatR: INotificationHandler<doSometingBeforeSendMailMediatR>
    {
        public Task Handle(doSometingBeforeSendMailMediatR notification, CancellationToken cancellationToken)
        {                       
            Console.WriteLine("Send MediatR "+ notification.msg);
            return Task.CompletedTask;
        }
        // public static void SendMediatR()
        // {            
        //     Email.SendMail("",true,"","","","","","","","");
        //     Console.WriteLine("Send MediatR");
        // }
    }
}