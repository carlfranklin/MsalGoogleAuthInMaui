using Newtonsoft.Json;
using System.Text;

namespace MsalAuthInMaui
{
    public static class Extensions
    {
        public static string[] ToStringArray(this NestedSettings[] nestedSettings)
        {
            string[] result = new string[nestedSettings.Length];

            for (int i = 0; i < nestedSettings.Length; i++)
            {
                result[i] = nestedSettings[i].Value;
            }

            return result;
        }
    }
}
