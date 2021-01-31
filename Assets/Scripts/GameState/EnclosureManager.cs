using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Animals;
using Mirror;
public class EnclosureManager : NetworkBehaviour
{
    public Enclosure penguinEnclosure, pandaEnclosure, lionEnclosure;
    public bool arePenguinsFound = false, arePandasFound = false, areLionsFound = false;

    private void Start()
    {
        penguinEnclosure.numberOfSpecies = FindObjectsOfType<Penguin>().Length;
        penguinEnclosure.enclosureSpecies = AnimalSpecies.Penguin;
        pandaEnclosure.numberOfSpecies = FindObjectsOfType<Panda>().Length;
        pandaEnclosure.enclosureSpecies = AnimalSpecies.Panda;
        // lionEnclosure.numberOfSpecies = FindObjectsOfType<Lion>().Length;
    }

    public void UpdateCaptureStatus(AnimalSpecies species, bool status)
    {
        switch (species)
        {
            case AnimalSpecies.Penguin:
                arePenguinsFound = status;
                break;
            case AnimalSpecies.Panda:
                arePandasFound = status;
                break;
            case AnimalSpecies.Lion:
                areLionsFound = status;
                break;
        }
        if (isServer)
        {
            if (CheckVictoryState())
            {
                print("Win!");
            }
        }
    }

    public bool CheckVictoryState()
    {
        return arePenguinsFound && arePandasFound;
    }
}
