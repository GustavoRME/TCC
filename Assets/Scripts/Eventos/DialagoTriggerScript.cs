using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialagoTriggerScript : MonoBehaviour {

    public DialagoScript dialago;

    public void TriggerDialago() {
        FindObjectOfType<GerenDialogScript>().ComecarDialago(dialago);
    }
}
