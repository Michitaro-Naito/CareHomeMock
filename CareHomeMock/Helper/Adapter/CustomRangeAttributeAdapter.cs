﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Helper.Adapter
{
	public class CustomRangeAttributeAdapter : RangeAttributeAdapter
	{
		public CustomRangeAttributeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)
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
					this.Attribute.ErrorMessageResourceName = "Range";
				}
			}
		}
	}
}