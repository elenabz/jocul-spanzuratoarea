using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class Entry
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int StructStatus { get; set; }

    public int StructuristId { get; set; }

    public int Adult { get; set; }

    public int MultipleMains { get; set; }

    public int CreateDate { get; set; }

    public int ModDate { get; set; }

    public int ModUserId { get; set; }
    public bool Usable { get; set; }
    public IList<EntryDefinition> EntryDefinitions { get; set; }
    public IList<EntryLexeme> EntryLexemes { get; set; }
}
