namespace Identity.API.Configurations
{
    using IdentityServer4.Test;
    using System.Collections.Generic;

    internal class Users
    {
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "testuser1",
                    Username = "testuser1",
                    Password = "testuser1"
                }
            };
        }
    }
}
