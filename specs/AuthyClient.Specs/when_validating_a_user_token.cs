using Machine.Specifications;

namespace AuthyClient.Specs
{
    [Ignore("This spec is dependent on Authy's api allowing their test api key and a valid user...")]
    public class when_validating_a_user_token : given_an_authy_client_sandbox_context
    {
        private static bool _actualResult;

        private Because of = () => _actualResult = AuthyClient.VerifyUserToken("1", ValidTestAuthyToken);

        private It should_validate_successfully = () => _actualResult.ShouldBeTrue();

    }
}