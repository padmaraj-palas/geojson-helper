using System.Text;

namespace CSVHelper
{
    public static class CsvExtentions
    {
        private const string EmptyArray = "[]";

        public static string ToJson(this CsvRecord[] records)
        {
            if (records == null || records.Length == 0)
            {
                return EmptyArray;
            }

            StringBuilder sb = new StringBuilder("[\n");

            for (int r = 0; r < records.Length; r++)
            {
                sb.Append("{\n");

                var record = records[r];
                for (int i = 0; i < record.Keys.Length; i++)
                {
                    var key = record.Keys[i];
                    var value = record.Values[i];

                    sb.Append($"\"{key}\": ");
                    if (value.Length > 0 && (value[0] == '{' || value[0] == '['))
                    {
                        sb.Append(value
                            .Replace('\'', '"')
                            .Replace("True", "true")
                            .Replace("False", "false")
                            .Replace("None", "\"\"")
                            .Replace("\\xa0A", "")
                            .Replace("\\xa0B", "")
                            .Replace("\\xa0", ""));
                    }
                    else
                    {
                        if (value.Length > 0 && value[0] == '<')
                            value = value.Replace("\"", "\\\"");

                        sb.Append($"\"{value}\"");
                    }

                    if (i < record.Keys.Length - 1)
                        sb.Append(",");

                    sb.Append("\n");
                }

                sb.Append("}");
                if (r < records.Length - 1)
                    sb.Append(",");

                sb.Append('\n');
            }

            sb.Append("]\n");

            return sb.ToString();
        }
    }
}
