using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IServiceProviders
    {
        // Service properties
        SystemUserAccountService SystemUserAccountService { get; }
        AppointmentsTienDmService AppointmentsTienDmService { get; }
        AppointmentStatusesTienDmService AppointmentStatusesTienDmService { get; }
        ServicesNhanVtService ServicesNhanVtService { get; }
    }
}
