using System.Security.Claims;

namespace api.Extensions;

public static class ClaimsExtensions
{
    public static string GetUsername(this ClaimsPrincipal user)
    {
        var claim = user.Claims.SingleOrDefault(x =>
            x.Type.Equals(ClaimTypes.Name) || x.Type.Equals(ClaimTypes.GivenName));

        if (claim == null)
            throw new InvalidOperationException("Claim for given name not found");
        
        Console.WriteLine($"Claim got with user logged: Claim Type: {claim.Type}; Claim Value: {claim.Value}");
        
        return claim.Value;
    }
}