namespace HKDG.BLL
{
    public interface IUserBLL:IDependency
    {

        Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang);

        bool CheckMerchantAccountExist(Guid merchantId);

        SystemResult CreateAccountForMerchant(string Ids);

        SystemResult Save(UserDto model);

        UserDto GetUserInfoById(string UserId);

        PageData<UserDto> Search(UserCondition condition);

        UserDto GetById(Guid userId);

        SystemResult Remove(Guid Id);

        SystemResult PhysicalDelete(UserDto model);

        SystemResult ResetPassword(Guid id);

        SystemResult Update(UserDto model);

        SystemResult UpdatePassword(PasswordView passwordView);
    }
}
