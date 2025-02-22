﻿using System.Collections.Generic;

namespace Ibralogue.Parser
{
    public class Choice: IMetadata
    {
        public string ChoiceName;
        public string LeadingConversationName;
        public Dictionary<string, string> Metadata;
        
        public bool HasMetadata(string key) => 
            Metadata.ContainsKey(key);

        public bool TryGetMetadataValue(string key, out string value)
        {
            if (Metadata.ContainsKey(key) && Metadata[key] != null)
            {
                value = Metadata[key];
                return true;
            }
            value = null;
            return false;
        }
    }
}