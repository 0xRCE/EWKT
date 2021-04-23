namespace EWKT.Parsers
{
    public enum TokenType
    {
        Word,
        GeometryStartSeparator,
        GeometryEndSeparator,
        GeometryChildSeparator,
        CoordinateSeparator,
        DecimalSeparator,
        Minus,
        Number,
        Whitespace,
        Eol,
        Eof
    }
}