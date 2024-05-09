namespace CSharpPrintf;

using System.Text;

public class Formatter
{
    private readonly StringBuilder stringBuilder = new StringBuilder();

    public string Format(string format, params object[] args)
    {
        int argIndex = 0;
        stringBuilder.Clear();


        for (int i = 0; i < format.Length;)
        {
            char current = format[i];
            if (current == '%')
            {
                var fmt = FormatSpecifier.Read(format, i, out i);
                fmt.Format(stringBuilder, args, argIndex++);
            }
            else
            {
                stringBuilder.Append(current);
                i++;
            }
        }

        return stringBuilder.ToString();
    }

    /*
    public void Scan(string str, string format, out params object[] values)
    {
        throw new System.NotImplementedException();
    }
    */
}
