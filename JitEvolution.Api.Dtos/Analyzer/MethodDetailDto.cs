using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class MethodDetailDto
    {
        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public int CyclomaticComplexity { get; set; }

        [DataMember]
        public int EndLine { get; set; }

        [DataMember]
        public bool IsConstructor { get; set; }

        [DataMember]
        public bool IsSetter { get; set; }

        [DataMember]
        public bool IsGetter { get; set; }

        [DataMember]
        public string Kind { get; set; }

        [DataMember]
        public int MaxNestingDepth { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public int NumberOfAccessedVariables { get; set; }

        [DataMember]
        public int NumberOfCalledMethods { get; set; }

        [DataMember]
        public int NumberOfCallers { get; set; }

        [DataMember]
        public int NumberOfInstructors { get; set; }

        [DataMember]
        public int StartLine { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Usr { get; set; }

        [DataMember]
        public int VersionNumber { get; set; }
    }
}
