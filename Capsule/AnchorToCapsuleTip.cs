using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]

public class AnchorToCapsuleTip : UnityEngine.MonoBehaviour
{
    [FormerlySerializedAs("_attachedTo")] [ SerializeField] private Capsule _capsule;

    private void Update()
    {
        if (Application.isPlaying == false)
        {
            if (_capsule)
            {
                transform.position = _capsule.endFulcrumendFulcrum;
            }
        }
    }
}