using System;
namespace Relational.Core
{
   public interface IUserReport
    {
        int ReportId { get; set; }
        bool UseAlternateReport { get; set; }
        System.Collections.Generic.List<Memsel> SelectedFilters { get; set; }
        System.Collections.Generic.List<Memsel> SelectedMembers { get; set; }
        System.Collections.Generic.List<Memsel> SelectedSearchStrings { get; set; }
        int Level { get; set; }
    }
}
