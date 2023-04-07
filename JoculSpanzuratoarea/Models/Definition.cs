using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class Definition
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte SourceId { get; set; }

    public string? Lexicon { get; set; }

    public string? InternalRep { get; set; }

    public byte Status { get; set; }

    public byte HasAmbiguousAbbreviations { get; set; }

    public string RareGlyphs { get; set; } = null!;

    public bool Structured { get; set; }

    public byte Volume { get; set; }

    public ushort Page { get; set; }

    public int CreateDate { get; set; }

    public int ModDate { get; set; }

    public int ModUserId { get; set; }
    public IList<EntryDefinition> EntryDefinitions { get; set; }

}
