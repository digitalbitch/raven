using System.Collections.Generic;
using System.IO;

namespace Raven.Utils
{
    public class Data
    {
        public Dictionary<string, string> ExtractWhoisServerList(string filePath)
        {
            Dictionary<string,string> whoisDictionary=new Dictionary<string, string>();
           var fileLines = File.ReadAllLines(filePath);
            foreach (var fileLine in fileLines)
            {
                if (fileLine == string.Empty)
                {
                    continue;
                }
                var parts=fileLine.Split(' ');
                whoisDictionary.Add(parts[0],parts[1]);
            }
            return whoisDictionary;
        }

      

    }
}
