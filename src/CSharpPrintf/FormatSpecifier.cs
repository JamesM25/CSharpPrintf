namespace CSharpPrintf;

using System.Text;

internal enum FormatType
{
    Percent,
    Decimal,
    UnsignedDecimal,
    FloatFixed,
    FloatSci,
    FloatNorm,
    Hexadecimal,
    Octal,
    String,
    Char,
    Pointer,
    FloatHex,
    N,

    Undefined
}

[Flags]
internal enum FormatFlag
{
    None = 0,

    AlignLeft = 1 << 0,
    Plus = 1 << 1,
    Space = 1 << 2,
    Zero = 1 << 3,
    Apostrophe = 1 << 4,
    Hash = 1 << 5
}

internal class FormatSpecifier
{
    public FormatType type = FormatType.Undefined;
    public FormatFlag flags;
    public int index = -1;
    public bool isUpper = false;

    public void Format(StringBuilder dest, object[] args, int index)
    {
        switch (type)
        {
            case FormatType.Percent:
                dest.Append('%');
                return;
            case FormatType.Decimal:
                dest.Append((int)args[index]);
                return;
            case FormatType.String:
                dest.Append((string)args[index]);
                return;
            case FormatType.Char:
                dest.Append((char)args[index]);
                return;
        }

        throw new NotImplementedException();
    }

    private static FormatFlag GetFlag(char c)
    {
        switch (c)
        {
            case '-': return FormatFlag.AlignLeft;
            case '+': return FormatFlag.Plus;
            case ' ': return FormatFlag.Space;
            case '0': return FormatFlag.Zero;
            case '\'': return FormatFlag.Apostrophe;
            case '#': return FormatFlag.Hash;
            default: return FormatFlag.None;
        }
    }

    private static FormatType GetType(char c)
    {
        switch (c)
        {
            case '%':
                return FormatType.Percent;
            case 'd':
            case 'i':
                return FormatType.Decimal;
            case 'u':
                return FormatType.UnsignedDecimal;
            case 'f':
            case 'F':
                return FormatType.FloatFixed;
            case 'e':
            case 'E':
                return FormatType.FloatSci;
            case 'g':
            case 'G':
                return FormatType.FloatNorm;
            case 'x':
            case 'X':
                return FormatType.Hexadecimal;
            case 'o':
                return FormatType.Octal;
            case 's':
                return FormatType.String;
            case 'c':
                return FormatType.Char;
            case 'p':
                return FormatType.Pointer;
            case 'a':
            case 'A':
                return FormatType.FloatHex;
            case 'n': return FormatType.N;
        }

        return FormatType.Undefined;
    }

    private static int ReadNumber(string str, int startIndex, out int endIndex)
    {
        int num = 0;
        int index = startIndex;
        while (true)
        {
            char current = str[index];
            if (!char.IsAsciiDigit(current)) break;

            num = num * 10 + current - '0';
            index++;
        }

        endIndex = index;
        return num;
    }


    public static FormatSpecifier Read(string str, int startIndex, out int endIndex)
    {
        var symbol = new FormatSpecifier();
        int len = str.Length;
        int index = startIndex;
        endIndex = index;

        if (index > len) throw new IndexOutOfRangeException("startIndex must be less than the string's length");

        if (str[index] != '%') throw new ArgumentException("Symbol must begin with %");
        index++;

        if (char.IsAsciiDigit(str[index]))
        {
            int num = ReadNumber(str, index, out index);
        }

        while (index < len)
        {
            char current = str[index++];
            var type = GetType(current);

            if (type != FormatType.Undefined)
            {
                symbol.type = type;
                symbol.isUpper = char.IsUpper(current);
                endIndex = index;
                break;
            }
        }

        return symbol;
    }
}