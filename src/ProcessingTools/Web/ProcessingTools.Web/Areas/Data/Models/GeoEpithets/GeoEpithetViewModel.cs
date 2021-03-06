﻿namespace ProcessingTools.Web.Areas.Data.Models.GeoEpithets
{
    using System.ComponentModel.DataAnnotations;
    using ProcessingTools.Constants.Data.Geo;
    using Strings = ProcessingTools.Web.Areas.Data.Resources.GeoEpithets.Models_Strings;

    public class GeoEpithetViewModel
    {
        public int Id { get; set; }

        [Display(Name = nameof(Strings.Name), ResourceType = typeof(Strings))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(Strings.NameValidationErrorMessage), ErrorMessageResourceType = typeof(Strings))]
        [StringLength(ValidationConstants.MaximalLengthOfGeoName, ErrorMessageResourceName = nameof(Strings.NameLengthValidationErrorMessage), ErrorMessageResourceType = typeof(Strings), MinimumLength = ValidationConstants.MinimalLengthOfGeoName)]
        public string Name { get; set; }
    }
}
