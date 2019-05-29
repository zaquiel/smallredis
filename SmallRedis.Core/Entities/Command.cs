using SmallRedis.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmallRedis.Core.Entities
{
    public class Command
    {
        public OperationEnum Operation { get; set; }
        public List<string> Key { get; set; }
        public object Value { get; set; }
        public int ExSeconds { get; set; }
        public int Start { get; set; }
        public int Stop { get; set; }
        public Dictionary<string, int> MemberScore { get; set; }
        public string Member { get; set; }

        public Command()
        {
            Key = new List<string>();
            MemberScore = new Dictionary<string, int>();
            ExSeconds = -1;
        }

        public override string ToString()
        {
            return $@"Comando:{this.Operation}
                Key: {this.Key}
                Value: {this.Value}
                ExSeconds: {this.ExSeconds}";
        }
    }
}
