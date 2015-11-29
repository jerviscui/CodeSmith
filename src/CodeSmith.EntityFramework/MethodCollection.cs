using System.Collections.ObjectModel;

namespace CodeSmith.EntityFramework
{
    public class MethodCollection
        : ObservableCollection<Method>
    {
        public bool IsProcessed { get; set; }
    }
}

