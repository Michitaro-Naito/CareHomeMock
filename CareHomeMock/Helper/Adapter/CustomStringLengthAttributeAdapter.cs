using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Helper.Adapter
{
	public class CustomStringLengthAttributeAdapter : StringLengthAttributeAdapter
	{
		public CustomStringLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)
			: base(metadata, context, attribute)
		{
			if (Attribute.ErrorMessage == null)
			{
				if (Attribute.ErrorMessageResourceType == null)
				{
					this.Attribute.ErrorMessageResourceType = typeof(MyResources.Validation);
				}

				if (Attribute.ErrorMessageResourceName == null)
				{
					if (Attribute.MinimumLength > 0) Attribute.ErrorMessageResourceName = "StringLengthBetween";
					else Attribute.ErrorMessageResourceName = "StringLength";
				}
			}
		}
	}
}