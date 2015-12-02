using System.Collections.ObjectModel;

// ReSharper disable once CheckNamespace
namespace CodeSmith
{
    public class MethodCollection
        : ObservableCollection<Method>
    {
        public bool IsProcessed { get; set; }
    }
}

