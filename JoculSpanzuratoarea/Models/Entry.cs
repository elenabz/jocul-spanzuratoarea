using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class Entry
{
    public int Id { get; set; }
    public string Description { get; set; } = null!;
    public int CreateDate { get; set; }
    public bool Usable { get; set; }
    public IList<EntryDefinition> EntryDefinitions { get; set; }
}
