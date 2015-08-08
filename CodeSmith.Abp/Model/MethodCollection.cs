using System.Collections.ObjectModel;

namespace CodeSmith.Abp.Model
{
    #region Base

    #endregion

    #region Model

    #endregion

    #region Collections

    public class MethodCollection 
        : ObservableCollection<Method>
    {
        public bool IsProcessed { get; set; }
    }

    #endregion
}

