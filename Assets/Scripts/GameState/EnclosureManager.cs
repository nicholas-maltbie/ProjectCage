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
        lionEnclosure.numberOfSpecies = FindObjectsOfType<Lion>().Length;
        lionEnclosure.enclosureSpecies = AnimalSpecies.Lion;
        // lionEnclosure.numberOfSpecies = FindObjectsOfType<Lion>().Length;
    }

}
