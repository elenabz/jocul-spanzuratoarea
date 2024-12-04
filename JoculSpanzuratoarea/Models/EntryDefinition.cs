using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class EntryDefinition
{
    public int Id { get; set; }
    public int EntryId { get; set; }
    public Entry Entry { get; set; }
    public int DefinitionId { get; set; }
    public Definition Definition { get; set; }
    public int CreateDate { get; set; }
}
