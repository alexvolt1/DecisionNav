using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relational.Core
{

    public class KeywordSearch
    {
        public List<KeywordSearchItem> SearchParams { get; set; }
        public void Add(KeywordSearchItem item)
        {
            if (SearchParams == null)
            {
                SearchParams = new List<KeywordSearchItem>();
            }
            SearchParams.Add(item);
        }
    }
     public class KeywordSearchItem
    {
         public int ID { get; set; }
         public string TableName { get; set; }
         public string ColumnName { get; set; }
         public KeywordSearchItem(int id, string tablename, string columnname)
         {
             this.ID = id;
             this.TableName = tablename;
             this.ColumnName = columnname;
         }
    }
}
