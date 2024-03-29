﻿namespace Domain
{
    /// <summary>
    /// 角色搜尋條件
    /// </summary>
    public class RoleCondition
    {
        public RoleCondition()
        {
            this.IsActive = false;
            this.IsDeleted = false;
            this.IsSystem = null;
        }
        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsSystem { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public Language Language { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
