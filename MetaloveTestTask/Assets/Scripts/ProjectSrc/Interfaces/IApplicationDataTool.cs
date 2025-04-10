using System;

namespace Scripts.ProjectSrc
{
    public interface IApplicationDataTool
    {
        public static bool IsAppVersionALatest(string appVersionA, string appVersionB)
        {
            Version parsedAppVersionA;
            Version parsedAppVersionB;

            if (!Version.TryParse(appVersionA, out parsedAppVersionA)) return false;
            if (!Version.TryParse(appVersionB, out parsedAppVersionB)) return true;
            
            int comparison = parsedAppVersionA.CompareTo(parsedAppVersionB);
            if (comparison > 0) return true;

            return false;
        }
    }
}
