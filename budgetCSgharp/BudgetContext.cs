using System.Data.Entity;

namespace budgetCSgharp
{
    class BudgetContext:DbContext
    {
        public BudgetContext():base("DBConnection")
        {

        }

        public DbSet<Sex> sexes { get; set; }
    }

}
