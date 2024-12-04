using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class Definition
{
    public int Id { get; set; }
    public string? Lexicon { get; set; }
    public string? InternalRep { get; set; }
    public byte Status { get; set; }
    public int CreateDate { get; set; }
    public IList<EntryDefinition>? EntryDefinitions { get; set; }

}
