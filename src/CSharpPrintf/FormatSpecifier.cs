namespace CSharpPrintf;

using System.Globalization;
using System.Text;

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

internal enum FormatLength
{
    Default,
    hh,
    h,
    l,
    ll,
    L,
    z,
    j,
    t
}

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

internal class FormatSpecifier
{
    public FormatType type = FormatType.Undefined;
    public FormatFlag flags;
    public FormatLength length;
    public int index = -1;
    public bool hasPrecision = false;
    public int precision = 6;
    public bool isUpper = false;
    public int width;
    public bool readWidth;

    public int Format(StringBuilder dest, object[] args, int index)
    {
        int startLength = dest.Length;
        int offsetPadding = 0;

        if (readWidth)
        {
            width = (int)args[index];
            index++;
        }
        
        switch (type)
        {
            case FormatType.Percent:
                dest.Append('%');
                break;
            case FormatType.Decimal:
            {
                long num = Convert.ToInt64(args[index]);

                if ((flags & FormatFlag.Plus) != 0 && num >= 0) dest.Append('+');
                else if ((flags & FormatFlag.Space) != 0 && num >= 0) dest.Append(' ');

                dest.Append(num);
                index++;
                break;
            }
            case FormatType.UnsignedDecimal:
            {
                ulong num = Convert.ToUInt64(args[index]);
                dest.Append(num);
                index++;
                break;
            }
            case FormatType.FloatFixed:
            {
                double num = Convert.ToDouble(args[index]);

                if ((flags & FormatFlag.Plus) != 0 && num >= 0) dest.Append('+');
                else if ((flags & FormatFlag.Space) != 0 && num >= 0) dest.Append(' ');

                dest.Append(num.ToString($"{(isUpper ? 'F' : 'f')}{precision}", CultureInfo.InvariantCulture));
                index++;
                break;
            }
            case FormatType.FloatSci:
            {
                double num = Convert.ToDouble(args[index]);

                if ((flags & FormatFlag.Plus) != 0 && num >= 0) dest.Append('+');
                else if ((flags & FormatFlag.Space) != 0 && num >= 0) dest.Append(' ');

                dest.Append(num.ToString($"{(isUpper ? 'E' : 'e')}{precision}", CultureInfo.InvariantCulture));
                index++;
                break;
            }
            case FormatType.FloatNorm:
            {
                double num = Convert.ToDouble(args[index]);

                if ((flags & FormatFlag.Plus) != 0 && num >= 0) dest.Append('+');
                else if ((flags & FormatFlag.Space) != 0 && num >= 0) dest.Append(' ');
                
                dest.Append(num.ToString($"{(isUpper ? 'G' : 'g')}{precision}", CultureInfo.InvariantCulture));
                index++;
                break;
            }
            case FormatType.Hexadecimal:
            {
                long num = Convert.ToInt64(args[index]);

                if ((flags & FormatFlag.Hash) != 0)
                {
                    dest.Append(isUpper ? "0X" : "0x");

                    if ((flags & FormatFlag.Zero) != 0)
                    {
                        // Padding zeros are applied after the "0x" prefix, but padding spaces are not
                        offsetPadding = 2;
                    }
                }
                
                dest.Append(num.ToString(isUpper ? "X" : "x"));
                index++;
                break;
            }
            case FormatType.Octal:
            {
                long num = Convert.ToInt64(args[index]);

                if ((flags & FormatFlag.Hash) != 0)
                {
                    dest.Append('0');

                    if ((flags & FormatFlag.Zero) != 0)
                    {
                        // Padding zeros are applied after the "0" prefix, but padding spaces are not
                        offsetPadding = 1;
                    }
                }
                
                dest.Append(Convert.ToString(num, 8));
                index++;
                break;
            }
            case FormatType.String:
                if (hasPrecision) dest.Append((string)args[index], 0, precision);
                else dest.Append((string)args[index]);
                index++;
                break;
            case FormatType.Char:
                dest.Append((char)args[index]);
                index++;
                break;
            default:
                throw new NotImplementedException();
        }

        int pad = width - (dest.Length - startLength);
        if (pad > 0)
        {
            int insertIndex = (flags & FormatFlag.AlignLeft) != 0 ? dest.Length : startLength;
            insertIndex += offsetPadding;

            string padContent = (flags & FormatFlag.Zero) != 0 ? "0" : " ";
            dest.Insert(insertIndex, padContent, pad);
        }

        return index;
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

    private static FormatFlag ReadFlags(string str, int startIndex, out int endIndex)
    {
        int index = startIndex;
        FormatFlag flags = FormatFlag.None;

        while (true)
        {
            FormatFlag currentFlag = GetFlag(str[index]);
            if (currentFlag == FormatFlag.None) break;

            flags |= currentFlag;
            index++;
        }

        // Left alignment overrides padding zeros
        if ((flags & FormatFlag.AlignLeft) != 0) flags &= ~FormatFlag.Zero;

        endIndex = index;
        return flags;
    }

    private static int ReadWidth(string str, int startIndex, out int endIndex)
    {
        if (str[startIndex] == '*')
        {
            endIndex = startIndex + 1;
            return -1;
        }

        return ReadNumber(str, startIndex, out endIndex);
    }

    private static FormatLength ReadLength(string str, int startIndex, out int endIndex)
    {
        char firstChar = str[startIndex];
        bool repeat = str.Length > startIndex + 1 && firstChar == str[startIndex + 1] && (firstChar == 'h' || firstChar == 'l');

        endIndex = startIndex + (repeat ? 2 : 1);

        switch (firstChar)
        {
            case 'h': return repeat ? FormatLength.hh : FormatLength.h;
            case 'l': return repeat ? FormatLength.ll : FormatLength.l;
            case 'L': return FormatLength.L;
            case 'z': return FormatLength.z;
            case 'j': return FormatLength.j;
            case 't': return FormatLength.t;
            default:
                endIndex = startIndex;
                return FormatLength.Default;
        }
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
        var format = new FormatSpecifier();
        int len = str.Length;
        int index = startIndex;
        endIndex = index;

        if (index > len) throw new IndexOutOfRangeException("startIndex must be less than the string's length");

        if (str[index] != '%') throw new ArgumentException("Format specifier must begin with %");
        index++;

        format.flags = ReadFlags(str, index, out index);
        
        format.width = ReadWidth(str, index, out index);
        if (format.width < 0) format.readWidth = true;

        if (str[index] == '.')
        {
            format.precision = ReadNumber(str, index + 1, out index);
            format.hasPrecision = true;
        }
        
        format.length = ReadLength(str, index, out index);

        while (index < len)
        {
            char current = str[index++];
            var type = GetType(current);

            if (type != FormatType.Undefined)
            {
                format.type = type;
                format.isUpper = char.IsUpper(current);
                endIndex = index;
                break;
            }
        }

        return format;
    }
}