using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS.Logs;
namespace Relational.Core
{
  public class ReportHierarchies
    {
      public List<ReportHierarchy> ReportHierarchiesCollection;
      IRelationalReport _reportsession;
      public ReportHierarchies(IRelationalReport reportsession)
      {
          _reportsession = reportsession;
          ReportHierarchiesCollection = new List<ReportHierarchy>();
          DATA.ReportData.GetHierarchies(ReportHierarchiesCollection, reportsession);
      }
      public ReportHierarchy GetCurrent()
      {
          if (ReportHierarchiesCollection == null)
          {
              throw new Exception("No hierarchies");
          }
          ReportHierarchy h = ReportHierarchiesCollection.Where(x => x.isCurrent == true).FirstOrDefault();
          if (h == null)
          {
              h = ReportHierarchiesCollection.Where(x => x.ID == 0).FirstOrDefault();
          }
          return h;
      }
      
      public ReportHierarchy SetCurrent(int id)
      {
          if (ReportHierarchiesCollection == null)
          {
              throw new Exception("No hierarchies");
          }
          ReportHierarchy current = GetCurrent();
          ReportHierarchy h = ReportHierarchiesCollection.Where(x => x.ID == id).FirstOrDefault();
          if (h != null)
          {
              current.isCurrent = false;
              h.isCurrent = true;
              h.ResetMembers();
              _reportsession.ReportMembersCollection.GetMemberValues();
              return h;
          }
          else
          {
              return current;
          }
         
      }
    }
  public class ReportHierarchy
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string SourceTable { get; set; }
        public bool isCurrent { get; set; }
        public List<ReportMember> Members { get; set; }
        public void AddMember(ReportMember member)
        {
            if (Members == null)
            {
                Members = new List<ReportMember>();
            }
            Members.Add(member);
        }

        internal void ResetMembers()
        {
            if (Members != null && Members.Count > 0)
            {
                foreach (ReportMember m in Members)
                {
                    if (m.values != null)
                    {
                        m.values.Clear();
                    }
                   
                }
            }
        }
    }
}
