
using EndPoint.Areas.Admin.ViewModel;

namespace Bugeto_store.Domain.Entities
{
    public class AddOrRemoveClaimViewModel
    {
        public AddOrRemoveClaimViewModel()
        {
            UserClaims = new List<ClaimViwModel>();
        }
        public AddOrRemoveClaimViewModel(string userid, IList<ClaimViwModel> userClaims)
        {
            UserId = userid;    
            UserClaims = userClaims;    
        }

        public string UserId { get; set; }
        public IList<ClaimViwModel> UserClaims{get; set;}

}


    public class ClaimViwModel
{
        public ClaimViwModel()
        {
            
        }
        public ClaimViwModel(string claimtype)
        {
            ClaimType = claimtype;
            
        }


        public string ClaimType { get; set; }
    public bool IsSelected { get; set; }
}


}
