using System.Collections.ObjectModel;

namespace CodeSmith.Abp0741
{
    public class MethodCollection
        : ObservableCollection<Method>
    {
        public bool IsProcessed { get; set; }
    }
}

