using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class when_creating_a_new_user_on_authy_with_email_phone_overload : given_an_authy_client_sandbox_context
    {
        private static string _actualResult;

        private Because of = () => _actualResult = AuthyClient.CreateAuthyUser("name@example.com", "555-555-5555");

        private It should_return_a_user_with_authy_user_id = () => _actualResult.ShouldNotBeEmpty();
    }
}