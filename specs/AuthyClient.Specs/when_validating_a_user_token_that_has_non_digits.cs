using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class when_validating_a_user_token_that_has_non_digits : given_an_authy_client_sandbox_context
    {
        private static bool _actualResult;

        private Because of = () =>
                             {
                                 _actualResult = AuthyClient.VerifyUserToken("1", "000000ZZ");
                             };

        private It should_not_be_true = () => _actualResult.ShouldBeFalse();
    }
}