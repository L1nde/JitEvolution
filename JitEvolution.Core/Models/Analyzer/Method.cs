using Newtonsoft.Json;

namespace JitEvolution.Core.Models.Analyzer
{
    public class Method : INode
    {
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
    }
}
