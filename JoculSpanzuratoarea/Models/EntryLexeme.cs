using System;
using System.Collections.Generic;

namespace JoculSpanzuratoarea;

public partial class EntryLexeme
{
    public int Id { get; set; }

    public int EntryId { get; set; }
    public Entry Entry { get; set; }

    public int LexemeId { get; set; }
    public Lexeme Lexeme { get; set; }

    public int EntryRank { get; set; }

    public int LexemeRank { get; set; }

    public int Main { get; set; }

    public int CreateDate { get; set; }

    public int ModDate { get; set; }
}
