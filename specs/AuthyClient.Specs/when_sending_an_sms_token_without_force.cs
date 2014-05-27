using System;
using Machine.Specifications;

namespace AuthyClient.Specs
{
    [Ignore("This spec fails with the test api_key because Authy's test api_key isn't the right level of account to return a successful response")]
    public class when_sending_an_sms_token_without_force : given_an_authy_client_sandbox_context
    {
        private static Exception _actualException;

        private Establish context = () => { };

        private Because of = () =>
                             {
                                 try
                                 {
                                     AuthyClient.SendSmsToken("1");
                                 }
                                 catch (Exception ex)
                                 {
                                     _actualException = ex;
                                 }
                             };

        private It should_not_throw_an_exception = () => _actualException.ShouldBeNull();
    }
}