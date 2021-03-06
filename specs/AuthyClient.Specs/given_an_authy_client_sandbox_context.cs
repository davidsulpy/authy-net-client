﻿using Machine.Specifications;

namespace AuthyClient.Specs
{
    public class given_an_authy_client_sandbox_context
    {
        protected static IAuthyApiClient AuthyClient;
        protected const string ValidTestAuthyToken = "0000000";

        private Establish context = () =>
                                    {
										// NOTE: must replace with real Authy Api Key in order to run these tests since
										// authy discontinued the sandbox api
                                        AuthyClient = new AuthyApiClient("d57d919d11e6b221c9bf6f7c882028f9");
                                    };
    }
}