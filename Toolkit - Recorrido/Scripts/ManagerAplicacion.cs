using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Recorrido{

    [System.Serializable]
    public struct AudioPerfil{
        [SerializeField]
        public string nombre;
        [SerializeField]
        public Transform carpeta;
        [SerializeField]
        public Audio  prefab;
    }
    [System.Serializable]
    public struct ClipPerfil{
        [SerializeField]
        public string nombre;
        [SerializeField]
        public AudioClip audio;
    }

    [System.Serializable]
    public class MetaData
    {

        [SerializeField]
        public string nombre = "Desconocido";
        [SerializeField]
        public string valor = "";

        public string GetValor()
        {
            return valor;
        }
        public string GetNombre()
        {
            return nombre;
        }

        public void SetValor(string valor)
        {
            this.valor = valor;
        }
        public void ModValor(float valor)
        {
            float numero = float.Parse(this.valor);
            numero += valor;
            this.valor = numero.ToString();
        }


        public bool IsNombre(string nombre)
        {
            return this.nombre == nombre;
        }

    }


    public class ManagerAplicacion : MonoBehaviour{

        [Header("General")]
        [SerializeField]
        private MetaData[] metadatos = null;
        [Header("Audio")]
        [SerializeField]
        private AudioPerfil []perfiles = null;
        [SerializeField]
        private ClipPerfil[] clips = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventoinicio = null;

        public static ManagerAplicacion instancia;

        private Camera camara = null;

        private void Awake(){
            instancia = this;
            camara = GameObject.FindObjectOfType<Camera>();
        }
        private void Start(){
            eventoinicio.Invoke();
        }

            
        public void PlayAudio(string perfil, AudioClip clip, Vector3 posicion)
        {
            for (int i = 0; i < perfiles.Length; i++)
                if (perfil == perfiles[i].nombre) { 
                    PlayAudio(i, clip, posicion);
                    break;
                }   
        }
        public void PlayAudio(int perfil,AudioClip clip, Vector3 posicion){
            Audio audio = perfiles[perfil].prefab;
            audio.Create(clip,perfiles[perfil].carpeta,posicion);
        }

        public void SetMetadato(string nombre, string valor)
        {
            for (int i = 0; i < metadatos.Length; i++)
            {
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].SetValor(valor);
            }
        }
        public void ModMetadato(string nombre, float valor)
        {
            for (int i = 0; i < metadatos.Length; i++)
            {
                if (metadatos[i].IsNombre(nombre))
                    metadatos[i].ModValor(valor);
            }
        }
        public Camera GetCamara() {
            return camara;
        }
        public string GetMetadato(string nombre)
        {
            for (int i = 0; i < metadatos.Length; i++)
                if (metadatos[i].IsNombre(nombre))
                    return metadatos[i].GetValor();
            return "";
        }

        public void AccionSetMetadato(string comando)
        {

            string[] data = comando.Split('_');
            if (data != null)
                if (data.Length == 2)
                    SetMetadato(data[0], data[1]);

        }
        public void AccionModMetadato(string comando)
        {

            string[] data = comando.Split('_');
            if (data != null)
                if (data.Length == 2)
                    ModMetadato(data[0], float.Parse(data[1]));

        }


        public void AccionLog(string mensaje){
            Debug.Log(mensaje);
        }
        public void AccionPlayAudio(string codigo){

            string[] data = codigo.Split('_');

            if (data.Length < 2)
                return;

            string perfil = data[0],
            clip = data[1];


            AudioClip sonido = null;
            for (int i = 0; i < clips.Length; i++)
                if (clip == clips[i].nombre) {
                    sonido = clips[i].audio;
                    break;
                }            

            for (int i = 0; i < perfiles.Length; i++)
                if (perfil == perfiles[i].nombre)
                {
                    PlayAudio(i, sonido, transform.position);
                    break;
                }
        }

        public void AccionCargarEscena(int escena){
            SceneManager.LoadScene(escena);
        }
        public void AccionCargarEscena(string escena){
            SceneManager.LoadScene(escena);
        }


        public static ManagerAplicacion GetInstancia()
        {
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<ManagerAplicacion>();
            return instancia;
        }


    }

}