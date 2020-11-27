using UnityEngine;
using System.Collections;

namespace Recorrido{

    [RequireComponent(typeof(BoxCollider))]
    public class Escena : MonoBehaviour{
 
        private static Escena instancia = null;

        private BoxCollider escena = null;

        private void Awake(){            
            instancia = null;
            escena     = GetComponent<BoxCollider>();
        }
  
        public Vector3   GetPosicion(){
            return transform.position;
        }
  
        public bool IsMapa(Entidad entidad){            
            return escena.bounds.Contains(entidad.transform.position);
        }
        public static Escena GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<Escena>();
            return instancia;
        }
            
    }

}
