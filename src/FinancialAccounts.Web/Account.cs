namespace FinancialAccounts.Web
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class Account
    {
        public DateTime BirthDate { get; set; }

        public string Family { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public string SecondName { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Sum { get; set; }
    }
}