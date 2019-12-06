using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;


public partial class MasterPage : System.Web.UI.MasterPage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            actualizarGrid();
            actualizarGridCatalogo();
            actualizarSelectDto();
            iniciarCamposModificar();
            iniciarCamposEliminar();
            iniciarCamposKiosko();
            llenarGridProductos();
            llenarCatalogoCKiosko();
        }
        




    }




    ServiceReference1.Service1Client Cliente = new ServiceReference1.Service1Client();

    //------------ INICIA TAB AGREGAR PRODUCTO--------------------------

    private void actualizarSelectDto()
    {
        var registro = Cliente.ListarDepartamentos();
        foreach (var depto in registro)
        {

            string n = depto.nombre;
            string id = depto.id + "";

            selectDtoAgregar.Items.Add(n);
            selectDtoModificar.Items.Add(n);
            selectDtoEliminar.Items.Add(n);
            selectDtoCKiosko.Items.Add(n);

        }
    }
    protected void btnGuardarProducto_Click(object sender, EventArgs e)
    {

        string codbar = txtCodBarAgregar.Text;
        string nombre = txtNombreAgregar.Text;
        string descripcion = txtDesAgregar.Text;
        string valor = selectUMAgregar.Value;
        string um = ""; int b = 3;
        //if (valor == "0") txtCodBarAgregar.Text = um;
        if (valor == "1") um = "pza";
        else if (valor == "2") um = "kg";
        else b = 0;
        string spv = txtPVAgregar.Text;
        float pv = float.Parse(spv);
        //string dep = selectDtoAgregar.SelectedIndex;
        //int nDep = selectDtoAgregar.SelectedIndex;
        string nDepV = selectDtoAgregar.Value;
        var productos = Cliente.ListarDepartamentos();
        var producto = productos.Where(i => i.nombre == nDepV).FirstOrDefault();
        int nDep = producto.id;


        if (Cliente.GuardarProducto(codbar, nombre, descripcion, um, pv, "Activo", nDep) && b > 0)
        {
            //Console.WriteLine("True");
            //Console.ReadKey();
            txtCodBarAgregar.Text = "";
            txtNombreAgregar.Text = "";
            txtDesAgregar.Text = "";
            selectUMAgregar.SelectedIndex = 0;
            txtPVAgregar.Text = "";
            selectDtoAgregar.SelectedIndex = 0;

        }





    }


    //------------ TERMINAR TAB AGREGAR PRODUCTO--------------------------


    //------------ INICIAR TAB MODIFICAR PRODUCTO--------------------------
    public void iniciarCamposModificar()
    {
        txtCodBarModificar.Text = "";
        txtCodBarModificar.Enabled = true;
        txtNombreModificar.Text = "";
        txtNombreModificar.Enabled = false;
        txtDescModificar.Text = "";
        txtDescModificar.Enabled = false;
        selectUMModificar.SelectedIndex = 0;
        selectUMModificar.Disabled = true;
        txtPreVenModificar.Text = "";
        txtPreVenModificar.Enabled = false;
        selectDtoModificar.SelectedIndex = 0;
        selectDtoModificar.Disabled = true;

    }
    public void activarCamposModificar()
    {
        txtCodBarModificar.Enabled = false;
        txtNombreModificar.Text = "";
        txtNombreModificar.Enabled = true;
        txtDescModificar.Text = "";
        txtDescModificar.Enabled = true;
        selectUMModificar.SelectedIndex = 0;
        selectUMModificar.Disabled = false;
        txtPreVenModificar.Text = "";
        txtPreVenModificar.Enabled = true;
        selectDtoModificar.SelectedIndex = 0;
        selectDtoModificar.Disabled = false;

    }
    protected void btnLimpiarModificar_Click(object sender, EventArgs e)
    {
        iniciarCamposModificar();
    }
    protected void btnBuscarModificar_Click(object sender, EventArgs e)
    {

        string codbar = "";
        codbar = txtCodBarModificar.Text;
        if (codbar.Length > 0)
        {
            try
            {
                activarCamposModificar();

                string producto = Cliente.BuscarProducto(codbar);
                string[] separadas;
                separadas = producto.Split('¬');

                txtNombreModificar.Text = separadas[1];
                txtDescModificar.Text = separadas[2];
                string um = separadas[3];
                if (um == "pza")
                    selectUMModificar.SelectedIndex = 1;
                else selectUMModificar.SelectedIndex = 2;

                txtPreVenModificar.Text = separadas[4];

                int dep = Int32.Parse(separadas[5]);

                selectDtoModificar.SelectedIndex = dep-9;

            }
            catch (Exception ex)
            {
                iniciarCamposModificar();
                string error = "No existe el producto";
                txtNombreModificar.Text = error;

            }

        }
        //string codbar = "";
        //codbar = txtCodBarModificar.Text;
        //if (codbar.Length > 0)
        //{
        //    try
        //    {
        //        //activarCamposModificar();
        //        var productos = Cliente.ListarProductos();
        //        var producto = productos.Where(i => i.codigo_producto == codbar).FirstOrDefault();



        //        //string producto = Cliente.BuscarProducto(codbar);
        //        //string[] separadas;
        //        //separadas = producto.Split('¬');

        //        txtNombreModificar.Text = producto.nombre;
        //        txtDescModificar.Text = producto.descripcion;
        //        string um = producto.unidad_medida;
        //        if (um == "pza")
        //            selectUMModificar.SelectedIndex = 1;
        //        else selectUMModificar.SelectedIndex = 2;

        //        txtPreVenModificar.Text = producto.precio + "";

        //        int dep = producto.fk_id_departamento;

        //        //selectDtoModificar.SelectedIndex = Int32.Parse(dep);


        //        var productos2 = Cliente.ListarDepartamentos();
        //        var producto2 = productos.Where(i => i.id == dep).FirstOrDefault();
        //        string nomDep = producto2.nombre;
        //        selectDtoModificar.Value = nomDep;
        //    }
        //    catch (Exception ex)
        //    {
        //        iniciarCamposModificar();
        //        string error = "No existe el producto";
        //        txtNombreModificar.Text = error;

        //    }

        //}
    }

    protected void btnGuardarModificar_Click(object sender, EventArgs e)
    {
        string codbar = txtCodBarModificar.Text;
        string nombre = txtNombreModificar.Text;
        string descripcion = txtDescModificar.Text;
        string valor = selectUMModificar.Value;
        string um = ""; int b = 3;
        //if (valor == "0") txtCodBarAgregar.Text = um;
        if (valor == "1") um = "pza";
        else if (valor == "2") um = "kg";
        else b = 0;
        string spv = txtPreVenModificar.Text;
        float pv = float.Parse(spv);
        //string dep = selectDtoAgregar.SelectedIndex;
        string nDepV = selectDtoModificar.Value;
        var productos = Cliente.ListarDepartamentos();
        var producto = productos.Where(i => i.nombre == nDepV).FirstOrDefault();
        int nDep = producto.id;

        Cliente.EnviaModificacion(codbar, nombre, descripcion, um, pv, "Activo", nDep);
        iniciarCamposModificar();
        llenarGridProductos();


    }



    //------------ TERMINAR TAB MODIFICAR PRODUCTO--------------------------





    //------------ INICIAR TAB ELIMINAR PRODUCTO-----
    bool existeMP = true;

    public void iniciarCamposEliminar()
    {
        txtCodBarEliminar.Text = "";
        txtCodBarEliminar.Enabled = true;
        txtNombreEliminar.Text = "";
        txtNombreEliminar.Enabled = false;
        txtDescEliminar.Text = "";
        txtDescEliminar.Enabled = false;
        selectUMEliminar.SelectedIndex = 0;
        selectUMEliminar.Disabled = true;
        txtPreVenEliminar.Text = "";
        txtPreVenEliminar.Enabled = false;
        selectDtoEliminar.SelectedIndex = 0;
        selectDtoEliminar.Disabled = true;

    }
    protected void btnBuscarEliminar_Click(object sender, EventArgs e)
    {

        string codbar = "";
        codbar = txtCodBarEliminar.Text;
        if (codbar.Length > 0)
        {
            try
            {
                txtCodBarEliminar.Enabled = false;
                existeMP = true;
                string producto = Cliente.BuscarProducto(codbar);
                string[] separadas;
                separadas = producto.Split('¬');

                txtNombreEliminar.Text = separadas[1];
                txtDescEliminar.Text = separadas[2];

                if (separadas[3] == "pza")
                    selectUMEliminar.SelectedIndex = 1;
                else selectUMEliminar.SelectedIndex = 2;

                txtPreVenEliminar.Text = separadas[4];

                selectDtoEliminar.SelectedIndex = Int32.Parse(separadas[6])-9;
            }
            catch (Exception ex)
            {
                iniciarCamposEliminar();
                existeMP = false;
                string error = "No existe el producto";
                txtNombreEliminar.Text = error;
            }



        }
    }
    protected void btnEliminarEliminar_Click(object sender, EventArgs e)
    {
        if (existeMP == true)
        {
            string codbar = "";
            codbar = txtCodBarEliminar.Text;
            Cliente.BajaProducto(codbar);

            txtCodBarEliminar.Text = "";
            txtNombreEliminar.Text = "";
            txtDescEliminar.Text = "";
            selectUMEliminar.SelectedIndex = 0;
            txtPreVenEliminar.Text = "";
            selectDtoEliminar.SelectedIndex = 0;
            llenarGridProductos();
        }


    }



    //------------ TERMINAR TAB ELIMINAR PRODUCTO--------------------------









    //------------ INICIA TAB DEPARTAMENTOS--------------------------

    private void actualizarGrid()
    {

        var registro = Cliente.ListarDepartamentos();
        listaDepto.DataSource = registro;
        listaDepto.DataBind();


    }

    protected void btnAgregarDep_Click(object sender, EventArgs e)
    {

        string nomDep = txtNomDept.Text;

        if (Cliente.AgregarDepartamento(nomDep, "Activo"))
        {
            txtNomDept.Text = "";
        }

    }



    // ----------- TERMINA TAB DEPARTAMENTOS ------------------------


    protected void btnUpload_Click(object sender, EventArgs e)
    {


        byte[] fileToSend = null;
        string name = "";

        if (img1Agregar.HasFile)
        {
            name = img1Agregar.FileName;
            Stream stream = img1Agregar.FileContent;
            stream.Seek(0, SeekOrigin.Begin);
            fileToSend = new byte[stream.Length];
            int count = 0;
            while (count < stream.Length)
            {
                fileToSend[count++] = Convert.ToByte(stream.ReadByte());
            }
            Stream stream1 = new MemoryStream(fileToSend);

        }

    }





    private void actualizarGridCatalogo()
    {

        var registro = Cliente.ListarDepartamentos();
        gridCatalogo.DataSource = registro;
        gridCatalogo.DataBind();


    }


    //--------------------------------------------------------------------------------------------------------
    //----------------- KIOSKO BUSCAR PRODUCTO ---------------------------------------------------------------
    public void iniciarCamposKiosko()
    {
        lblNomProdKiosko.Text = "Escanee su producto";
        lblDescKiosko.Text = "Luego pulse Buscar";
        lblUMKiosko.Text = "U.M.";
        lblPrecioKiosko.Text = "Precio ";
        lblDepKiosko.Text = "S/N";
    }
    protected void btnBuscarProdKiosko_Click(object sender, EventArgs e)
    {
        string codbar = "";
        codbar = txtCodBarProdKiosko.Text;
        if (codbar.Length > 0)
        {
            try
            {

                string producto = Cliente.BuscarProducto(codbar);
                string[] separadas;
                separadas = producto.Split('¬');
                string status = separadas[5];
                if (status == "Activo")
                {
                    lblNomProdKiosko.Text = separadas[1];
                    lblDescKiosko.Text = separadas[2];
                    lblUMKiosko.Text = separadas[3];
                    lblPrecioKiosko.Text = separadas[4] + " ";

                    selectDtoModificar.SelectedIndex = Int32.Parse(separadas[6])-9;
                    string dep = selectDtoModificar.Value;
                    lblDepKiosko.Text = dep;
                    string url_img = separadas[7];
                    imgKiosko.ImageUrl = url_img;
                }
                else
                {
                    iniciarCamposKiosko();
                    string error = "El producto está inactivo";
                    lblNomProdKiosko.Text = error;
                    lblDescKiosko.Text = "";
                    lblPrecioKiosko.Text = "";
                    lblUMKiosko.Text = "";
                }

            }
            catch (Exception ex)
            {
                iniciarCamposKiosko();
                string error = "No existe el producto";
                lblNomProdKiosko.Text = error;
                lblDescKiosko.Text = "";
                lblPrecioKiosko.Text = "";
                lblUMKiosko.Text = "";

            }

        }
    }

    //--------------------------------------------------------------------------------------------------------
    //----------------- KIOSKO CATALOGO PRODUCTO ---------------------------------------------------------------
    public int eligeDep()
    {
        int nDep = selectDtoCKiosko.SelectedIndex;
        return nDep;
    }
    public void llenarCatalogoCKiosko()
    {
        var productos = Cliente.ListarProductos();

        Repeater1.DataSource = productos;
        Repeater1.DataBind();
    }

    protected void btnBuscarDtoCKiosko_Click(object sender, EventArgs e)
    {

        int nDep = selectDtoCKiosko.SelectedIndex;
        var productos = Cliente.ListarProductos();
        var producto = productos.Where(i => i.fk_id_departamento == (nDep+9)).ToList();

        Repeater1.DataSource = producto;
        Repeater1.DataBind();

    }


    //_------------------------------------PRODUCTOS CATALOGO ----------------------------------------

    public void llenarGridProductos()
    {
        var productos = Cliente.ListarProductos();

        gridCatalogo.DataSource = productos;
        gridCatalogo.DataBind();

    }







}
