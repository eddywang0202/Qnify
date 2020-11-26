using Qnify.Model.Table;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Qnify.Model.Response
{
    [DataContract]
    public class UserDemographicModelResponse
    {
        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "un")]
        public string UserName { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        [DataMember(Name = "tsq")]
        public IEnumerable<UserDemographicModel> UserDemographicModels { get; set; }

        public UserDemographicModelResponse()
        {
            UserDemographicModels = new List<UserDemographicModel>();
        }
    }
}
