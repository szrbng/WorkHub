namespace WorkHub.Api.Authorization
{
    public static class Policies
    {
        public const string AdminOnly = "AdminOnly";
        public const string Approvers = "Approvers";   // Manager + Admin
        public const string HrStaff = "HrStaff";       // HumanResources + Admin
    }
}
