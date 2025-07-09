using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacecraftService
{
    private SpacecraftController spacecraftController;

    public void CreateSpacecraft(SpacecraftScriptable spacecraftScriptable)
    {
        RemovePreviousSpacecraft();
        SpacecraftModel spacecraftModel = new SpacecraftModel(spacecraftScriptable);
        spacecraftController = new SpacecraftController(spacecraftModel);
        spacecraftController.Configure();
    }

    public void RemovePreviousSpacecraft()
    {
        if (spacecraftController != null)
        {
            spacecraftController.Destroy();
            spacecraftController = null;
        }
    }

    public SpacecraftController GetSpacecraftController()
    {
        return spacecraftController;
    }
}
