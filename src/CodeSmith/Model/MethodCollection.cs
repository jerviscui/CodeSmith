using System.Collections.ObjectModel;

namespace CodeSmith.Model
{
    public class MethodCollection
        : ObservableCollection<Method>
    {
        public bool IsProcessed { get; set; }
    }
}

