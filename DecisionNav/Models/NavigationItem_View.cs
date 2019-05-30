using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DecisionNav.Models
{
    [Table("NavigationItem_View")]
    public class NavigationItem_View
    {
        public int Id { get; set; }
        public string ClientID { get; set; }
        public string TopicId { get; set; }
        public string ViewId { get; set; }
        public string RType { get; set; }
    }
}
