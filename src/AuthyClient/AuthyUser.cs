namespace AuthyClient
{
    public class AuthyUser
    {
        public AuthyUser()
        {
            
        }

        /// <summary>
        /// Helper constructor for creating Authy user with necessarily properties
        /// </summary>
        /// <param name="email">email of user</param>
        /// <param name="cellphone">cellphone of user in format xxx-xxx-xxxx</param>
        /// <param name="countryCode">country code of cellphone, defaults to 1 (US)</param>
        public AuthyUser(string email, string cellphone, int countryCode = 1)
        {
            Email = email;
            Cellphone = cellphone;
            CountryCode = countryCode;
        }

        /// <summary>
        /// UserId from Authy
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Email address of user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Cellphone number of user, must be unique.
        /// </summary>
        public string Cellphone { get; set; }

        /// <summary>
        /// Country code for Cellphone.
        /// </summary>
        public int CountryCode { get; set; }
    }
}