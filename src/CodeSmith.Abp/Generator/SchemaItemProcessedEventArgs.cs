using System;

namespace CodeSmith.Abp.Generator
{
    public class SchemaItemProcessedEventArgs : EventArgs
    {
        public SchemaItemProcessedEventArgs(string name)
        {
            _name = name;
        }

        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }
    }
}