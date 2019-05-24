using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;
namespace RelationalInternal
{
    public class SecureConnection
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        public extern static bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr TokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool RevertToSelf();

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);




        internal static SqlConnection GetSecureConnection(string connString, string GroupName, string user, string password)
        {
            // Impersonate the user.. If the call fail, then cache the impersonation
            // context so that later we can revert impersonation.
            WindowsImpersonationContext currImpContext = null;
            SqlConnection conn = null;
            string errorString = "";
            if (null == currImpContext)
            {
                try
                {
                    currImpContext = ImpersonateUser(user, System.Configuration.ConfigurationManager.AppSettings["domainString"], password, ref errorString);
                }
                catch (ApplicationException ex)
                { throw new Exception("Error impersonating user", ex); }
            }
            if (null != currImpContext)
            {
                try
                {
                    conn = new SqlConnection(connString);

                }
                catch (Exception ex)
                {
                    throw new Exception("Error opening secure database connection for connection string: [" + connString + "]", ex);
                }
            }
            return conn;
        }

        internal static WindowsImpersonationContext ImpersonateUser(string sUsername, string sDomain, string sPassword, ref string errStr)
        {
            // initialize tokens 
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;

            // if domain name was blank, assume local machine 
            if (sDomain == "")
                sDomain = System.Environment.MachineName;
            int c = sUsername.LastIndexOf('@');
            if (c != -1 && c !=0)
            {
                sUsername = sUsername.Substring(0, c);
            }
            try
            {
                //string sResult = null; 

                const int LOGON32_PROVIDER_DEFAULT = 0;
                // create token 
                const int LOGON32_LOGON_INTERACTIVE = 2;
                //const int LOGON32_LOGON_NETWORK = 3;
                const int SecurityImpersonation = 2;



                // We save that initial identity to be able to restore it later

                WindowsIdentity _initialIdentity = WindowsIdentity.GetCurrent();
                // We call the API "RevertToSelf" to undo the impersonation configured in the web.config:
                RevertToSelf();

                // get handle to token 
                bool bImpersonated = LogonUser(sUsername, sDomain, sPassword,
                    LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                    ref pExistingTokenHandle);

                // did impersonation fail? 
                if (false == bImpersonated)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    errStr = "LogonUser() failed with error code: " +
                        nErrorCode +  "  user: " + sUsername + " password: " + sPassword + " Domain: " + sDomain +   "\r\n" ;
                    // show the reason why LogonUser failed 
                    throw new Exception(errStr);
                }

                // Get identity before impersonation 
                errStr += "Before impersonation: " +
                    WindowsIdentity.GetCurrent().Name + "\r\n";

                bool bRetVal = DuplicateToken(pExistingTokenHandle,
                    SecurityImpersonation,
                    ref pDuplicateTokenHandle);

                // did DuplicateToken fail? 
                if (false == bRetVal)
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    // close existing handle 
                    CloseHandle(pExistingTokenHandle);
                    errStr += "DuplicateToken() failed with error code: "
                        + nErrorCode + "\r\n";

                    // show the reason why DuplicateToken failed 
                    throw new Exception(errStr);
                }
                else
                {
                    // create new identity using new primary token 
                    WindowsIdentity newId = new WindowsIdentity
                        (pDuplicateTokenHandle);
                    WindowsImpersonationContext impersonatedUser =
                        newId.Impersonate();

                    // check the identity after impersonation 
                    errStr += "After impersonation: " +
                        WindowsIdentity.GetCurrent().Name + "\r\n";

                    Trace.Write(errStr);
                    return impersonatedUser;
                }
            }
            finally
            {
                // close handle(s) 
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }
    }
}