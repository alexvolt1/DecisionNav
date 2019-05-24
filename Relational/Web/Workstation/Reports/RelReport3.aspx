<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RelReport3.aspx.cs" Inherits="Relational.Workstation.Reports.RelReport3" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>New Relational Control</title>
    <link href="../../Scripts/default.css"  rel="stylesheet" type="text/css"/>
    <link href="../../Scripts/toolbar.css" rel="stylesheet" />
    <link href="../../Scripts/slick.grid.css" rel="stylesheet" type="text/css"/>
    <link href="../../Scripts/plugins/TotalsPlugin.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/slick.pager.css" rel="stylesheet" type="text/css"/>
    <link href="../../Scripts/jquery-ui-1.8.24.custom.css" rel="stylesheet" type="text/css"/>

    
    <link href="../../Scripts/slick.columnpicker.css" rel="stylesheet" type="text/css"/>
    <link href="../../Scripts/relational.css" rel="stylesheet" />

    <link href="../../Scripts/plugins/filtersdialog.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/memberselectdialog.css" rel="stylesheet" />

        <script type="text/javascript" src="../../Scripts/firebugx.js"></script>
        <script type="text/javascript" src="../../Scripts/jquery-1.7.min.js"></script>
        <script type="text/javascript" src="../../Scripts/jquery-ui-1.8.24.custom.min.js"></script>
        <script type="text/javascript" src="../../Scripts/jquery.FileDownload.js"></script>
        <script type="text/javascript" src="../../Scripts/jquery.getscrollbarwidth.js"></script>
        <script type="text/javascript" src="../../Scripts/jquery.event.drag-2.2.js"></script>
        <script type="text/javascript" src="../../Scripts/jquery.mousewheel.js"></script>
        <script type="text/javascript" src="../../Scripts/slick.core.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/slick.editors.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/slick.formatters.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/slick.rowselectionmodel.js"></script>
        <script type="text/javascript" src="../../Scripts/slick.grid.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/slick.dataview.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/slick.pager.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/TotalsDataViewVirtual.js"></script>
        <script type="text/javascript" src="../../Scripts/plugins/TotalsPluginVirtual.js"></script>
        <script type="text/javascript" src="../../Scripts/relational-2.0.js"></script>
       
        <script src="../../Scripts/plugins/memberselectdialog.js"></script>
        <script src="../../Scripts/plugins/filterdialog.js"></script>
        

        <style>
        .cell-title {
            font-weight: bold;
        }

        .cell-effort-driven {
            text-align: center;
        }

        .cell-selection {
            border-right-color: silver;
            border-right-style: solid;
            background: #f5f5f5;
            color: gray;
            text-align: right;
            font-size: 10px;
        }

        .slick-row.selected .cell-selection {
            background-color: transparent; /* show default selected row background */
        }


            #contextMenu {
      background: #e1efc7;
      border: 1px solid gray;
      padding: 2px;
      display: inline-block;
      min-width: 100px;
      -moz-box-shadow: 2px 2px 2px silver;
      -webkit-box-shadow: 2px 2px 2px silver;
      z-index: 99999;
    }

    #contextMenu li {
      padding: 4px 4px 4px 14px;
      list-style: none;
      cursor: pointer;
      background: url("../images/arrow_right_peppermint.png") no-repeat center left;
    }

    #contextMenu li:hover {
      background-color: white;
    }


    </style>

      <style>
        #debug {
            /*display:none;*/ 
             width: 250px;
             height: 300px;
             position:absolute;
             z-index:10000000;
             right:50px;
             bottom:150px;
             font-size:10px;
             padding:2px;
        }
                #debug p { 
               padding:0;
               margin:0;
        }
      </style>
      <script>
      $( function() {
    $( "#debug" ).draggable();
       } );
     </script>

</head>
<%--<body oncontextmenu="return false;">--%>

<body>
   
       <!-- Toolbar -->
    <table id="Toolbar">
        <tbody>
            <tr>
                <td>
                    <div class="moduleHeader"><span class="pageTitleText !important" id="titleSpan"><%=ReportName %><br/><%=ReportDescription%></span></div>
                </td>

                <td class="ProcessInstruction">&nbsp;&nbsp;</td>
                <td style="WHITE-SPACE: nowrap; TEXT-ALIGN: right">
                    <!-- Toolbar buttons and Drop-down menu -->
                    <button name="BtnMSDialog" title="Member Select Dialog" class="iconButton tileList" id="btnmselect" type="button""><span></span></button>
                    <button name="BtnFilter" title="Filters" class="iconButton filter" id="btnfilters" type="button"><span></span></button>
                    <button name="BtnExcelReport" title="Excel Report" class="iconButton exportExcel" id="Img2" type="button" alt="Excel Report"><span></span></button>
                </td>
            </tr>
        </tbody>
    </table>
    <div id="gridcontainer"></div>
    <div id="pager"></div>


    <ul id="contextMenu" style="display: none; position: absolute">
        <li id="cExport" class="exportExcel">Export To Excel</li>
        <li id="cMems" class="tileList">Show Member Select</li>
        <li id="cFilts"class="filter">Show Report Filters</li>
   </ul>
   
        <!-- MemSel  -->
   <div id="memsel"></div>
   <!-- Filters  -->
   <div id="filterselect"></div>   

        <script type="text/javascript">
            //<!--
            $(document).ready(function () {
                $.ajaxSetup({ cache: false });
                var relreport = new RelReport();
                var topicid = "<%=TopicId%>";
                relreport.init(topicid);
                $("#Img2").on("click", function () {
                    var w = window.open("RelToExcel.ashx?id=" + topicid, "", "width=700,height=150");
                });
            });
</script>
    <%if(isDebugging){ %>
       <!-- Console-->
    <div id="debug" class="ui-widget-content">
    <a href="#">Clear</a>
     </div>
    <%} %>

    <div id="overlay-inAbox" class="overlay">
        <div class="wrapper">
            Connection with the Server was interrupted. Please <span class="toolbar"><a class="close" href="../DefaultDesktop.aspx">Reload the page</a></span>
        </div>
    </div>
</body>
</html>
