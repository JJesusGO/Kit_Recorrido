using UnityEngine;
using System.Collections;

namespace Recorrido{
    public class Navegador : Entidad{


        [Header("General")]
        [SerializeField]
        private string[] layerdisponible;
        [SerializeField]
        private Transform camara = null;
        [Header("Click")]
        [SerializeField]
        private bool enableclick = true;
        [SerializeField]
        private float radiominimax = 80.0f;
        [SerializeField]
        private float radiomaximax = 400.0f;
        [SerializeField]
        private float radiominimay = 60.0f;
        [SerializeField]
        private float radiomaximay = 300.0f;
        [Header("Movimiento")]
        [SerializeField]
        private bool enablemovimiento = true;
        [SerializeField]
        private string []layermovimiento;
        [SerializeField]
        private float movimientovelocidad = 7.5f;
        [Header("Rotacion")]
        [SerializeField]
        private bool enablerotacion = true;
        [SerializeField]
        private float rotacionminima = 40.0f;
        [SerializeField]
        private float rotacionmaxima = 100.0f;
        [SerializeField]
        private float rotacionaceleracion = 10.0f;
        [Header("Vista")]
        [SerializeField]
        private string[] layervista;
        [SerializeField]
        private float vistaminima = -70.0f;
        [SerializeField]
        private float vistamaxima = 70.0f;
        [SerializeField]
        private float vistarotacionminima = 40.0f;
        [SerializeField]
        private float vistarotacionmaxima = 100.0f;
        [SerializeField]
        private float vistarotacionaceleracion = 10.0f;

        private const float checktiempo = 0.2f;
        private Vector2 inicio,fin,actual;

        private bool  click  = false;
        private bool  check  = false;
        private float tiempo = 0.0f;


        private float rotacion = 0.0f,
                         vista = 0.0f;
        private Vector2 direccion = Vector2.zero;
        private Vector3 posiciontarget = Vector2.zero,
                        posicioninicial = Vector2.zero;

        protected override void Awake(){
            base.Awake();
        }
        protected override void Start(){
            base.Start();
            posiciontarget = transform.position;
            posicioninicial = camara.transform.localPosition;
        }        

        protected override void Update(){
            base.Update();

            UpdateGestos();

            UpdateClick();
            UpdateGirar();
            UpdateVista();

            UpdatePosicion();
                                             
        }
        public override void Generacion(){

        }
        private void UpdateGestos() {
            if (!enableclick)
                return;
            if (Input.GetMouseButtonDown(0)){
                click = true;
                inicio = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (!check && !IsDeslizamiento(20))
                    Click(true);
                click = false;                
            }

            if (click){
                actual = Input.mousePosition;
                tiempo += Time.deltaTime;
                if (tiempo >= checktiempo && !check)
                {
                    fin = actual;
                    check = true;
                }
                else if(!check){
                    fin = actual;
                }
            }
            else {
                tiempo = 0;
                check = false;
            }
        
        }
        private void UpdateClick()
        {
            if (IsClick())
            {
                if (IsDeslizamiento(20))
                {
                    Vector2 direccion = actual - inicio;
                    Vector2 k = Vector2.zero;

                    if (Mathf.Abs(direccion.x) > radiominimax)
                        k.x = Mathf.Clamp(direccion.x / radiomaximax, -1, 1);
                    if (Mathf.Abs(direccion.y) > radiominimay)
                        k.y = Mathf.Clamp(direccion.y / radiomaximay, -1, 1);

                    Girar(k);
                }
                else if (IsCheck())
                    Click();
            }
            else
                Girar(Vector2.zero);
        }
        private void UpdateVista()
        {

            vista += direccion.y * vistarotacionaceleracion;

            if (vista != 0.0f)
            {
                if (Mathf.Abs(vista) < vistarotacionminima)
                    vista = vista / Mathf.Abs(vista) * vistarotacionminima;
                else if (Mathf.Abs(vista) > vistarotacionmaxima)
                    vista = vista / Mathf.Abs(vista) * vistarotacionmaxima;
            }

            camara.Rotate(new Vector3(-vista, 0, 0) * Time.deltaTime);
            float vistax = camara.localRotation.eulerAngles.x;
            if (vistax >= 90.0f)
                vistax = -(360.0f - vistax);

            camara.localRotation = Quaternion.Euler(Mathf.Clamp(vistax, vistaminima, vistamaxima), 0, 0);
        }
        private void UpdateGirar()
        {

            rotacion += direccion.x * rotacionaceleracion;

            if (rotacion != 0.0f)
            {
                if (Mathf.Abs(rotacion) < rotacionminima)
                    rotacion = rotacion / Mathf.Abs(rotacion) * rotacionminima;
                else if (Mathf.Abs(rotacion) > rotacionmaxima)
                    rotacion = rotacion / Mathf.Abs(rotacion) * rotacionmaxima;
            }

            transform.Rotate(new Vector3(0.0f, rotacion * Time.deltaTime, 0.0f));

        }
        private void UpdatePosicion()
        {
            camara.localPosition = posicioninicial;
            transform.position = Vector3.MoveTowards(transform.position, posiciontarget, movimientovelocidad * Time.deltaTime);
        }

