namespace HKDG.Repository
{
    public interface IRoleRepository:IDependency
    {
        RoleDto GetRoleByEager(Guid id);

        List<RoleDto> Search(RoleCondition cond);
    }
}
