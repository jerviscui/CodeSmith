using System.Collections.Generic;
using System.Diagnostics;

namespace CodeSmith.Abp.Model
{
    [DebuggerDisplay("Other: {OtherEntity}, Property: {OtherPropertyName}, Relationship: {RelationshipName}")]
    public class Relationship : EntityBase
    {
        public Relationship()
        {
            OtherProperties = new List<string>();
            ThisProperties = new List<string>();
        }

        public string RelationshipName { get; set; }

        public string ThisEntity { get; set; }
        public string ThisPropertyName { get; set; }
        public Cardinality ThisCardinality { get; set; }
        public List<string> ThisProperties { get; set; }

        public string OtherEntity { get; set; }
        public string OtherPropertyName { get; set; }
        public Cardinality OtherCardinality { get; set; }
        public List<string> OtherProperties { get; set; }

        public bool? CascadeDelete { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsMapped { get; set; }

        public bool IsManyToMany
        {
            get
            {
                return ThisCardinality == Cardinality.Many
                       && OtherCardinality == Cardinality.Many;
            }
        }

        public bool IsOneToOne
        {
            get
            {
                return ThisCardinality != Cardinality.Many
                       && OtherCardinality != Cardinality.Many;
            }
        }

        public string JoinTable { get; set; }
        public string JoinSchema { get; set; }
        public List<string> JoinThisColumn { get; set; }
        public List<string> JoinOtherColumn { get; set; }

    }
}