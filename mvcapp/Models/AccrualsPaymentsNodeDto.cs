using System;
using System.Collections.Generic;
using System.Text;


namespace Models
{

    /// <summary>
    /// Узел дерева для содержащий начисления и  оплаты
    /// </summary>
    public class AccrualsPaymentsNodeDto : TreeNodeDto<AccrualsPayments>
    {
        public AccrualsPaymentsNodeDto(long id, string name = null, string nodeType = null, long? parentId = null) : base(id, name, nodeType, parentId)
        {  }

        public AccrualsPaymentsNodeDto() : base()
        {  }

       
        
    }
}
