using UnityEngine;
using System.Collections;


namespace Recorrido
{
    public class ExtensionSeguimientoCamara : Extension
    { 
        private void LateUpdate()
        {
            Vector3 pos = ManagerAplicacion.GetInstancia().GetCamara().transform.position;
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
        }

    }

}
