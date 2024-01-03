using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExpensesAPI.Models
{
    public class TransactionDTO
    {
        private int _transactionType;
        public int Id { get; set; }
        public int PeriodId { get; set; }
        public double Value { get; set; }
        public int TransactionType
        {
            get { return _transactionType; }
            set
            {
                _transactionType = value;

                if (TransactionType == 1) 
                {
                    TransactionName = "Crédito";
                }
                else if (TransactionType == 2)
                {
                    TransactionName = "Débito";
                }
                else
                {
                    TransactionName = "Salário";
                }
            }
        }
        public string TransactionName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfMovement { get; set; }
    }
}
