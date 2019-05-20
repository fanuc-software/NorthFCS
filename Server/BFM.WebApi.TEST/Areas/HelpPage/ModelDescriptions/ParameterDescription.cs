using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BFM.WebApi.Areas.HelpPage.ModelDescriptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ParameterDescription
    {
        /// <summary>
        /// 
        /// </summary>
        public ParameterDescription()
        {
            Annotations = new Collection<ParameterAnnotation>();
        }

        public Collection<ParameterAnnotation> Annotations { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Documentation { get; set; }

        public string Name { get; set; }

        public ModelDescription TypeDescription { get; set; }
    }
}