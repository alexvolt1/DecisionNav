using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace ManagementConsole.Components
{
    public class Security
    {

    [DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

    [DllImport("advapi32.dll", SetLastError = true)]
    public extern static bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, ref IntPtr TokenHandle);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private extern static bool CloseHandle(IntPtr handle);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
        int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
    public static WindowsImpersonationContext ImpersonateUser(string sUsername, string sDomain, string sPassword, ref string errStr)
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
        if (c != -1 && c != 0)
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
            // get handle to token 
            bool bImpersonated = LogonUser(sUsername, sDomain, sPassword,
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                ref pExistingTokenHandle);

            // did impersonation fail? 
            if (false == bImpersonated)
            {
                int nErrorCode = Marshal.GetLastWin32Error();
                errStr = "LogonUser() failed with error code: " +
                    nErrorCode + "\r\n";
                // show the reason why LogonUser failed 
                throw new Exception(errStr);
            }

            // Get identity before impersonation 
           // errStr += "Before impersonation: " +
           //     WindowsIdentity.GetCurrent().Name + "\r\n";

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
             //   errStr += "After impersonation: " +
              //     WindowsIdentity.GetCurrent().Name + "\r\n";


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
