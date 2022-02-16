using System.Runtime.Serialization;

namespace JitEvolution.Api.Dtos.Analyzer
{
    [DataContract]
    public class AnalyzerResponseDto
    {
        public AnalyzerResponseDto(long id)
        {
            Results = new AnalyzerResponseDto.ResultDto[]
            {
                new AnalyzerResponseDto.ResultDto
                {
                    Data = new AnalyzerResponseDto.ResultDto.DataDto[]
                    {
                        new AnalyzerResponseDto.ResultDto.DataDto
                        {
                            Row = new long[]
                            {
                                id
                            }
                        }
                    }
                }
            };
        }

        [DataMember]
        public ResultDto[] Results { get; set; }

        [DataContract]
        public class ResultDto
        {
            [DataMember]
            public string[] Columns { get; set; }

            [DataMember]
            public DataDto[] Data { get; set; }

            [DataContract]
            public class DataDto
            {
                [DataMember]
                public long[] Row { get; set; }
                [DataMember]
                public string[] Meta { get; set; }
            }
        }
    }
}
