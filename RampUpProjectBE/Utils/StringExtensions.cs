using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RampUpProjectBE.Utils {
    public static class StringExtensions {
        /// <summary>
        /// Convert from a SecureString to a regular string.
        /// </summary>
        /// <param name="str">SecureString to be converted</param>
        /// <returns>string equivalent of the input SecureString.</returns>
        public static string ConvertToUnsecureString(this SecureString str) {
            if (str == null) {
                throw new ArgumentNullException("string value");
            }

            IntPtr unmanagedString = IntPtr.Zero;
            try {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(str);
                return Marshal.PtrToStringUni(unmanagedString);
            } finally {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Convert from a regular string to a SecureString.
        /// </summary>
        /// <param name="str">SecureString to be converted</param>
        /// <returns>SecureString equivalent of the input string.</returns>
        public static SecureString ConvertToSecureString(this string password) {
            if (string.IsNullOrWhiteSpace(password)) {
                return null;
            } else {
                SecureString result = new SecureString();
                foreach (char c in password.ToCharArray())
                    result.AppendChar(c);
                return result;
            }
        }
    }
}