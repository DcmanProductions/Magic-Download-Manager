using System.Linq;

namespace com.drewchaseproject.MDM.Library.Utilities
{
    public class DataUtility
    {
        public static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }

        public static string GetValidComponentName(string input)
        {
            string output = input;
            char[] illegal = " ~`!@#$%^&*()_+1234567890-={}[]\\|';:\"?></.,".ToCharArray();
            foreach (char v in illegal)
            {
                output = output.Replace(v + "", "");
            }
            return output;
        }
    }
}
