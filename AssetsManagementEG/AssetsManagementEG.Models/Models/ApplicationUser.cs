using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AssetsManagementEG.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        //this below will ref to the main id of identity user
        public string ID => Id;

        public virtual ICollection<UsersDistrict> UsersDistricts { get; set; }
    }
}
