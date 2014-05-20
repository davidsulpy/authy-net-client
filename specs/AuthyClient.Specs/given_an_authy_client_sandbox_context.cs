using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class given_an_authy_client_sandbox_context
    {
        protected static AuthyClient AuthyClient;
        protected const string ValidTestAuthyToken = "0000000";

        private Establish context = () =>
                                    {
                                        AuthyClient = new AuthyClient("d57d919d11e6b221c9bf6f7c882028f9");
                                    };
    }
}