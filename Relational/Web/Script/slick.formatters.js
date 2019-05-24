/***
 * Contains basic SlickGrid formatters.
 * 
 * NOTE:  These are merely examples.  You will most likely need to implement something more
 *        robust/extensible/localizable/etc. for your use!
 * 
 * @module Formatters
 * @namespace Slick
 */

(function ($) {
  // register namespace
  $.extend(true, window, {
    "Slick": {
      "Formatters": {
        "PercentComplete": PercentCompleteFormatter,
        "PercentCompleteBar": PercentCompleteBarFormatter,
        "YesNo": YesNoFormatter,
        "YNCheck": YNCheckmark,
        "YesNoNACheck": YesNoNAFormatter,
        "YearQuoter": dateToYearQuoterFormatter,
        "DateTime": datetimeFormatter,
        "ShortDateTime": shortdatetimeFormatter,
        "ShortDate": shortdateFormatter,
        "QubeProcess":QubeProcessFormatter,
        "Checkmark": CheckmarkFormatter,
        "Dollar": DollarFormatter,
        "MonthYear":datemonthyearformatter
      }
    }
  });
  function datemonthyearformatter(row, cell, value, columnDef, dataContext) {
      
      var match;
      if (value != null && (match = value.match(/\d+/))) {
          var date = new Date();
          date.setTime(match[0] - 0);
          var result;
          var monthNames = ["January", "February", "March", "April", "May", "June",
                   "July", "August", "September", "October", "November", "December"];

          result = monthNames[date.getMonth()] + ' ' + date.getFullYear();
      }
      return (value == null || value === "NULL" || value =="" || value == "null") ? "All" : result;
  }
  function PercentCompleteFormatter(row, cell, value, columnDef, dataContext) {
    if (value == null || value === "") {
      return "-";
    } else if (value < 50) {
      return "<span style='color:red;font-weight:bold;'>" + value + "%</span>";
    } else {
      return "<span style='color:green'>" + value + "%</span>";
    }
  }


  function datetimeFormatter(row, cell, value, columnDef, dataContext) {
      var match;
      if (value != null && (match = value.match(/\d+/))) {
          var date = new Date();
          date.setTime(match[0] - 0);

          var result;
          result = date.toLocaleDateString() + " " + date.toLocaleTimeString();

      }
      return value == null ? "" : result;
  }
  function shortdateFormatter(row, cell, value, columnDef, dataContext) {
      var match;
   if (value != null && (match = value.match(/\d+/))) {
          
          var date = new Date();
          date.setTime(match[0] - 0);
          var result;
          
          if (date.getFullYear() < 1980 || date.getFullYear() > 2099) {
             result = "";
          }
          else {

              result = date.getMonth() + 1 +
                  "/" + ("0" + date.getDate()).slice(-2) +
                  "/" + date.getFullYear();
          }


      }
      return value == null ? "" : result;
  }

  function shortdatetimeFormatter(row, cell, value, columnDef, dataContext) {
      var match;
      if (value != null && (match = value.match(/\d+/))) {
          var date = new Date();
          date.setTime(match[0] - 0);

          var result;
          if (date.getFullYear() < 1980 || date.getFullYear() > 2099) {
              result = "";
          }
          else {
         
              result = date.getMonth() + 1 +
                  "/" + ("0" + date.getDate()).slice(-2) +
                  "/" + date.getFullYear() + " " + date.getHours() +
                  ":" + ("0" + date.getMinutes()).slice(-2) +
                  ":" + ("0" + date.getSeconds()).slice(-2);
          }
          

      }
      return value == null ? "" : result;
  }

  function dateToYearQuoterFormatter(row, cell, value, columnDef, dataContext) {
      var match;
      if (value!=null && (match = value.match(/\d+/))) {
      var date = new Date();
      date.setTime(match[0] - 0);

      var result;
      var year = date.getFullYear();
      var quoter;
      result =  year + ' Q_' + getQuarter(date);
      }
      return value == null ? "All" : result;
  }
 
  function QubeProcessFormatter(row, cell, value, columnDef, dataContext) {
     
      if (value == null || value === "") {
          return "";
      }

      var color;
      var formattedvalue;

      if (value == 0) {
          formattedvalue = "Suspended";
          color = "#eee";
      } else if (value == 1) {
          formattedvalue = "Waiting to Process";
          color = "#F3F5B5";
      } else if (value == 2) {
          formattedvalue = "Processing...";
          color = "#99D8FF";
      } else if (value == 3) {
          formattedvalue = "Processed Successfully";
          color = "#96FAB1";
      } else if (value == 9) {
          formattedvalue = "Processing Failed";
          color = "#FF8C8E";
      }
      else {
          formattedvalue = "Invalid";
          color = "#FAD2E6";
      }
      var cellformat= {
          bgColor:color,
          contentHtml: "<div style=' width:100%;height:100%;  cursor:pointer;'>" + formattedvalue + "</div>"
                      }

      return cellformat;
    //  return "<div style='background:" + color + "; width:100%;height:100%;  cursor:pointer;'>" + value + "</div>";
  }
  function PercentCompleteBarFormatter(row, cell, value, columnDef, dataContext) {
    if (value == null || value === "") {
      return "";
    }

    var color;

    if (value < 30) {
      color = "red";
    } else if (value < 70) {
      color = "silver";
    } else {
      color = "green";
    }

    return "<span class='percent-complete-bar' style='background:" + color + ";width:" + value + "%'></span>";
  }

  function YesNoFormatter(row, cell, value, columnDef, dataContext) {
    return value ? "Yes" : "No";
  }
  function YNCheckmark(row, cell, value, columnDef, dataContext) {
      
      
      //if (value != null && value != false && (value.toLowerCase() == "y" || value.toLowerCase() == "yes" || value.toLowerCase() == "1")) {
      //    return "<img src='images/tick.png'>";
      //}
      //else {
      //    return "";
      //}
      return (value != null && value!=false && value.toLowerCase() == "y") ? "<img src='images/tick.png'>" : "";
  }
  function YesNoNAFormatter(row, cell, value, columnDef, dataContext) {
    
      value = $.trim(value);
      if (value != null && value != false && value != true && value.toLowerCase() == "n/a") {
          return "N/A";
      }
      if (value != null && value != false && (value.toLowerCase() == "y" || value.toLowerCase() == "yes" || value.toLowerCase() == "1")) {
          return "<img src='images/tick.png'>";
      }
      else {
          return "";
      }


     // return value ? "<img src='images/tick.png'>" + value : "";
  }

  function CheckmarkFormatter(row, cell, value, columnDef, dataContext) {
     // debugger;
      if (value != null && value !=false && value!= true && value.toLowerCase()=="n/a") {
      return "N/A";
      }
      if (value == "0") {
          return "";
      }
       return value ? "<img src='images/tick.png'>" : "";
  }


  function DollarFormatter(row, cell, value, columnDef, dataContext) {
      if (value != null) {
          return format2(value, "$");
      }
      else {
          return value;
      }
        
  }


  function format1(n, currency) {
      return currency + " " + n.toFixed(2).replace(/./g, function (c, i, a) {
          return i > 0 && c !== "." && (a.length - i) % 3 === 0 ? "," + c : c;
      });
  }

  function format2(n, currency) {
      return currency + " " + n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
  }

  function getQuarter(d) {
      // d = d || new Date(); // If no date supplied, use today
      var q = [1, 2, 3, 4];
      return q[Math.floor(d.getMonth() / 3)];
  }
})(jQuery);
