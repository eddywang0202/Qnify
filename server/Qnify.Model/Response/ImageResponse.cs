using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model
{
    [DataContract]
    public class ImageResponse
    {
        /// <summary>
        /// CellImage
        /// </summary>
        [DataMember(Name = "cimg")]
        public List<string> CellImage { get; set; }

        public ImageResponse()
        {
            CellImage = new List<string>();
        }
    }
}
