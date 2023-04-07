using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class Lexeme
{
    public int Id { get; set; }

    public string Form { get; set; } = null!;

    public string FormNoAccent { get; set; } = null!;

    public string FormUtf8General { get; set; } = null!;

    public string Reverse { get; set; } = null!;

    public int? Number { get; set; }

    public string Description { get; set; } = null!;

    public int NoAccent { get; set; }

    public int ConsistentAccent { get; set; }

    public float? Frequency { get; set; }

    public string? Hyphenations { get; set; }

    public string? Pronunciations { get; set; }

    public int StopWord { get; set; }

    public int Compound { get; set; }

    public string ModelType { get; set; } = null!;

    public string ModelNumber { get; set; } = null!;

    public string Restriction { get; set; } = null!;

    public sbyte StaleParadigm { get; set; }

    public string Notes { get; set; } = null!;

    public sbyte HasApheresis { get; set; }

    public sbyte HasApocope { get; set; }

    public int CreateDate { get; set; }

    public int ModDate { get; set; }
    public IList<EntryLexeme> EntryLexemes { get; set; }

}
