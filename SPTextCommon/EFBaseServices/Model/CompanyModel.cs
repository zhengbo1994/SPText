using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.EFBaseServices.Model
{
    public partial class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreatorId { get; set; }

        public int? LastModifierId { get; set; }

        public DateTime? LastModifyTime { get; set; }
    }



    public partial class sysCompanyModelRepository : BaseDal<Company>, IsysCompanyModelRepository
    {
    }

    public partial interface IsysCompanyModelRepository : IBaseDal<Company>
    {
    }
    public partial interface IBaseCompanyModelServices : IBaseServices<Company>
    {

    }
    public partial class BaseCompanyModelServices : BaseServices<Company>, IBaseCompanyModelServices
    {
        IsysCompanyModelRepository subdal;

        public BaseCompanyModelServices(IsysCompanyModelRepository dal)
        {
            this.subdal = dal;
            base.basedal = dal;
        }
    }
}
