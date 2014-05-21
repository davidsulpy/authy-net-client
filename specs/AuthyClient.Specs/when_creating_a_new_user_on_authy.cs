using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class when_creating_a_new_user_on_authy : given_an_authy_client_sandbox_context
    {
        private static string _actualResult;

        private Establish context = () => {  };

        private Because of = () => _actualResult = AuthyClient.CreateAuthyUser(new AuthyUser
                                                                                {
                                                                                    Email = "name@example.com",
                                                                                    Cellphone = "555-555-5555",
                                                                                    CountryCode = 1
                                                                                });

        private It should_return_a_user_with_authy_user_id = () => _actualResult.ShouldNotBeEmpty();
    }
}