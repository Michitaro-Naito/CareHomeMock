using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareHomeMock.Helper.Adapter
{
	public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
		{
			var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
			if (metadata.PropertyName != null)
			{
				var keys = new string[]{
					containerType.FullName.Replace('.','_') + "_" + metadata.PropertyName,	// Werewolf_Models_Room_RoomName
					containerType.Name + "_" + metadata.PropertyName,						// Room_RoomName
					metadata.PropertyName													// RoomName
				};

				// Search localized string for key
				string displayName = null;
				foreach(var key in keys){
					displayName = MyResources.Model.ResourceManager.GetString(key);
					if(displayName != null) break;
				}
				if(displayName == null) displayName = keys.Last();		// Use the last key as DisplayName if not found

				metadata.DisplayName = displayName;						// Overwrite DisplayName
			}
			return metadata;
		}
	}
}