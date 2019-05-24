<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Relational.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login Page</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <link href="Styles/fullheight.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function setFocus() {
            document.getElementById("uid").focus();
        }

// KyKaReKy


    </script>
</head>
<body onload="setFocus()">
    <form id="form1" runat="server">
        <div id="container">
            <div id="headerWrapper">
                <div id="siteHeader" style=" height: 55px;">
                    <div id="appName">
                        <asp:Label ID="lblAppName" runat="server" Text=""></asp:Label>
                    </div>
                    <div id="logo">
                        <a href="Default.aspx">
                            <img src="images/afs-logo.gif" alt="afs The Market Leader In Lending Solutions" /></a>
                    </div>
                    <div id="headerBlk">
                        <div id="headerNav" style="text-align: right;">
                            <img alt="" src='images/LogoSmall_AFS.gif' /><br />
                            <span style="color: #bbb;">
                                 <%= AssemblyProductVersion + " - " +System.Environment.GetEnvironmentVariable("COMPUTERNAME") %></span>
                        </div>
                    </div>
                </div>
                <div id="globalNav">
                </div>
            </div>
            <div id="content">
                <div style="margin: 30px; height: 400px;">
                    <table id="loginWrapper">
                        <tr>
                            <td colspan="2" style="font-size: 18px;">Log In to Your Account
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">Provide your username and password below to login:
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; vertical-align: bottom;">
                                <label class="loginLabel" for="USER">
                                    User Name:</label>
                            </td>
                            <td>
                                <asp:TextBox class="textBox" ID="uid" runat="server" MaxLength="150">demo</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; vertical-align: bottom;">
                                <label class="loginLabel" for="password">
                                    Password:</label>
                            </td>
                            <td>
                                <asp:TextBox class="textBox" ID="pwd" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>

                        <asp:Panel ID="pnlChangePassword" runat="server" Visible="false">
                            <tr>
                                <td style="text-align: right; vertical-align: bottom;">
                                    <label class="loginLabel" for="password">
                                        New  Password:</label>
                                </td>
                                <td>
                                    <asp:TextBox class="textBox" ID="npwd1" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>


                            <tr>
                                <td style="text-align: right; vertical-align: bottom;">
                                    <label class="loginLabel" for="password">
                                        Confirm   New  Password:</label>
                                </td>
                                <td>
                                    <asp:TextBox class="textBox" ID="npwd2" runat="server" TextMode="Password" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
 
                        </asp:Panel>


                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblLoginStatus" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Button ID="btnLogin" class="button" runat="server" Text="Sign In" OnClick="btnLogin_Click" />
                            </td>
                            <td>

                                &nbsp;</td>
                        </tr>
                    </table>


                </div>
            </div>
            <div id="footer">
                <div class="copyright">
                    <p>

                        <script type="text/javascript" src="Script/Footer.js">
                        </script>

                    </p>
                    <script type="text/javascript">
                        //  alert("DefaultDesktop is under construction");
                    </script>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

