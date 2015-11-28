using System.Collections.ObjectModel;

namespace CodeSmith.EntityFramework.Model
{
    public class MethodCollection
        : ObservableCollection<Method>
    {
        public bool IsProcessed { get; set; }
    }
}

