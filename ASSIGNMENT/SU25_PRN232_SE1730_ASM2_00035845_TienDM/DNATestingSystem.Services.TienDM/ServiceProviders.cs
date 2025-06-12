using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public class ServiceProviders : IServiceProviders
    {
        private SystemUserAccountService _systemUserAccountService;
        private AppointmentsTienDmService _appointmentsTienDmService;
        private AppointmentStatusesTienDmService _appointmentStatusesTienDmService;
        private ServicesNhanVtService _servicesNhanVtService;

        public ServiceProviders() { }

        public SystemUserAccountService SystemUserAccountService
        {
            get { return _systemUserAccountService ??= new SystemUserAccountService(); }
        }

        public AppointmentsTienDmService AppointmentsTienDmService
        {
            get { return _appointmentsTienDmService ??= new AppointmentsTienDmService(); }
        }

        public AppointmentStatusesTienDmService AppointmentStatusesTienDmService
        {
            get { return _appointmentStatusesTienDmService ??= new AppointmentStatusesTienDmService(); }
        }

        public ServicesNhanVtService ServicesNhanVtService
        {
            get { return _servicesNhanVtService ??= new ServicesNhanVtService(); }
        }
    }
}
