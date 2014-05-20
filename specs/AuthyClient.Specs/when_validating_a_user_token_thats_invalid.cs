using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class when_validating_a_user_token_thats_invalid : given_an_authy_client_sandbox_context
    {
        private static bool _actualResult;

        private Because of = () =>
                             {
                                     _actualResult = AuthyClient.VerifyUserToken("1", "1234567");
                             };

        private It should_not_be_true = () => _actualResult.ShouldBeFalse();
    }
}