        private void Click(bool forzar = false) {

            Ray ray = ManagerAplicacion.GetInstancia().GetCamara().ScreenPointToRay(actual);
            RaycastHit hit;
            int layerMask = LayerMask.GetMask(layerdisponible);

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMask))
            {
                string nombre = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (check || forzar)
                {
                    for (int i = 0; i < layermovimiento.Length; i++)
                    {
                        if (layermovimiento[i].Equals(nombre))
                        {
                            Posicionar(hit.point);
                            break;
                        }
                    }
                    for (int i = 0; i < layervista.Length; i++)
                    {
                        if (layervista[i].Equals(nombre))
                        {
                            Mirar(hit.point);
                            break;
                        }
                    }

                }
                Vector3 posicionmundo = ManagerAplicacion.GetInstancia().GetCamara().ScreenToWorldPoint(Input.mousePosition);
                Debug.DrawRay(posicionmundo, (hit.point - posicionmundo) * hit.distance, Color.yellow);
            }


        }

        private void Girar(Vector2 direccion)
        {
            if (!enablerotacion)
                return;
            if (Mathf.Sign(this.direccion.x) != direccion.x || this.direccion.x == 0.0f)
                rotacion = 0.0f;
            if (Mathf.Sign(this.direccion.y) != direccion.y || this.direccion.y == 0.0f)
                vista = 0.0f;
            this.direccion = direccion;


        }
        private void Posicionar(Vector3 posicion)
        {
            if (!enablemovimiento)
                return;
            posiciontarget = new Vector3(posicion.x, posicion.y, posicion.z);
            camara.localPosition = posicioninicial;
        }
        private void Mirar(Vector3 posicion)
        {
            if (!enablerotacion)
                return;
            transform.LookAt(new Vector3(posicion.x, transform.position.y, posicion.z));
            camara.LookAt(posicion);
        }

        private void ResetMirada()
        {
            if (!enablerotacion)
                return;
            camara.localRotation = Quaternion.Euler(Vector3.zero);
        }
        private void ModGiro(float rotacion)
        {
            if (!enablerotacion)
                return;
            transform.Rotate(0.0f, rotacion, 0.0f);
        }

        #region GESTOS
        private bool IsDeslizamiento(float distancia) {
            return (inicio-fin).magnitude >= distancia;
        }
        private bool IsCheck() {
            return check;
        }
        private bool IsClick(){
            return click;
        }
        #endregion
        public void AccionResetMirada() {
            ResetMirada();
        }
        public void AccionPosicionar(Transform transform) {
            Posicionar(transform.position);
        }
        public void AccionPosicionar(string comando)
        {
            string[] data = comando.Split('_');
            Vector3 posicion = Vector3.zero;
            if (data != null)
                if (data.Length == 3)
                    posicion = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));
            Posicionar(posicion);
        }
        public void AccionGirar(float rotacion){
            ModGiro(rotacion);
        }
        public void AccionMovimientoRelativo(string comando) {

            string[] data = comando.Split('_');
            Vector3 mod = Vector3.zero;
            if (data != null)
                if (data.Length == 3)
                    mod = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));

            Vector3 modrelativo = transform.right*mod.x + transform.forward*mod.z + transform.up*mod.y;
            Posicionar(GetPosicion() + modrelativo);            
        }
        public void AccionMovimiento(string comando){

            string[] data = comando.Split('_');
            Vector3 mod = Vector3.zero;
            if (data != null)
                if (data.Length == 3)
                    mod = new Vector3(float.Parse(data[0]), float.Parse(data[1]), float.Parse(data[2]));

            Posicionar(GetPosicion() + mod);
        }

        public void AccionSetEnableClick(bool enable) {
            enableclick = enable;        
        }
        public void AccionToggleClick(){
            enableclick = !enableclick;
        }
        public void AccionSetEnableMovimiento(bool enable){
            enablemovimiento = enable;
        }
        public void AccionToggleMovimiento(){
            enablemovimiento = !enablemovimiento;
        }
        public void AccionSetEnableRotacion(bool enable)
        {
            enablerotacion = enable;
        }
        public void AccionToggleRotacion()
        {
            enablerotacion = !enablerotacion;
        }

    }

}

