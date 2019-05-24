using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Relational.Core.CONVERTERS;

namespace Relational.Core
{
   public class ReportMembersCollection
    {
       IRelationalReport reportsession;
      // private ReportHierarchy CurrentHierarchy{get;set;}
       public string SearchStrings = "";
       public ReportMembersCollection(IRelationalReport reportsession)
       {
           this.reportsession = reportsession;
           DATA.ReportData.GetKeywordSearchParams(reportsession);
         
       }

       public void GetMemberValues()
       {
           GetMemberValues(reportsession.Hierarchies.GetCurrent().ID, 0);
       }
       public void GetMemberValues(int level)
       {
           GetMemberValues(reportsession.Hierarchies.GetCurrent().ID, level);
       }
       public void GetMemberValues(int hierarhyid, int level)
       {
           DATA.ReportData.GetMemberValues(reportsession.Hierarchies.GetCurrent(), level, reportsession);
       }


       public object ToJson(Relational.Core.UserReport Report)
       {
           GetMemberValues(Report.Level);
           //dynamic MembersJSON = new System.Dynamic.ExpandoObject();
           //MembersJSON.Member = this.reportsession.ReportMembersCollection.Hierarchies[0].Members[Report.Level];
           //return MembersJSON.Member;
           return new { AlternateID = reportsession.AlternateReport==""?false:true
               , Members = reportsession.Hierarchies.GetCurrent().Members[Report.Level]
               , SearchQ = reportsession.KeywordSearch.SearchParams
           };
           //return  this.reportsession.ReportMembersCollection.Hierarchies[0].Members[Report.Level];
       }
       public object ToJson()
       {
           //dynamic MembersJSON = new System.Dynamic.ExpandoObject();
           //MembersJSON.Members = this.reportsession.ReportMembersCollection.Hierarchies[0].Members.ToList();
           //return MembersJSON.Members;
          // return this.reportsession.ReportMembersCollection.Hierarchies[0].Members.ToList();
           return new { AlternateID = reportsession.AlternateReport == "" ? false : true
               , Members = reportsession.Hierarchies.GetCurrent().Members.ToList()
               , SearchQ = reportsession.KeywordSearch.SearchParams};
       }


       internal void ParseMembers(List<Memsel> selectedmembers)
       {
           if (selectedmembers == null)
               return;

           if (selectedmembers.Count == 0)
           {
               var memberscount = reportsession.Hierarchies.GetCurrent().Members.Count;
               for (int i = 0; i < memberscount; i++)
               {
                   var vs = reportsession.Hierarchies.GetCurrent().Members[i].values;
                 if (vs != null)
                 {
                     foreach(var v in vs){
                         v.Selected = false;
                     }
                 }
               }
           }
            foreach (Memsel member in selectedmembers)
           {
               int level = Memsel.GetLevel(member);





               var servermembervalues = reportsession.Hierarchies.GetCurrent().Members[level].values;
               if (servermembervalues != null)
               {
                   foreach (var value in servermembervalues)
                   {
                       value.Selected = false;
                   }
                   foreach (var uservalue in member.values)
                   {
                       var match = servermembervalues.Where(s1 => s1.Text == uservalue).FirstOrDefault();
                       if (match != null)
                       {
                           match.Selected = true;
                       }
                   }
                   int nextlevel = level + 1;
                   var memberscount = reportsession.Hierarchies.GetCurrent().Members.Count;
                   for (int i = nextlevel; i < memberscount; i++)
                   {
                       var m = reportsession.Hierarchies.GetCurrent().Members[i].values;
                       if (m != null)
                       {
                           foreach (var v in m)
                           {
                               v.Selected = false;
                           }
                       }
                   }

               }
           }
       }

       internal string ToSql()
       {
           List<string> nodes = new List<string>();

           var members = reportsession.Hierarchies.GetCurrent().Members;
           foreach (var member in members)
           {
               if (member.values != null)
               {
                   var selectedvalues = member.values.Where(s1 => s1.Selected).ToList();
                   if (selectedvalues != null && selectedvalues.Count > 0)
                   {
                       List<string> values = new List<string>();
                       foreach (var value in selectedvalues)
                       {
                           values.Add(value.Text.Replace("'","''"));
                       }
                       string node = string.Format("{0} in ('{1}')", SQLHelper.WrapSQLName( member.Name), string.Join("','", values.ToArray()));
                       nodes.Add(node);
                   }
               }
           }
           return string.Join(" AND ", nodes.ToArray()); 
       }

       internal string SearchStringsToSQL(List<Memsel> searchstrings)
       {
           if (reportsession.KeywordSearch.SearchParams == null)
               return "";
           if (searchstrings == null)
               return "";

           List<string> nodes = new List<string>();
           foreach (var ss in searchstrings)
           {
               List<string> values = new List<string>();
               string Node = "";
              
               Node = reportsession.KeywordSearch.SearchParams.Where(x=>x.ID.ToString()==ss.name).FirstOrDefault().ColumnName;
             
               if (ss.level  == "Selected")
               {
                   if (ss.values != null && ss.values.Length > 0)
                   {
                       foreach (string s in ss.values)
                       {
                           values.Add(s.Sanitize().Replace("'", "''"));
                       }
                       string n = string.Format("{0} in ('{1}')", SQLHelper.WrapSQLName(Node), string.Join("','", values.ToArray()));
                       nodes.Add(n);
                   }
               }
               else if (ss.level == "ALL")
               {
                   nodes.Add(String.Format("({0} like '%{1}%')", SQLHelper.WrapSQLName(Node), ss.values[0].Sanitize().Replace("'", "''")));
               }
           }

           SearchStrings = string.Join(" AND ", nodes.ToArray()).Trim();
           return SearchStrings;
       }


       internal void Clear()
       {
           throw new NotImplementedException();
       }
    }

   public class ReportMember
   {
       public string HierarhyId { get; set; }
       public int Level { get; set; }
       public string Name { get; set; }
       public List<ReportMemberValue> values { get; set; }

       internal void AddValue(ReportMemberValue membervalue)
       {
           if (values == null)
           {
               values = new List<ReportMemberValue>();
           }
           values.Add(membervalue);
       }
   }
   public class ReportMemberValue
   {
     public string Text { get; set; }
     public bool Selected { get; set; }
   }
}
