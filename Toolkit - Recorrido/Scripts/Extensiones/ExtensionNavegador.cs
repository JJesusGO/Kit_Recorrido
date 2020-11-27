using UnityEngine;
using System.Collections;

namespace Recorrido
{
    public class ExtensionNavegador : Extension{

        private Navegador navegador = null;
        protected override void Awake(){
            base.Awake();
            navegador = GameObject.FindObjectOfType<Navegador>();
        }

        public void AccionDestruir()
        {
            if (navegador == null)
                return;
            navegador.AccionDestruir();
        }
        public void AccionSetMetadato(string comando)
        {

            if (navegador == null)
                return;
            navegador.AccionSetMetadato(comando);

        }
        public void AccionModMetadato(string comando)
        {

            if (navegador == null)
                return;
            navegador.AccionModMetadato(comando);

        }

        public void AccionResetMirada()
        {
            if (navegador == null)
                return;
            navegador.AccionResetMirada();
        }
        public void AccionPosicionar(Transform posicionar)
        {
            if (navegador == null)
                return;
            navegador.AccionPosicionar(posicionar);
        }
        public void AccionPosicionar(string comando)
        {
            if (navegador == null)
                return;
            navegador.AccionPosicionar(comando);
        }
        public void AccionGirar(float rotacion)
        {
            if (navegador == null)
                return;
            navegador.AccionGirar(rotacion);
        }

        public void AccionSetEnableClick(bool enable)
        {
            if(navegador == null)
                return;
            navegador.AccionSetEnableClick(enable);
        }
        public void AccionToggleClick()
        {
            if(navegador == null)
                return;
            navegador.AccionToggleClick();
        }
        public void AccionSetEnableMovimiento(bool enable)
        {
            if(navegador == null)
                return;
            navegador.AccionSetEnableClick(enable);
        }
        public void AccionToggleMovimiento()
        {
            if (navegador == null)
                return;
            navegador.AccionToggleMovimiento();
        }
        public void AccionSetEnableRotacion(bool enable)
        {
            if (navegador == null)
                return;
            navegador.AccionSetEnableRotacion(enable);
        }
        public void AccionToggleRotacion()
        {
            if (navegador == null)
                return;
            navegador.AccionToggleRotacion();
        }

        public void AccionMovimientoRelativo(string codigo)
        {
            if (navegador == null)
                return;
            navegador.AccionMovimientoRelativo(codigo);
        }
        public void AccionMovimiento(string codigo)
        {
            if (navegador == null)
                return;
            navegador.AccionMovimiento(codigo);
        }


    }
}