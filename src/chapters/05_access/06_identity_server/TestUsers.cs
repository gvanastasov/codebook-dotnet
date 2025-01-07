using IdentityServer4.Test;

public static class TestUsers
{
    public static List<TestUser> GetUsers() =>
        new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "testuser",
                Password = "password",
                IsActive = true,
                Claims =
                {
                    new System.Security.Claims.Claim("name", "Test User"),
                    new System.Security.Claims.Claim("email", "testuser@example.com")
                }
            }
        };
}
