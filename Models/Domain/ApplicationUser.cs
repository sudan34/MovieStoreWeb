using Microsoft.AspNetCore.Identity;

namespace MovieStoreWeb.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }    
    }
}
