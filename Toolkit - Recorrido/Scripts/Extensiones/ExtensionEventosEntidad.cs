using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Recorrido{

    public enum EventoEntidad{
        CREAR,DESTRUIR
    }

    [RequireComponent(typeof(Entidad))]
    public class ExtensionEventosEntidad : Extension{

        [Header("Eventos")]
        [SerializeField]
        private UnityEvent crear = new UnityEvent();        
        [SerializeField]
        private UnityEvent destruir = new UnityEvent();

        public void EntidadEvento(EventoEntidad evento){            
            switch(evento){
                case EventoEntidad.CREAR:
                    crear.Invoke();
                    break;                
                case EventoEntidad.DESTRUIR:
                    destruir.Invoke();
                    break;
            }
        }
       
    }

}
