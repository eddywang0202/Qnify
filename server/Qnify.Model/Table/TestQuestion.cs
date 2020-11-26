using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class TestQuestion : Question
    {
        public string TestSetTitle { get; set; }
        public uint TestSetOrder { get; set; }
        public uint CellId { get; set; }
        public uint CellRow { get; set; }
        public uint CellColumn { get; set; }
        public string CellPropertyJson { get; set; }
        public string AdditionalCellPropertyJson { get; set; }
        public string ImagePath { get; set; }
        public string CorrectAnswerIds { get; set; }
    }
}
