<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Relational.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .header ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #333;
            position: fixed;
            top: 0;
            width: 100%;
        }
            .header ul li {
                float: left;
                border-right: 1px solid #bbb;
            }
                .header ul li:last-child {
                    border-right: none;
                }

                .header ul li a, .header ul li span {
                    display: block;
                    color: white;
                    text-align: center;
                    padding: 14px 16px;
                    text-decoration: none;
                }

                    .header ul li a:hover:not(.active) {
                        background-color: #111;
                    }

            .header ul .active {
                background-color: #4CAF50;
            }

        .container {
            margin-top: 60px;
        }

            .container ul {
                list-style-type: none;
                margin: 0;
                padding: 0;
                width: 400px;
                background-color: #f1f1f1;
            }

                .container ul li a {
                    display: block;
                    color: #000;
                    padding: 3px 6px;
                    text-decoration: none;
                }

                    .container ul li a:hover {
                        background-color: #555;
                        color: white;
                    }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="header">
            <ul>
                <!--<li><a href="default.asp">Home</a></li>
        <li><a href="news.asp">News</a></li>-->
                <li style="float: right"><a href="Logout.aspx">Logout</a></li>
                <li style="float: right"><span><%=DeciwebProperties.DeciwebSession.Current.DisplayUserName  %></span></li>

            </ul>
        </div>

        <div class="container">

             <ul>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=4&b=0">Obligor List</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=5&b=0">Credit Products By Obligor</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=14&b=0">Loan Products By Obligor</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=7&b=0">Largest Obligors By Exposure</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=6&b=0">Largest Obligors By Loan Size</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=8&b=0">Industry Groupings</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=1&b=0">Highest / Lowest Rates</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=9&b=0">Comparable Deals (DM_Fac)</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=10&b=0">Comparable Deals (DM_Loan)</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=11&b=0">Risk Rating Upgrades/Downgrades (Facilities)</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=12&b=0">Risk Rating Upgrades/Downgrades (Loans)</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=3&b=0">Delinquent Obligors</a></li>
            </ul>
            <hr />
            <ul>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=101&b=0">Active Unit Query</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=102&b=0">Completed Event Query</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=103&b=0">In Process Event Query</a></li>
                <li><a href="Workstation/Reports/RelReport5.aspx?a=104&b=0">Loan Operations Query</a></li>
            </ul>


             <ul>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=4&b=0">Obligor List</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=5&b=0">Credit Products By Obligor</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=14&b=0">Loan Products By Obligor</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=7&b=0">Largest Obligors By Exposure</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=6&b=0">Largest Obligors By Loan Size</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=8&b=0">Industry Groupings</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=1&b=0">Highest / Lowest Rates</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=9&b=0">Comparable Deals (DM_Fac)</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=10&b=0">Comparable Deals (DM_Loan)</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=11&b=0">Risk Rating Upgrades/Downgrades (Facilities)</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=12&b=0">Risk Rating Upgrades/Downgrades (Loans)</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=3&b=0">Delinquent Obligors</a></li>
            </ul>
            <hr />
            <ul>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=101&b=0">Active Unit Query</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=102&b=0">Completed Event Query</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=103&b=0">In Process Event Query</a></li>
                <li><a href="Workstation/Reports/RelReport4.aspx?a=104&b=0">Loan Operations Query</a></li>
            </ul>


<hr />

            <ul>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=4&b=0">Obligor List</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=5&b=0">Credit Products By Obligor</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=14&b=0">Loan Products By Obligor</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=7&b=0">Largest Obligors By Exposure</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=6&b=0">Largest Obligors By Loan Size</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=8&b=0">Industry Groupings</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=1&b=0">Highest / Lowest Rates</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=9&b=0">Comparable Deals (DM_Fac)</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=10&b=0">Comparable Deals (DM_Loan)</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=11&b=0">Risk Rating Upgrades/Downgrades (Facilities)</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=12&b=0">Risk Rating Upgrades/Downgrades (Loans)</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=3&b=0">Delinquent Obligors</a></li>
            </ul>
            <hr />
            <ul>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=101&b=0">Active Unit Query</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=102&b=0">Completed Event Query</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=103&b=0">In Process Event Query</a></li>
                <li><a href="Workstation/Reports/RelReport3.aspx?a=104&b=0">Loan Operations Query</a></li>
            </ul>




       <hr />
            Other controls prototypes
        <hr />
            <ul>
                <li>
                    <a href="Workstation/Reports/MemberSelectDialog.html">MemberSelectDialog</a>
               </li>
               <li>
                    <a href="Workstation/Reports/FiltersDialog.html">FiltersDialog</a>
               </li>
                               <li>
                    <a href="Workstation/Reports/Dialogs.html">Dialogs</a>
               </li>
            </ul>


        </div>
    </form>
</body>
</html>
