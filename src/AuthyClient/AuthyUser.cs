namespace AuthyClient
{
    public class AuthyUser
    {
        public AuthyUser()
        {
            
        }
        public AuthyUser(string email, string cellphone, int countryCode = 1)
        {
            Email = email;
            Cellphone = cellphone;
            CountryCode = countryCode;
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public string Cellphone { get; set; }
        public int CountryCode { get; set; }
    }
}