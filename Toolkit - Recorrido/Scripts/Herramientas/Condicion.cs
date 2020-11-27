using UnityEngine;
using System.Collections;

namespace Recorrido{

    public enum CondicionEstado{
        MENOR,
        MAYOR,
        MENORIGUAL,
        MAYORIGUAL,
        IGUAL,
        DIFERENTE,
        SIEMPRE
    }

    [System.Serializable]
    public class Condicion{

        [SerializeField]
        private Entidad entidad = null;
        [SerializeField]
        private CondicionEstado estado = CondicionEstado.IGUAL;
        [SerializeField]
        private string valor = "0";
        [Header("Metadato")]
        [SerializeField]
        private string nombre = "Desconocido";

        private bool IsCondicionValor(string valor){
            switch(estado){
                case CondicionEstado.DIFERENTE:
                    return this.valor != valor;
                case CondicionEstado.IGUAL:
                    return this.valor == valor;
                case CondicionEstado.MENOR:
                    return float.Parse(valor) < float.Parse(this.valor);
                case CondicionEstado.MENORIGUAL:
                    return float.Parse(valor) <= float.Parse(this.valor);
                case CondicionEstado.MAYOR:
                    return float.Parse(valor) > float.Parse(this.valor);
                case CondicionEstado.MAYORIGUAL:
                    return float.Parse(valor) >= float.Parse(this.valor);                    
                case CondicionEstado.SIEMPRE:
                    return true; 
            }
            return true;
        }       
        public bool  IsCondicion(){ 
            string valor = "";
            if (entidad != null)
                valor = entidad.GetMetadato(nombre);
            else
                valor = ManagerAplicacion.GetInstancia().GetMetadato(nombre);
            return IsCondicionValor(valor);                                               
        }


    }


}

