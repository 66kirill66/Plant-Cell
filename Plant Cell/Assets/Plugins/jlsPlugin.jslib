mergeInto(LibraryManager.library,
{
  
  callTNITfunction: function() 
  {
    globals.init();
  },

  onResetDone: function() 
  {
    globals.onResetDone();
  },

  EntityClick: function(id) 
  {
    globals.entityClick(id);
  },

  MeetMassege: function(activatorId, otherId, carrierProteinIndex) 
  {
    globals.MeetMessage(activatorId, otherId, carrierProteinIndex);
  },

   DeleteEntity: function(id) 
  {
    globals.DeleteEntity(id);
  },

  


  SendLangCode: function()
	{
		globals.SendLangCode();
	},

  LightStatuUpdate: function(entityId, booleanValue)
	{
		globals.LightStatuUpdate(entityId, booleanValue);
	},

  CarbonDioxideEntersToCell: function(carbonId, cellId)
	{
		globals.CarbonDioxideEntersToCell(carbonId, cellId);
	},

  


  

  UpdateSimulationConfigValues: function(entityId, name, booleanValue)
	{
    var text = UTF8ToString(name)
		globals.UpdateSimulationConfigValues(entityId, text, booleanValue);
	},

  UpdateCytoplasmPressure: function(entityId, type)
	{
    var text = UTF8ToString(type)
		globals.UpdateCytoplasmPressure(entityId, text);
	},

  UpdateMembranePressure: function(entityId, type)
	{
    var text = UTF8ToString(type)
		globals.UpdateMembranePressure(entityId, text);
	},

  SendEvent: function(name)
	{
		var text = UTF8ToString(name)
		globals.SendEvent(text);
	},

  WaterEntersToVacuole:function(waterId, vacuoleId, booleanValue)
	{
		globals.WaterEntersToVacuole(waterId, vacuoleId, booleanValue);
	},




  
  

  




  

  

 



  
});
