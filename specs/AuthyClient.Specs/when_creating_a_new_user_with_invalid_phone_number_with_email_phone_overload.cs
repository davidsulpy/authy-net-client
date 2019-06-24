using System;
using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class when_creating_a_new_user_with_invalid_phone_number_with_email_phone_overload : given_an_authy_client_sandbox_context
    {
        private static Exception _actualException;

        private Because of = () =>
                             {
                                 try
                                 {
                                     AuthyClient.CreateAuthyUser("name@example.com", "AAA9-0247");
                                 }
                                 catch (Exception ex)
                                 {
                                     _actualException = ex;
                                 }
                             };

	    [Ignore("This spec will fail without a real authy api key")]
		private It should_contain_the_expected_message = () => _actualException.Message.ShouldEqual("User was not valid");

	    [Ignore("This spec will fail without a real authy api key")]
		private It should_throw_an_exception_with_errors = () => ((AuthyClientException)_actualException).Errors.ShouldNotBeEmpty();

	    [Ignore("This spec will fail without a real authy api key")]
		private It should_throw_an_authy_client_exception =
            () => _actualException.ShouldBeOfExactType<AuthyClientException>();
    }
}