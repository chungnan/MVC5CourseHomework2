namespace MVC5CourseHomework.Models
{
    public static class RepositoryHelper
    {
        public static IUnitOfWork GetUnitOfWork()
        {
            return new EFUnitOfWork();
        }

        public static sysdiagramsRepository GetsysdiagramsRepository()
        {
            var repository = new sysdiagramsRepository();
            repository.UnitOfWork = GetUnitOfWork();
            return repository;
        }

        public static sysdiagramsRepository GetsysdiagramsRepository(IUnitOfWork unitOfWork)
        {
            var repository = new sysdiagramsRepository();
            repository.UnitOfWork = unitOfWork;
            return repository;
        }

        public static View_客戶對應聯絡人及銀行帳戶數量Repository GetView_客戶對應聯絡人及銀行帳戶數量Repository()
        {
            var repository = new View_客戶對應聯絡人及銀行帳戶數量Repository();
            repository.UnitOfWork = GetUnitOfWork();
            return repository;
        }

        public static View_客戶對應聯絡人及銀行帳戶數量Repository GetView_客戶對應聯絡人及銀行帳戶數量Repository(IUnitOfWork unitOfWork)
        {
            var repository = new View_客戶對應聯絡人及銀行帳戶數量Repository();
            repository.UnitOfWork = unitOfWork;
            return repository;
        }

        public static CustomerRepository Get客戶資料Repository()
        {
            var repository = new CustomerRepository();
            repository.UnitOfWork = GetUnitOfWork();
            return repository;
        }

        public static CustomerRepository Get客戶資料Repository(IUnitOfWork unitOfWork)
        {
            var repository = new CustomerRepository();
            repository.UnitOfWork = unitOfWork;
            return repository;
        }

        public static CustomerBankRepository Get客戶銀行資訊Repository()
        {
            var repository = new CustomerBankRepository();
            repository.UnitOfWork = GetUnitOfWork();
            return repository;
        }

        public static CustomerBankRepository Get客戶銀行資訊Repository(IUnitOfWork unitOfWork)
        {
            var repository = new CustomerBankRepository();
            repository.UnitOfWork = unitOfWork;
            return repository;
        }

        public static CustomerContantRepository Get客戶聯絡人Repository()
        {
            var repository = new CustomerContantRepository();
            repository.UnitOfWork = GetUnitOfWork();
            return repository;
        }

        public static CustomerContantRepository Get客戶聯絡人Repository(IUnitOfWork unitOfWork)
        {
            var repository = new CustomerContantRepository();
            repository.UnitOfWork = unitOfWork;
            return repository;
        }
    }
}