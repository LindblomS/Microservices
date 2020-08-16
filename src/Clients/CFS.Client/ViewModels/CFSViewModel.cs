using CFS.Application.Application.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace CFS.Client.ViewModels
{
    public class CFSViewModel
    {
        public CustomerViewModel Customer { get; set; }
        public List<FacilityViewModel> Facilites { get; set; }
        public List<ServiceViewModel> Services { get; set; }
    }
}
