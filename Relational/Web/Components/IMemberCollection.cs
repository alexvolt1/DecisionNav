using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Relational.Components
{

   public class ILevelItem
   {
       public string Name;
       public ILevelItem(string name)
       {
           this.Name = name;
       }

       public ILevelItem()
       {
           
       }

       public int HierId { get; set; }

       public int Level { get; set; }

       public bool Leaf { get; set; }

       public int Path { get; set; }

       public bool Exclusive { get; set; }
   }

   public class LevelItemExtended : ILevelItem
   {
       public LevelItemExtended(string name)
       {
           base.Name = name;
       }

       public LevelItemExtended()
       {
           
       }

       public string LevelName { get; set; }
      
       public string SqlExpression { get; set; }

       public string ParentLevel { get; set; }

       public string ParentName { get; set; }

       public string SourceTable { get; set; }

       public int listid { get; set; }

       public string orderfield { get; set; }

       public int fieldtype { get; set; }

       public bool HasNextStep { get; set; }

       public string RMin { get; set; }

       public string RMax { get; set; }
   }

}
