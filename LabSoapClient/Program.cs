//namespace LabSoapClient
//{
//    public class Program
//    {
//        static async Task Main(string[] args)
//        {
//            var soapServiceClient = new UserServiceClient(UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService);
//            var registerUserResponse = await soapServiceClient.RegisterUserAsync(new User()
//            {
//                FirstName = "Jan",
//                LastName = "Kowalski",
//                EmailAddress = "jankowalski@wsei.edu.pl",
//                Age = 25,
//                MarketingConsent = true
//            });
//            Console.WriteLine(registerUserResponse);
//            Console.ReadKey();
//        }
//    }
//}
