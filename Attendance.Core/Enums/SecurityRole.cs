namespace Attendance.Core.Enums
{
    public class SecurityRole
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        //این مدیر به تاریخچه رانندگان دسترسی ندارد
        public const string Admin2 = "Admin2";
        //این مدیر به تاریخچه کارت دسترسی ندارد
        public const string Admin3 = "Admin3";
        public const string User = "User";
        public const string Monitoring = "Monitoring";
    }
}
