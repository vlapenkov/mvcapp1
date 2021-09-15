using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    /// <summary>
    /// Класс содержащий начисления, оплаты и итог для любого узла дерева
    /// </summary>
    public class AccrualsPayments
    {
        private double accrualsSum, paymentsSum;
        public double AccrualsSum { get => Math.Round(accrualsSum,2); set { accrualsSum = value; } }
        public double PaymentsSum { get => Math.Round(paymentsSum,2); set { paymentsSum = value; } }
        public double RestSum => Math.Round(AccrualsSum - PaymentsSum,2);
    }
}
