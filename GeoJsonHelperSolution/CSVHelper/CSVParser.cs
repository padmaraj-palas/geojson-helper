using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVHelper
{
    public static class CSVParser
    {
        private const char Comma = ',';
        private const char DoubleQuote = '"';

        public static CsvRecord[] Parse(string filePath)
        {
            IList<CsvRecord> records = new List<CsvRecord>();

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at {filePath}");
                return records.ToArray();
            }

            using (StreamReader sr = new StreamReader(filePath, true))
            {
                var line = ReadLine(sr);
                var keys = SplitValuesFromLine(line);
                line = ReadLine(sr);
                while (line != null)
                {
                    var values = SplitValuesFromLine(line).ToArray();
                    if (keys.Length != values.Length)
                    {
                        throw new Exception("Not same length");
                    }

                    records.Add(new CsvRecord
                    {
                        Keys = keys,
                        Values = values
                    });

                    line = ReadLine(sr);
                }
            }

            return records.ToArray();
        }

        public static Task ParseAsync(string filePath, Action<CsvRecord[]> onComplete)
        {
            return Task.Run(() =>
            {
                var records = Parse(filePath);
                onComplete?.Invoke(records);
            });
        }

        private static string ReadLine(StreamReader reader)
        {
            StringBuilder sb = new StringBuilder();
            int intValue = reader.Read();
            while (intValue >= 0)
            {
                char charValue = (char)intValue;

                if ((sb.Length == 0 && (charValue == '\n' || charValue == '\r')))
                {
                    intValue = reader.Read();
                    continue;
                }

                if (charValue == '\n' || charValue == '\r')
                {
                    break;
                }

                sb.Append(charValue);
                intValue = reader.Read();
            }

            return sb.Length > 0 ? sb.ToString() : null;
        }

        private static string[] SplitValuesFromLine(string line)
        {
            List<string> values = new List<string>();

            if (string.IsNullOrEmpty(line))
            {
                return values.ToArray();
            }

            bool insideQuote = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < line.Length; i++)
            {
                char charValue = line[i];

                if (charValue == DoubleQuote)
                {
                    insideQuote = !insideQuote;
                    if (insideQuote)
                        continue;
                }

                if (charValue == Comma && !insideQuote)
                {
                    values.Add(sb.ToString());
                    if (i == line.Length - 1)
                    {
                        values.Add(string.Empty);
                    }

                    sb.Clear();
                    insideQuote = false;
                    continue;
                }

                if (charValue != DoubleQuote)
                    sb.Append(charValue);

                if (i == line.Length - 1)
                {
                    values.Add(sb.ToString());
                    sb.Clear();
                    insideQuote = false;
                }
            }

            return values.ToArray();
        }
    }
}
