using BK2T.BankDataReporting.Departments.ManualCapitals;
using BK2T.BankDataReporting.TargetPlans;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace BK2T.BankDataReporting.Departments
{
    public class DepartmentManager : DomainService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IGuidGenerator _guidGenerator;

        public DepartmentManager(IDepartmentRepository departmentRepository, IGuidGenerator guidGenerator)
        {
            _departmentRepository = departmentRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task<Department> CreateAsync(
            [NotNull] string code,
            [NotNull] string name,
            string oldCode,
            IEnumerable<string> customerSegments)
        {
            var targetPlans = SeedTargetPlans();
            var manualCapitals = SeedManualCapitals();
            Check.NotNullOrWhiteSpace(code, nameof(code));
            Check.NotNullOrWhiteSpace(name, nameof(name));

            if (await _departmentRepository.AnyAsync(d => d.Code == code))
            {
                throw new DepartmentAlreadyExistsException(code);
            }

            return new Department(_guidGenerator.Create(), code, name, oldCode?.Trim(), customerSegments, targetPlans, manualCapitals);
        }

        public async Task ChangeCodeAsync([NotNull] Department department, [NotNull] string newCode)
        {
            Check.NotNull(department, nameof(department));
            Check.NotNullOrWhiteSpace(newCode, nameof(newCode));

            var existingDepartment = await _departmentRepository.FirstOrDefaultAsync(d => d.Code == newCode);
            if (existingDepartment != null && existingDepartment.Id != department.Id)
            {
                throw new DepartmentAlreadyExistsException(newCode);
            }

            department.ChangeCode(newCode);
        }

        private List<ManualCapital> SeedManualCapitals()
        {
            List<ManualCapital> manualCapitals = new()
            {
                new ManualCapital(
                    CustomerType.BigEnterpriseCustomer,
                    UnitMeasure.Billion,
                    new List<string>
                    {
                        "06", "07", "08"
                    }),
                new ManualCapital(
                    CustomerType.MediumEnterpriseCustomer,
                    UnitMeasure.Billion,
                    new List<string>
                    {
                        "00", "01", "02", "03", "05", "88", "89", "97", "99"
                    }),
                new ManualCapital(
                    CustomerType.FDIEnterpriseCustomer,
                    UnitMeasure.Billion,
                    new List<string>
                    {
                        "04", "09"
                    }),
                new ManualCapital(
                    CustomerType.PersonalCustomer,
                    UnitMeasure.Billion,
                    new List<string>
                    {
                        "CN01", "CN02", "CN03", "CN04", "CN05", "CN06", "CN07", "CN08", "CN09"
                    }),
                new ManualCapital(
                    CustomerType.FinancialCustomer,
                    UnitMeasure.Billion,
                    new List<string>
                    {
                        "98"
                    })
            };
            return manualCapitals;
        }
        private List<TargetPlan> SeedTargetPlans()
        {
            var targetPlans = new List<TargetPlan>();

            var smallCreditBalanceTargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.BigEnterpriseCreditBalance, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.MediumEnterpriseCreditBalance, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.FDIEnterpriseCreditBalance, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.PersonalCreditBalance, UnitMeasure.Billion),
            };
            var creditBalanceTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.CreditBalance, UnitMeasure.Billion, smallCreditBalanceTargetPlans);

            var smallCapitalTargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.BigEnterpriseCapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.MediumEnterpriseCapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.FDIEnterpriseCapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.PersonalCreditCapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.FinancialCapital, UnitMeasure.Billion)
            };
            var capitalTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.Capital, UnitMeasure.Billion, smallCapitalTargetPlans);

            var smallCASACapitalTargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.BigEnterpriseCASACapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.MediumEnterpriseCASACapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.FDIEnterpriseCASACapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.PersonalCreditCASACapital, UnitMeasure.Billion),
                new TargetPlan(_guidGenerator.Create(), PlanType.FinancialCASACapital, UnitMeasure.Billion),
            };
            var cASACapitalTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.CASACapital, UnitMeasure.Billion, smallCASACapitalTargetPlans);

            var smallNIITargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.CreditNII, UnitMeasure.Million),
                new TargetPlan(_guidGenerator.Create(), PlanType.CapitalNII, UnitMeasure.Million),
            };
            var  niiTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.NII, UnitMeasure.Million, smallNIITargetPlans);

            var serviceFeeTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.ServiceFee, UnitMeasure.Million, new List<TargetPlan>());

            var tradeMoneyProfitTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.ProfitOfTradeMoney, UnitMeasure.Million, new List<TargetPlan>());

            var totalProfitTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.TotalProfit, UnitMeasure.Million, new List<TargetPlan>());

            var smallInsuranceTargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.LifeInsuranceSale, UnitMeasure.Million),
                new TargetPlan(_guidGenerator.Create(), PlanType.NonLifeInsuranceSale, UnitMeasure.Million),
            };
            var insuranceTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.InsuranceSale, UnitMeasure.Million, smallInsuranceTargetPlans);
            
            var smallCustomerGoalTargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.EnterpriseCustomerGoal_small, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.EnterpriseCustomerGoal_big , UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.ExcellenceCustomerGoal, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.ExcellenceCustomerGoal_5, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.PersonalCustomerGoal, UnitMeasure.NumberOfCustomer),
            };
            var customerGoalTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.CustomerGoal, UnitMeasure.NumberOfCustomer, smallCustomerGoalTargetPlans);

            var smallCrossSellingTargetPlans = new List<TargetPlan>()
            {
                new TargetPlan(_guidGenerator.Create(), PlanType.IPayCustomer, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.EFastCustomer , UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.KTBLCustomer, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.EnterpriseCustomerPaidSalary, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.StaffRecieveSalary, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.ActiveCredit, UnitMeasure.NumberOfCustomer),
                new TargetPlan(_guidGenerator.Create(), PlanType.AcceptCreditUnit, UnitMeasure.NumberOfCustomer),
            };
            var crossSellingTargetPlan = new TargetPlan(_guidGenerator.Create(), PlanType.CrossSellingGoal, UnitMeasure.NumberOfCustomer, smallCrossSellingTargetPlans);
            
            targetPlans.AddRange(new List<TargetPlan>
            { 
                creditBalanceTargetPlan,
                capitalTargetPlan,
                cASACapitalTargetPlan,
                niiTargetPlan,
                serviceFeeTargetPlan,
                tradeMoneyProfitTargetPlan,
                totalProfitTargetPlan,
                insuranceTargetPlan,
                customerGoalTargetPlan,
                crossSellingTargetPlan
            });
            return targetPlans;
        }
    }
}