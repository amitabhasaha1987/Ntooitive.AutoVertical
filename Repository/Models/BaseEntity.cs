using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models
{
    public abstract class BaseEntity
    {
        public virtual string BaseEntity_Id { get; set; }

        public virtual DateTime? BaseEntity_CreatedDate { get; set; }

        public virtual DateTime? BaseEntity_UpdatedDate { get; set; }
        public virtual bool IsUpdatedByPortal { get; set; }
    }
}
