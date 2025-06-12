using DNATestingSystem.Repository.TienDM.DBContext;
using System;
using System.Threading.Tasks;

namespace DNATestingSystem.Repository.TienDM
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SE18_PRN232_SE1730_G3_DNATestingSystemContext _context;
        private AppointmentsTienDmRepository _appointmentsTienDmRepository;
        private AppointmentStatusesTienDmRepository _appointmentStatusesTienDmRepository;
        private ServicesNhanVtRepository _servicesNhanVtRepository;
        private UserAccountRepository _userAccountRepository;

        public UnitOfWork() => _context ??= new SE18_PRN232_SE1730_G3_DNATestingSystemContext();

        public AppointmentsTienDmRepository AppointmentsTienDmRepository
        {
            get { return _appointmentsTienDmRepository ??= new AppointmentsTienDmRepository(_context); }
        }

        public AppointmentStatusesTienDmRepository AppointmentStatusesTienDmRepository
        {
            get { return _appointmentStatusesTienDmRepository ??= new AppointmentStatusesTienDmRepository(_context); }
        }

        public ServicesNhanVtRepository ServicesNhanVtRepository
        {
            get { return _servicesNhanVtRepository ??= new ServicesNhanVtRepository(_context); }
        }

        public UserAccountRepository UserAccountRepository
        {
            get { return _userAccountRepository ??= new UserAccountRepository(_context); }
        }

        public void Dispose() => _context.Dispose();

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }
    }
}
