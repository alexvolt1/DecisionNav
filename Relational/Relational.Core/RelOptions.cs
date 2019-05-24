using System;
using System.Collections.Generic;

namespace Relational.Core
{
    [Serializable]
  public  class RelOptions
    {
        public List<LevelItemExtended> items { get; set; }
        public List<string> logentries { get; set; }
    }
}
