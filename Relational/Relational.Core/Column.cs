using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Relational.Core
{
    public class Column
    {
        public string id { get; set; }
        public string name { get; set; }
        public string field { get; set; }
        public Column() { }
        public Column(string id)
        {
            this.id = id;
            this.name = id;
            this.field = id;

        }
    }
    [DataContract]
    public class UIColumn
    {
        [DataMember(Name = "id", IsRequired = false)]
        public string id { get; set; }
        [DataMember(Name = "name", IsRequired = false)]
        public string name { get; set; }
        [DataMember(Name = "field", IsRequired = false)]
        public string field { get; set; }

        [DataMember(Name = "sortable", IsRequired = false)]
        public bool sortable { get; set; }


        [DataMember(Name = "formatter", IsRequired = false, EmitDefaultValue = false)]
        public string formatter { get; set; }

        [DataMember(Name = "hasTotal", IsRequired = false, EmitDefaultValue = false)]
        public bool hasTotal { get; set; }

        [DataMember(Name = "Total", IsRequired = false, EmitDefaultValue = false)]
        public decimal Total { get; set; }

        [DataMember(Name = "width", IsRequired = false, EmitDefaultValue = false)]
        public int width { get; set; }

        [DataMember(Name = "cssClass", IsRequired = false, EmitDefaultValue = false)]
        public string cssClass { get; set; }


        public UIColumn() { }
        public UIColumn(string id)
        {
            this.id = id;
            this.name = id;
            this.field = id;
            this.sortable = true;
        }





      
    }
}