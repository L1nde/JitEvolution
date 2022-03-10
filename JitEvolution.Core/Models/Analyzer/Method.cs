using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Method : INode, IEquatable<Method?>
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("cyclomatic_complexity")]
        public int CyclomaticComplexity { get; set; }

        [JsonProperty("end_line")]
        public int EndLine { get; set; }

        [JsonProperty("is_constructor")]
        public bool IsConstructor { get; set; }

        [JsonProperty("is_setter")]
        public bool IsSetter { get; set; }

        [JsonProperty("is_getter")]
        public bool IsGetter { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("max_nesting_depth")]
        public int MaxNestingDepth { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number_of_accessed_variables")]
        public int NumberOfAccessedVariables { get; set; }

        [JsonProperty("number_of_called_methods")]
        public int NumberOfCalledMethods { get; set; }

        [JsonProperty("number_of_callers")]
        public int NumberOfCallers { get; set; }

        [JsonProperty("number_of_instructors")]
        public int NumberOfInstructors { get; set; }

        [JsonProperty("start_line")]
        public int StartLine { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("usr")]
        public string Usr { get; set; }

        [JsonProperty("version_number")]
        public int VersionNumber { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("added_on")]
        public int AddedOn { get; set; }

        [JsonProperty("modifier")]
        public string Modifier { get; set; }

        [JsonProperty("calls")]
        public string[] Calls { get; set; }

        [JsonProperty("uses")]
        public string[] Uses { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Method);
        }

        public bool Equals(Method? other)
        {
            return other != null &&
                   Code == other.Code &&
                   CyclomaticComplexity == other.CyclomaticComplexity &&
                   IsConstructor == other.IsConstructor &&
                   IsSetter == other.IsSetter &&
                   IsGetter == other.IsGetter &&
                   Kind == other.Kind &&
                   MaxNestingDepth == other.MaxNestingDepth &&
                   Name == other.Name &&
                   NumberOfAccessedVariables == other.NumberOfAccessedVariables &&
                   NumberOfCalledMethods == other.NumberOfCalledMethods &&
                   NumberOfCallers == other.NumberOfCallers &&
                   NumberOfInstructors == other.NumberOfInstructors &&
                   Type == other.Type &&
                   Usr == other.Usr &&
                   VersionNumber == other.VersionNumber;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Code);
            hash.Add(CyclomaticComplexity);
            hash.Add(IsConstructor);
            hash.Add(IsSetter);
            hash.Add(IsGetter);
            hash.Add(Kind);
            hash.Add(MaxNestingDepth);
            hash.Add(Name);
            hash.Add(NumberOfAccessedVariables);
            hash.Add(NumberOfCalledMethods);
            hash.Add(NumberOfCallers);
            hash.Add(NumberOfInstructors);
            hash.Add(Type);
            hash.Add(Usr);
            hash.Add(VersionNumber);
            return hash.ToHashCode();
        }
    }
}
