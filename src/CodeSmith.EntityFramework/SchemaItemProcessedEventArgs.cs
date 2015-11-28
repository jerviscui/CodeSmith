using System;

namespace CodeSmith.EntityFramework
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