using log4net;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Security.Principal;
using static AutoMapper.Internal.ExpressionFactory;
using Member = Model.Member;

namespace HKDG.BLL
{
    public class LoginBLL : BaseBLL, ILoginBLL
    {
        IUserRoleRepository userRoleRepository;
        public LoginBLL(IServiceProvider services) : base(services)
        {
             userRoleRepository = Services.Resolve<IUserRoleRepository>();
        }

        public async Task<SystemResult> Login(LoginInput input) {

            var result = new SystemResult() ;
            var user = await baseRepository.GetModelAsync<Member>(x =>x.Account == input.Account);

            if (user == null) throw new BLException("账号错误");
            string pwdBase = PwdUtil.GenPwdBase(user.Account, input.Password);
            if (user.Password != HashUtil.MD5(pwdBase))
            {
                throw new BLException("密码错误");
            }

            result.ReturnValue = AutoMapperExt.MapTo<MemberDto>(user);
            result.Succeeded = true;          
            return result;
        }

        public async Task<SystemResult> AdminLogin(UserDto user)
        { 
            var result = new SystemResult() ;
          
            var roles = this.userRoleRepository.GetUserRoles(user.Id);
            user.Roles = AutoMapperExt.MapTo<List<RoleDto>>(roles);

            user.LoginType = user.MerchantId != Guid.Empty ? LoginType.Merchant : LoginType.Admin;

            foreach (var item in user.Roles)
            {
                if (item.IsSystem && item.Name == "SuperAdmin")
                {
                    var permissionList = await baseRepository.GetListAsync<Permission>(x => x.IsActive && !x.IsDeleted);
                    item.PermissionList = AutoMapperExt.MapTo<List<PermissionDto>>(permissionList);
                }
            }

            result.ReturnValue = user;
            result.Succeeded = true;

            return result;
        }

        public async Task<UserDto> CheckAdminLogin(LoginInput input)
        {
            var result = new SystemResult();
            //string pwd = ToolUtil.Md5Encrypt(input.Password);
            string pwd = HashUtil.HashPwd(input.Password);
           
            var dbUser = await baseRepository.GetModelAsync<User>(d => d.IsActive && !d.IsDeleted && (d.Account == input.Account || d.Email == input.Account));

            if (dbUser == null) throw new ServiceException(HKDG.Resources.Message.AccountNotExist);
            if (dbUser.Password != pwd) throw new ServiceException(HKDG.Resources.Message.PasswordNotMatch);

            var user = AutoMapperExt.MapTo<UserDto>(dbUser);

            if (user.MerchantId != Guid.Empty)
            {
                user.LoginType = LoginType.Merchant;
            }
            else
            {
                user.LoginType = LoginType.Admin;
            }

            return user;
        }

        public async Task<SystemResult> FBLogin(LoginInput input, bool thirdLinkUp = true)
        {
            var result = new SystemResult();
            if (thirdLinkUp)
            {
                var mInfo = await baseRepository.GetModelAsync<Member>(x => x.Account == input.Account);
                if (mInfo == null)
                {
                    result.Message = Resources.Message.AccountNotExist;
                    result.ReturnValue = 1001;//  不存在會員
                    return result;
                }

                if (!mInfo.IsApprove)
                {
                    result.Message = Resources.Message.AccountIsNotActive;
                    result.ReturnValue = 1002; //your account has not be activated
                    return result;
                }
                if (!mInfo.IsActive)
                {
                    result.Message = Resources.Message.AccountDisabled;
                    result.ReturnValue = 1003;//your account is locked
                    return result;
                }
                if (mInfo.IsDeleted)
                {                   
                    result.Message = Resources.Message.AccountDisabled;
                    result.ReturnValue = 1005;// your account is deleted
                    return result;
                }

                UnitOfWork.IsUnitSubmit = true;
                var loginDatetime = DateTime.Now;
               
                mInfo.LastLogin = loginDatetime;

               await baseRepository.UpdateAsync(mInfo);

                var flag = await baseRepository.AnyAsync<MemberAccount>(x => x.MemberId == mInfo.Id);
                if (!flag) {

                    MemberAccount memberAccount = new MemberAccount();
                    memberAccount.Id = Guid.NewGuid();
                    memberAccount.MemberId = mInfo.Id;                  
                    memberAccount.CreateBy = mInfo.Id;
                    memberAccount.IsActive = true;
                    memberAccount.IsDeleted = false;
                    memberAccount.Fun = 0;
                    await baseRepository.InsertAsync(memberAccount);
                }

                MemberLoginRecord record = new MemberLoginRecord();
                record.Id = Guid.NewGuid();
                record.MemberId = mInfo.Id;
                record.LoginTime = loginDatetime;
                record.LoginFrom =  AppTypeEnum.WebSite;
                await baseRepository.InsertAsync(record);
 
                UnitOfWork.Submit();

                result.Succeeded = true;
                result.ReturnValue = mInfo;
            }

            return result;
        }
    }
}
