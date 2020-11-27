using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


namespace Recorrido{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entidad : MonoBehaviour{
           
        [Header("Entidad - General")]
        [SerializeField]
        private bool destruir = true;        
        [SerializeField]
        private MetaData []metadatos = null;

        private List<EntidadModulo> modulos = new List<EntidadModulo>();
        
        protected Animador    animador = null;
                  
        private   Escena        escena  = null;

        private ExtensionEventosEntidad eventos = null;
        private Vector3 direccionoriginal = Vector3.zero;

        public Entidad Create(Transform padre,Vector3 posicion){            
            Entidad instancia = GameObject.Instantiate(gameObject,posicion,Quaternion.identity,padre).GetComponent<Entidad>();
            return instancia;
        }        
        public void    Destruir(){
            if (!destruir)
                return;
            if (eventos != null)
                eventos.EntidadEvento(EventoEntidad.DESTRUIR);
            GameObject.Destroy(gameObject);
        }

        protected virtual void Awake(){
            animador     = GetComponentInChildren<Animador>();
            eventos = GetComponentInChildren<ExtensionEventosEntidad>();            
        }
        protected virtual void Start(){
            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Start();            
            escena = Escena.GetInstancia();
            if (eventos != null)
                eventos.EntidadEvento(EventoEntidad.CREAR);
        }
        protected virtual void Update(){            

            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Update();

            if (escena != null && destruir)
                if (!escena.IsMapa(this))
                    Destruir();       

        }     
       
        public abstract void Generacion();
              
        protected void AddModulo(EntidadModulo modulo){
            if (!modulos.Contains(modulo)){
                modulo.SetEntidad(this);       
                modulos.Add(modulo);
            }
        }

        public    void     SetPosicion(Vector3 posicion){
                transform.position = posicion;
        }
  
        public    void     SetMetadato(string nombre,string valor){
            for (int i = 0; i < metadatos.Length; i++){
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].SetValor(valor);
            }
        }
        public    void     ModMetadato(string nombre,float valor){
            for (int i = 0; i < metadatos.Length; i++){
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].ModValor(valor);
            }
        }
    
        public Animador         GetAnimador(){
            return animador;
        }
        public Vector3          GetPosicion(){
            return transform.position;
        }
        public Vector3 GetDistanciaEspacial(Entidad entidad){
            return (GetPosicion() - entidad.GetPosicion());
        }
        public Vector3 GetDistanciaPlano(Entidad entidad){
            Vector3 distancia = GetDistanciaEspacial(entidad);
            distancia.y = 0;
            return distancia;
        }
        public Vector3 GetDireccionOriginal(){
            return direccionoriginal;
        }

        public string           GetMetadato(string nombre){
            for (int i = 0; i < metadatos.Length; i++)
                if (metadatos[i].IsNombre(nombre))
                    return metadatos[i].GetValor();
            return "";
        }
                          

        public void AccionDestruir(){
            Destruir();
        }
        public void AccionPlayAudio(string codigo){
            if (ManagerAplicacion.GetInstancia() != null)
                ManagerAplicacion.GetInstancia().AccionPlayAudio(codigo);
        }
        public void AccionSetMetadato(string comando){

            string[] data = comando.Split('_');
            if (data != null)
            if (data.Length == 2)
                SetMetadato(data[0],data[1]);            

        }
        public void AccionModMetadato(string comando){

            string[] data = comando.Split('_');
            if (data != null)
            if (data.Length == 2)
                ModMetadato(data[0],float.Parse(data[1]));            

        }            
        public void AccionLog(string mensaje){
            Debug.Log(mensaje);
        }                   

    }
        
}
