using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPTextCommon.EFBaseServices.Model
{
    public class CompanyModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreatorId { get; set; }

        public int? LastModifierId { get; set; }

        public DateTime? LastModifyTime { get; set; }
    }



    public partial class sysCompanyModelRepository : BaseDal<CompanyModel>, IsysCompanyModelRepository
    {
    }

    public partial interface IsysCompanyModelRepository : IBaseDal<CompanyModel>
    {
    }
    public partial interface IBaseCompanyModelServices : IBaseServices<CompanyModel>
    {

    }
    public partial class BaseCompanyModelServices : BaseServices<CompanyModel>, IBaseCompanyModelServices
    {
        IsysCompanyModelRepository subdal;

        public BaseCompanyModelServices(IsysCompanyModelRepository dal)
        {
            this.subdal = dal;
            base.basedal = dal;
        }
    }
}
