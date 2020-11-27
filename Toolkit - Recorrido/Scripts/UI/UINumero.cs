using UnityEngine;
using System.Collections;
using TMPro;

namespace Recorrido{

    public enum UINumeroTipo{
        FLOAT,INT
    }
        
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UINumero : MonoBehaviour{

        [Header("Metadato")]
        [SerializeField]
        private Entidad entidad = null;
        [SerializeField]
        private string metanombre = "";
        [Header("Condiguración")]
        [SerializeField]
        private UINumeroTipo tipo = UINumeroTipo.FLOAT;
        [SerializeField]
        private string formato = "{0:0.00}";
        [Tooltip("Si la velocidad es menor o igual a cero, se actualiza inmediatamente")]
        [SerializeField]
        private float  velocidad = 0.0f;
       
        private float numero = 0, 
                    uinumero = 0;
        private TextMeshProUGUI uitexto = null;

        private void Awake(){
            uitexto = GetComponent<TextMeshProUGUI>();
            SetUINumero();
        }
        private void Update(){
                   
            SetNumero();
            if(numero!=uinumero){
                if(velocidad!=0.0f)
                    uinumero = Mathf.MoveTowards(uinumero,numero,velocidad);
                Actualizar();
            }


        }

        private void Actualizar(){
            if (tipo == UINumeroTipo.INT)
                uitexto.text = string.Format(formato, (int)uinumero);
            else
                uitexto.text = string.Format(formato, uinumero);
        }
            
        private void SetNumero(){     

            float valor = 0;

            if (entidad != null)
                valor = float.Parse(entidad.GetMetadato(metanombre));
            else 
                valor = float.Parse(ManagerAplicacion.GetInstancia().GetMetadato(metanombre));

            numero = valor;
            if (velocidad <= 0){
                uinumero = numero;
                Actualizar();
            }
        }
        private void SetUINumero(){
            SetNumero();  
            uinumero = numero;
            Actualizar();
        }

    }

}