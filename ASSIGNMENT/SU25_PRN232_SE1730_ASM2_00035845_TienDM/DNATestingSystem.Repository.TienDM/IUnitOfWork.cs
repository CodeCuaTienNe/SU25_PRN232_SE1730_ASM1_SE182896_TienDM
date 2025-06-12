using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Repository.TienDM
{
    public interface IUnitOfWork
    {
        // Repository properties
        AppointmentsTienDmRepository AppointmentsTienDmRepository { get; }
        AppointmentStatusesTienDmRepository AppointmentStatusesTienDmRepository { get; }
        ServicesNhanVtRepository ServicesNhanVtRepository { get; }
        UserAccountRepository UserAccountRepository { get; }

        //Unit of Work methods with transaction handling
        int SaveChangesWithTransaction();
        Task<int> SaveChangesWithTransactionAsync();
    }
}
